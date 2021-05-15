using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public class SqliteCellarRepository : ICellarRepository
    {
        private const int CommandTimeout = 120;

        public SqliteCellarRepository(ICellar cellar, String sqlitePath)
        {
            if (cellar == null)
            {
                throw new ArgumentNullException("Cellar");
            }
            if (String.IsNullOrEmpty(sqlitePath))
            {
                throw new ArgumentNullException("SqlitePath");
            }
            if (!File.Exists(sqlitePath))
            {
                throw new FileNotFoundException(sqlitePath);
            }
            this.Cellar = cellar;
            this.SqlitePath = sqlitePath;
            this.LockFile = new SqliteLockFile(String.Concat(sqlitePath, Config.SqliteLockFileExtension));
        }

        public ICellar Cellar { get; private set; }

        public SqliteLockFile LockFile { get; private set; }

        public String SqlitePath { get; private set; }

        private String GetConnectionString()
        {
            return String.Format("DataSource={0}", this.SqlitePath);
        }

        private SQLiteConnection GetReadableConnection()
        {
            String connectionString = GetConnectionString();
            SQLiteConnection sqliteConnection = new SQLiteConnection(connectionString);
            int retryCount = 0;
            while (sqliteConnection.State != ConnectionState.Open)
            {
                try
                {
                    sqliteConnection.Open();
                    sqliteConnection.DefaultTimeout = CommandTimeout;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    Thread.Sleep(Config.Data.RetryDelay);
                    if (++retryCount > Config.Data.MaxRetries)
                    {
                        break;
                    }
                }
            }
            if (sqliteConnection.State != ConnectionState.Open)
            {
                sqliteConnection.Close();
                return null;
            }
            return sqliteConnection;
        }

        private void CloseReadableConnection(SQLiteConnection sqliteConnection)
        {
            try
            {
                sqliteConnection.Close();
                sqliteConnection.Dispose();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private SQLiteConnection GetWriteableConnection()
        {
            this.LockFile.Lock();
            SQLiteConnection sqliteConnection = GetReadableConnection();
            if (sqliteConnection == null)
            {
                this.LockFile.Unlock();
            }
            return sqliteConnection;
        }

        private void CloseWriteableConnection(SQLiteConnection sqliteConnection)
        {
            CloseReadableConnection(sqliteConnection);
            this.LockFile.Unlock();
        }

        public bool Delete<T>(T ploudObject) where T : IPloudObject
        {
            return this.Delete(ploudObject.Yield());
        }

        public bool Delete<T>(IEnumerable<T> ploudObjects) where T : IPloudObject
        {
            SQLiteConnection sqliteConnection = GetWriteableConnection();
            if (sqliteConnection == null)
            {
                return false;
            }
            SQLiteTransaction sqliteTransaction = sqliteConnection.BeginTransaction();
            foreach (IPloudObject ploudObject in ploudObjects)
            {
                String tableName = ploudObject.GetType().GetTableName();
                using (SQLiteCommand command = sqliteConnection.CreateCommand())
                {
                    command.Transaction = sqliteTransaction;
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = CommandTimeout;
                    command.CommandText = String.Format("delete from \"{0}\" where id = @id", tableName);
                    command.Parameters.AddWithValue("@id", ploudObject.Identifier);
                    command.ExecuteNonQueryWithRetry();
                }
            }
            bool success = false;
            try
            {
                sqliteTransaction.Commit();
                success = true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            this.CloseWriteableConnection(sqliteConnection);
            return success;
        }

        public bool CreateStorage<T>() where T : IPloudObject
        {
            SQLiteConnection sqliteConnection = GetWriteableConnection();
            if (sqliteConnection == null)
            {
                return false;
            }
            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                command.AsCreate(typeof(T));
                command.ExecuteNonQueryWithRetry();
            }
            this.CloseWriteableConnection(sqliteConnection);
            return true;
        }

        public bool Save<T>(T ploudObject) where T : IPloudObject
        {
            return this.Save(ploudObject.Yield());
        }

        public bool Save<T>(IEnumerable<T> ploudObjects) where T : IPloudObject
        {
            SQLiteConnection sqliteConnection = GetWriteableConnection();
            if (sqliteConnection == null)
            {
                return false;
            }
            SQLiteTransaction sqliteTransaction = sqliteConnection.BeginTransaction();
            foreach (IPloudObject ploudObject in ploudObjects)
            {
                String tableName = ploudObject.GetType().GetTableName();
                using (SQLiteCommand command = sqliteConnection.CreateCommand())
                {
                    command.Transaction = sqliteTransaction;
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = CommandTimeout;
                    bool objectExists = command.Exists(ploudObject);
                    if (objectExists)
                    {
                        command.AsUpdate(ploudObject);
                    }
                    else
                    {
                        command.AsInsert(ploudObject);
                    }
                    command.ExecuteNonQueryWithRetry();
                }
            }
            bool success = false;
            try
            {
                sqliteTransaction.Commit();
                success = true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            this.CloseWriteableConnection(sqliteConnection);
            return success;
        }

        public T Get<T>(String identifier) where T : IPloudObject
        {
            SQLiteConnection sqliteConnection = GetReadableConnection();
            T ploudObject = default(T);
            if (sqliteConnection == null)
            {
                return ploudObject;
            }
            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                String tableName = typeof(T).GetTableName();
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                command.CommandText = String.Format("select * from \"{0}\" where id = @id", tableName);
                command.Parameters.AddWithValue("@id", identifier);
                SQLiteDataReader reader = command.ExecuteReaderWithRetry();
                if (reader.Read())
                {
                    ploudObject = this.Cellar.CreateObject<T>();
                    reader.MapDataToObject<T>(ploudObject, this.Cellar.CryptoProvider);
                }
                reader.Close();
            }
            this.CloseReadableConnection(sqliteConnection);
            return ploudObject;
        }

        public T Get<T>(IQuery query) where T : IPloudObject
        {
            SQLiteConnection sqliteConnection = GetReadableConnection();
            T ploudObject = default(T);
            if (sqliteConnection == null)
            {
                return ploudObject;
            }
            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                String tableName = typeof(T).GetTableName();
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                command.CommandText = String.Format("select * from \"{0}\" {1}", tableName, (query == null ? "" : query.ToString()));
                SQLiteDataReader reader = command.ExecuteReaderWithRetry();
                if (reader.Read())
                {
                    ploudObject = this.Cellar.CreateObject<T>();
                    reader.MapDataToObject<T>(ploudObject, this.Cellar.CryptoProvider);
                }
                reader.Close();
            }
            this.CloseReadableConnection(sqliteConnection);
            return ploudObject;
        }

        public PloudObjectCollection<T> GetAll<T>(IQuery query) where T : IPloudObject
        {
            PloudObjectCollection<T> ploudObjects = new PloudObjectCollection<T>();
            SQLiteConnection sqliteConnection = GetReadableConnection();
            if (sqliteConnection == null)
            {
                return ploudObjects;
            }
            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                String tableName = typeof(T).GetTableName();
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                command.CommandText = String.Format("select * from \"{0}\" {1}", tableName, (query == null ? "" : query.ToString()));
                SQLiteDataReader reader = command.ExecuteReaderWithRetry();
                while (reader.Read())
                {
                    T ploudObject = this.Cellar.CreateObject<T>();
                    reader.MapDataToObject<T>(ploudObject, this.Cellar.CryptoProvider);
                    ploudObjects.Add(ploudObject);
                }
                reader.Close();
            }
            this.CloseReadableConnection(sqliteConnection);
            return ploudObjects;
        }

        public PloudObjectCollection<T> GetAll<T>() where T : IPloudObject
        {
            PloudObjectCollection<T> ploudObjects = new PloudObjectCollection<T>();
            SQLiteConnection sqliteConnection = GetReadableConnection();
            if (sqliteConnection == null)
            {
                return ploudObjects;
            }
            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                String tableName = typeof(T).GetTableName();
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                command.CommandText = String.Format("select * from \"{0}\"", tableName);
                SQLiteDataReader reader = command.ExecuteReaderWithRetry();
                while (reader.Read())
                {
                    T ploudObject = this.Cellar.CreateObject<T>();
                    reader.MapDataToObject<T>(ploudObject, this.Cellar.CryptoProvider);
                    ploudObjects.Add(ploudObject);
                }
                reader.Close();
            }
            this.CloseReadableConnection(sqliteConnection);
            return ploudObjects;
        }

        public bool Execute(CellarOperation cellarOperation)
        {
            bool status;
            if (cellarOperation == CellarOperation.Compress)
            {
                status = this.Compress();
            }
            else if (cellarOperation == CellarOperation.Encrypt)
            {
                status = this.Encrypt();
            }
            else if (cellarOperation == CellarOperation.Decrypt)
            {
                status = this.Decrypt();
            }
            else
            {
                throw new NotSupportedException();
            }
            return status;
        }

        private bool Compress()
        {
            SQLiteConnection sqliteConnection = GetWriteableConnection();
            if (sqliteConnection == null)
            {
                return false;
            }
            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                command.CommandText = "VACUUM;";
                command.ExecuteNonQueryWithRetry();
            }
            this.CloseWriteableConnection(sqliteConnection);
            return true;
        }

        private bool Encrypt()
        {
            SQLiteConnection sqliteConnection = GetWriteableConnection();
            if (sqliteConnection == null)
            {
                return false;
            }

            sqliteConnection.BindFunction(new SQLiteFunctionAttribute("ploudEncrypt", 1, FunctionType.Scalar),
                (Func<object[], object>)((object[] args) => this.Cellar.CryptoProvider.Encrypt((string)((object[])args[1])[0])),
                null);

            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                command.Encrypt<Appellation>();
                command.Encrypt<BottleFormat>();
                command.Encrypt<Classification>();
                command.Encrypt<Color>();
                command.Encrypt<Country>();
                command.Encrypt<Document>();
                command.Encrypt<GlobalParameter>();
                command.Encrypt<Grapes>();
                command.Encrypt<Order>();
                command.Encrypt<Owner>();
                command.Encrypt<Rack>();
                command.Encrypt<RackItem>();
                command.Encrypt<Region>();
                command.Encrypt<TastingNotes>();
                command.Encrypt<Vendor>();
                command.Encrypt<Wine>();
            }
            this.CloseWriteableConnection(sqliteConnection);
            return true;
        }

        private bool Decrypt()
        {
            SQLiteConnection sqliteConnection = GetWriteableConnection();
            if (sqliteConnection == null)
            {
                return false;
            }

            sqliteConnection.BindFunction(new SQLiteFunctionAttribute("ploudDecrypt", 1, FunctionType.Scalar),
                (Func<object[], object>)((object[] args) => this.Cellar.CryptoProvider.Decrypt((string)((object[])args[1])[0])),
                null);

            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                command.Decrypt<Appellation>();
                command.Decrypt<BottleFormat>();
                command.Decrypt<Classification>();
                command.Decrypt<Color>();
                command.Decrypt<Country>();
                command.Decrypt<Document>();
                command.Decrypt<GlobalParameter>();
                command.Decrypt<Grapes>();
                command.Decrypt<Order>();
                command.Decrypt<Owner>();
                command.Decrypt<Rack>();
                command.Decrypt<RackItem>();
                command.Decrypt<Region>();
                command.Decrypt<TastingNotes>();
                command.Decrypt<Vendor>();
                command.Decrypt<Wine>();

                command.Parameters.Clear();
                command.CommandText = "DROP TABLE mbw;";
                command.ExecuteNonQueryWithRetry();

                command.CommandText = "VACUUM;";
                command.ExecuteNonQueryWithRetry();
            }
            this.CloseWriteableConnection(sqliteConnection);
            return true;
        }
    }
}
