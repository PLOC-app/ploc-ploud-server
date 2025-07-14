using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public class SqliteCellarRepository : ICellarRepository
    {
        public ICellar Cellar { get; private set; }

        public SqliteLockFile LockFile { get; private set; }

        public string SqlitePath { get; private set; }

        private const int CommandTimeout = 120;

        public SqliteCellarRepository(ICellar cellar, string sqlitePath)
        {
            if (cellar == null)
            {
                throw new ArgumentNullException("Cellar");
            }

            if (string.IsNullOrEmpty(sqlitePath))
            {
                throw new ArgumentNullException("SqlitePath");
            }
            
            if (!File.Exists(sqlitePath))
            {
                throw new FileNotFoundException(sqlitePath);
            }
            
            this.Cellar = cellar;
            this.SqlitePath = sqlitePath;
            this.LockFile = new SqliteLockFile(string.Concat(sqlitePath, Config.SqliteLockFileExtension));
        }

        private string GetConnectionString()
        {
            return this.GetConnectionString(this.SqlitePath);
        }

        private string GetConnectionString(string sqlitePath)
        {
            return string.Format("DataSource={0}", sqlitePath);
        }

        private SQLiteConnection GetReadableConnection()
        {
            string connectionString = this.GetConnectionString();

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

        private async Task<SQLiteConnection> GetReadableConnectionAsync()
        {
            string connectionString = GetConnectionString();
            SQLiteConnection sqliteConnection = new SQLiteConnection(connectionString);
            int retryCount = 0;
            
            while (sqliteConnection.State != ConnectionState.Open)
            {
                try
                {
                    await sqliteConnection.OpenAsync();
                    sqliteConnection.DefaultTimeout = CommandTimeout;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
            
                    await Task.Delay(Config.Data.RetryDelay);
                    
                    if (++retryCount > Config.Data.MaxRetries)
                    {
                        break;
                    }
                }
            }

            if (sqliteConnection.State != ConnectionState.Open)
            {
                await sqliteConnection.CloseAsync();
            
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

        private async Task CloseReadableConnectionAsync(SQLiteConnection sqliteConnection)
        {
            try
            {
                await sqliteConnection.CloseAsync();
                await sqliteConnection.DisposeAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        private SQLiteConnection GetWriteableConnection()
        {
            bool isLocked = this.LockFile.Lock();

            if (!isLocked)
            {
                return null;
            }
            
            SQLiteConnection sqliteConnection = GetReadableConnection();
            
            if (sqliteConnection == null)
            {
                this.LockFile.Unlock();
            }
            
            return sqliteConnection;
        }

        private async Task<SQLiteConnection> GetWriteableConnectionAsync()
        {
            bool isLocked = this.LockFile.Lock();

            if (!isLocked)
            {
                return null;
            }
            
            SQLiteConnection sqliteConnection = await this.GetReadableConnectionAsync();
            
            if (sqliteConnection == null)
            {
                this.LockFile.Unlock();
            }
            
            return sqliteConnection;
        }

        private void CloseWriteableConnection(SQLiteConnection sqliteConnection)
        {
            this.CloseReadableConnection(sqliteConnection);

            this.LockFile.Unlock();
        }

        private async Task CloseWriteableConnectionAsync(SQLiteConnection sqliteConnection)
        {
            try
            {
                await sqliteConnection.CloseAsync();
                await sqliteConnection.DisposeAsync();
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            this.LockFile.Unlock();
        }

        public bool Delete<T>(T ploudObject) where T : IPloudObject
        {
            return this.Delete(ploudObject.Yield());
        }

        public Task<bool> DeleteAsync<T>(T ploudObject) where T : IPloudObject
        {
            return this.DeleteAsync(ploudObject.Yield());
        }

        public bool Delete<T>(IEnumerable<T> ploudObjects) where T : IPloudObject
        {
            SQLiteConnection sqliteConnection = this.GetWriteableConnection();
            
            if (sqliteConnection == null)
            {
                return false;
            }

            SQLiteTransaction sqliteTransaction = sqliteConnection.BeginTransaction();
            
            foreach (IPloudObject ploudObject in ploudObjects)
            {
                string tableName = ploudObject.GetType().GetTableName();
            
                using (SQLiteCommand command = sqliteConnection.CreateCommand())
                {
                    command.Transaction = sqliteTransaction;
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = CommandTimeout;
                    command.CommandText = string.Format("delete from \"{0}\" where id = @id", tableName);
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

        public async Task<bool> DeleteAsync<T>(IEnumerable<T> ploudObjects) where T : IPloudObject
        {
            SQLiteConnection sqliteConnection = await this.GetWriteableConnectionAsync();
            
            if (sqliteConnection == null)
            {
                return false;
            }
            
            SQLiteTransaction sqliteTransaction = sqliteConnection.BeginTransaction();
            
            foreach (IPloudObject ploudObject in ploudObjects)
            {
                string tableName = ploudObject.GetType().GetTableName();
                
                using (SQLiteCommand command = sqliteConnection.CreateCommand())
                {
                    command.Transaction = sqliteTransaction;
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = CommandTimeout;
                    command.CommandText = string.Format("delete from \"{0}\" where id = @id", tableName);
                    command.Parameters.AddWithValue("@id", ploudObject.Identifier);

                    await command.ExecuteNonQueryWithRetryAsync();
                }
            }

            bool success = false;
            
            try
            {
                await sqliteTransaction.CommitAsync();

                success = true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
            
            await this.CloseWriteableConnectionAsync(sqliteConnection);
            
            return success;
        }

        public bool CreateStorage<T>() where T : IPloudObject
        {
            SQLiteConnection sqliteConnection = this.GetWriteableConnection();

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

        public Task<bool> SaveAsync<T>(T ploudObject) where T : IPloudObject
        {
            return this.SaveAsync(ploudObject.Yield());
        }

        public bool Save<T>(IEnumerable<T> ploudObjects) where T : IPloudObject
        {
            SQLiteConnection sqliteConnection = this.GetWriteableConnection();

            if (sqliteConnection == null)
            {
                return false;
            }

            SQLiteTransaction sqliteTransaction = sqliteConnection.BeginTransaction();
            
            foreach (IPloudObject ploudObject in ploudObjects)
            {
                string tableName = ploudObject.GetType().GetTableName();
                
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

        public async Task<bool> SaveAsync<T>(IEnumerable<T> ploudObjects) where T : IPloudObject
        {
            SQLiteConnection sqliteConnection = await this.GetWriteableConnectionAsync();

            if (sqliteConnection == null)
            {
                return false;
            }
            
            SQLiteTransaction sqliteTransaction = sqliteConnection.BeginTransaction();
            
            foreach (IPloudObject ploudObject in ploudObjects)
            {
                string tableName = ploudObject.GetType().GetTableName();
                
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

                    await command.ExecuteNonQueryWithRetryAsync();
                }
            }

            bool success = false;
            
            try
            {
                await sqliteTransaction.CommitAsync();

                success = true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            await this.CloseWriteableConnectionAsync(sqliteConnection);
            
            return success;
        }

        public IPloudObject Get(string identifier, Type ploudObjectType)
        {
            SQLiteConnection sqliteConnection = this.GetReadableConnection();

            IPloudObject ploudObject = null;
            
            if (sqliteConnection == null)
            {
                return ploudObject;
            }

            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                string tableName = ploudObjectType.GetTableName();
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                command.CommandText = string.Format("select * from \"{0}\" where id = @id", tableName);
                command.Parameters.AddWithValue("@id", identifier);
                
                DbDataReader reader = command.ExecuteReaderWithRetry();

                if (reader.Read())
                {
                    ploudObject = this.Cellar.CreateObject(ploudObjectType);
                    reader.MapDataToObject(ploudObject, this.Cellar.CryptoProvider, true);
                }
                
                reader.Close();
            }

            this.CloseReadableConnection(sqliteConnection);
            
            return ploudObject;
        }

        public T Get<T>(string identifier) where T : IPloudObject
        {
            SQLiteConnection sqliteConnection = this.GetReadableConnection();

            T ploudObject = default(T);

            if (sqliteConnection == null)
            {
                return ploudObject;
            }
            
            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                string tableName = typeof(T).GetTableName();
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                command.CommandText = string.Format("select * from \"{0}\" where id = @id", tableName);
                command.Parameters.AddWithValue("@id", identifier);
                
                DbDataReader reader = command.ExecuteReaderWithRetry();
                
                if (reader.Read())
                {
                    ploudObject = this.Cellar.CreateObject<T>();
                    reader.MapDataToObject(ploudObject, this.Cellar.CryptoProvider, true);
                }

                reader.Close();
            }

            this.CloseReadableConnection(sqliteConnection);
            
            return ploudObject;
        }

        public async Task<T> GetAsync<T>(string identifier) where T : IPloudObject
        {
            SQLiteConnection sqliteConnection = await this.GetReadableConnectionAsync();
            
            T ploudObject = default(T);
            
            if (sqliteConnection == null)
            {
                return ploudObject;
            }
            
            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                string tableName = typeof(T).GetTableName();
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                command.CommandText = string.Format("select * from \"{0}\" where id = @id", tableName);
                command.Parameters.AddWithValue("@id", identifier);
                
                DbDataReader reader = await command.ExecuteReaderWithRetryAsync();
                
                if (reader.Read())
                {
                    ploudObject = this.Cellar.CreateObject<T>();
                    reader.MapDataToObject(ploudObject, this.Cellar.CryptoProvider, true);
                }

                reader.Close();
            }

            await this.CloseReadableConnectionAsync(sqliteConnection);
            
            return ploudObject;
        }

        public T Get<T>(IQuery query) where T : IPloudObject
        {
            SQLiteConnection sqliteConnection = this.GetReadableConnection();

            T ploudObject = default(T);
            
            if (sqliteConnection == null)
            {
                return ploudObject;
            }

            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                string tableName = typeof(T).GetTableName();
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                command.CommandText = string.Format("select * from \"{0}\" {1}", tableName, (query == null ? "" : query.ToString()));
                
                DbDataReader reader = command.ExecuteReaderWithRetry();
                if (reader.Read())
                {
                    ploudObject = this.Cellar.CreateObject<T>();
                    reader.MapDataToObject(ploudObject, this.Cellar.CryptoProvider, true);
                }
                
                reader.Close();
            }

            this.CloseReadableConnection(sqliteConnection);
            
            return ploudObject;
        }

        public async Task<T> GetAsync<T>(IQuery query) where T : IPloudObject
        {
            SQLiteConnection sqliteConnection = await this.GetReadableConnectionAsync();
            
            T ploudObject = default(T);

            if (sqliteConnection == null)
            {
                return ploudObject;
            }

            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                string tableName = typeof(T).GetTableName();
                
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                command.CommandText = string.Format("select * from \"{0}\" {1}", tableName, (query == null ? "" : query.ToString()));
                
                DbDataReader reader = await command.ExecuteReaderWithRetryAsync();
                if (reader.Read())
                {
                    ploudObject = this.Cellar.CreateObject<T>();
                    reader.MapDataToObject(ploudObject, this.Cellar.CryptoProvider, true);
                }
                
                reader.Close();
            }

            await this.CloseReadableConnectionAsync(sqliteConnection);
            
            return ploudObject;
        }

        public PloudObjectCollection<T> GetAll<T>(IQuery query) where T : IPloudObject
        {
            PloudObjectCollection<T> ploudObjects = new PloudObjectCollection<T>();
            SQLiteConnection sqliteConnection = this.GetReadableConnection();
            
            if (sqliteConnection == null)
            {
                return ploudObjects;
            }
            
            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                string tableName = typeof(T).GetTableName();
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                command.CommandText = string.Format("select * from \"{0}\" {1}", tableName, (query == null ? "" : query.ToString()));
                
                DbDataReader reader = command.ExecuteReaderWithRetry();
                
                while (reader.Read())
                {
                    T ploudObject = this.Cellar.CreateObject<T>();
                    reader.MapDataToObject(ploudObject, this.Cellar.CryptoProvider);
                    ploudObjects.Add(ploudObject);
                }

                reader.Close();
            }

            this.CloseReadableConnection(sqliteConnection);
            
            return ploudObjects;
        }

        public async Task<PloudObjectCollection<T>> GetAllAsync<T>(IQuery query) where T : IPloudObject
        {
            PloudObjectCollection<T> ploudObjects = new PloudObjectCollection<T>();
            SQLiteConnection sqliteConnection = await this.GetReadableConnectionAsync();

            if (sqliteConnection == null)
            {
                return ploudObjects;
            }

            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                string tableName = typeof(T).GetTableName();
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                command.CommandText = string.Format("select * from \"{0}\" {1}", tableName, (query == null ? "" : query.ToString()));
                DbDataReader reader = await command.ExecuteReaderWithRetryAsync();

                while (reader.Read())
                {
                    T ploudObject = this.Cellar.CreateObject<T>();
                    reader.MapDataToObject(ploudObject, this.Cellar.CryptoProvider);
                    ploudObjects.Add(ploudObject);
                }

                reader.Close();
            }
            
            await this.CloseReadableConnectionAsync(sqliteConnection);

            return ploudObjects;
        }

        public PloudObjectCollection<T> GetAll<T>() where T : IPloudObject
        {
            PloudObjectCollection<T> ploudObjects = new PloudObjectCollection<T>();
            SQLiteConnection sqliteConnection = this.GetReadableConnection();

            if (sqliteConnection == null)
            {
                return ploudObjects;
            }

            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                string tableName = typeof(T).GetTableName();
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                command.CommandText = string.Format("select * from \"{0}\"", tableName);
                
                DbDataReader reader = command.ExecuteReaderWithRetry();
                
                while (reader.Read())
                {
                    T ploudObject = this.Cellar.CreateObject<T>();
                    reader.MapDataToObject(ploudObject, this.Cellar.CryptoProvider);
                    ploudObjects.Add(ploudObject);
                }

                reader.Close();
            }

            this.CloseReadableConnection(sqliteConnection);
            
            return ploudObjects;
        }

        public async Task<PloudObjectCollection<T>> GetAllAsync<T>() where T : IPloudObject
        {
            PloudObjectCollection<T> ploudObjects = new PloudObjectCollection<T>();
            SQLiteConnection sqliteConnection = await this.GetReadableConnectionAsync();
            
            if (sqliteConnection == null)
            {
                return ploudObjects;
            }
            
            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                string tableName = typeof(T).GetTableName();
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                command.CommandText = string.Format("select * from \"{0}\"", tableName);
                
                DbDataReader reader = await command.ExecuteReaderWithRetryAsync();
                
                while (reader.Read())
                {
                    T ploudObject = this.Cellar.CreateObject<T>();
                    reader.MapDataToObject(ploudObject, this.Cellar.CryptoProvider);
                    ploudObjects.Add(ploudObject);
                }

                reader.Close();
            }

            await this.CloseReadableConnectionAsync(sqliteConnection);
            
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
            else if (cellarOperation == CellarOperation.Validate)
            {
                status = this.Validate();
            }
            else if (cellarOperation == CellarOperation.ClearTimestamp)
            {
                status = this.ClearTimestamp();
            }
            else
            {
                throw new NotSupportedException();
            }

            return status;
        }

        public async Task<bool> ExecuteAsync(CellarOperation cellarOperation)
        {
            bool status;

            if (cellarOperation == CellarOperation.Compress)
            {
                status = await this.CompressAsync();
            }
            else if (cellarOperation == CellarOperation.Encrypt)
            {
                status = await this.EncryptAsync();
            }
            else if (cellarOperation == CellarOperation.Decrypt)
            {
                status = await this.DecryptAsync();
            }
            else if (cellarOperation == CellarOperation.Validate)
            {
                status = await this.ValidateAsync();
            }
            else if (cellarOperation == CellarOperation.ClearTimestamp)
            {
                status = await this.ClearTimestampAsync();
            }
            else
            {
                throw new NotSupportedException();
            }

            return status;
        }

        private bool Compress()
        {
            SQLiteConnection sqliteConnection = this.GetWriteableConnection();

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

        private async Task<bool> CompressAsync()
        {
            SQLiteConnection sqliteConnection = await this.GetWriteableConnectionAsync();
            
            if (sqliteConnection == null)
            {
                return false;
            }

            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                command.CommandText = "VACUUM;";

                await command.ExecuteNonQueryWithRetryAsync();
            }

            await this.CloseWriteableConnectionAsync(sqliteConnection);
            
            return true;
        }

        private bool Validate()
        {
            SQLiteConnection sqliteConnection = this.GetReadableConnection();

            if (sqliteConnection == null)
            {
                return false;
            }
            
            long count = 0;
            
            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                command.CommandText = "SELECT count(*) from sqlite_master where name in ('rackitem', 'wine', 'globalparameter');";
                count = command.ExecuteScalarWithRetry();
            }

            this.CloseReadableConnection(sqliteConnection);
            
            return count == 3;
        }

        private async Task<bool> ValidateAsync()
        {
            SQLiteConnection sqliteConnection = await this.GetReadableConnectionAsync();

            if (sqliteConnection == null)
            {
                return false;
            }
            
            long count = 0;
            
            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                command.CommandText = "SELECT count(*) from sqlite_master where name in ('rackitem', 'wine', 'globalparameter');";

                count = await command.ExecuteScalarWithRetryAsync();
            }

            await this.CloseReadableConnectionAsync(sqliteConnection);
            
            return count == 3;
        }

        private bool ClearTimestamp()
        {
            SQLiteConnection sqliteConnection = this.GetWriteableConnection();

            if (sqliteConnection == null)
            {
                return false;
            }
            
            long millisecondsSince1970 = DateTime.UtcNow.AddDays(-1).GetMillisecondsSince1970();

            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                foreach (string tableName in Config.Data.TableNames)
                {
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = CommandTimeout;
                    command.CommandText = string.Format("update \"{0}\" set tp = {1}", tableName, millisecondsSince1970);

                    command.ExecuteNonQueryWithRetry();
                }
            }

            this.CloseWriteableConnection(sqliteConnection);
            
            return true;
        }

        private async Task<bool> ClearTimestampAsync()
        {
            SQLiteConnection sqliteConnection = await this.GetWriteableConnectionAsync();

            if (sqliteConnection == null)
            {
                return false;
            }
            
            long millisecondsSince1970 = DateTime.UtcNow.AddDays(-1).GetMillisecondsSince1970();
            
            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                foreach (string tableName in Config.Data.TableNames)
                {
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = CommandTimeout;
                    command.CommandText = string.Format("update \"{0}\" set tp = {1}", tableName, millisecondsSince1970);

                    await command.ExecuteNonQueryWithRetryAsync();
                }
            }

            await this.CloseWriteableConnectionAsync(sqliteConnection);
            
            return true;
        }

        private bool Encrypt()
        {
            SQLiteConnection sqliteConnection = this.GetWriteableConnection();

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

        private async Task<bool> EncryptAsync()
        {
            SQLiteConnection sqliteConnection = await this.GetWriteableConnectionAsync();
            
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

                await command.EncryptAsync<Appellation>();
                await command.EncryptAsync<BottleFormat>();
                await command.EncryptAsync<Classification>();
                await command.EncryptAsync<Color>();
                await command.EncryptAsync<Country>();
                await command.EncryptAsync<Document>();
                await command.EncryptAsync<GlobalParameter>();
                await command.EncryptAsync<Grapes>();
                await command.EncryptAsync<Order>();
                await command.EncryptAsync<Owner>();
                await command.EncryptAsync<Rack>();
                await command.EncryptAsync<RackItem>();
                await command.EncryptAsync<Region>();
                await command.EncryptAsync<TastingNotes>();
                await command.EncryptAsync<Vendor>();
                await command.EncryptAsync<Wine>();
            }

            await this.CloseWriteableConnectionAsync(sqliteConnection);
            
            return true;
        }

        private bool Decrypt()
        {
            SQLiteConnection sqliteConnection = this.GetWriteableConnection();

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

        private async Task<bool> DecryptAsync()
        {
            SQLiteConnection sqliteConnection = await this.GetWriteableConnectionAsync();

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

                await command.DecryptAsync<Appellation>();
                await command.DecryptAsync<BottleFormat>();
                await command.DecryptAsync<Classification>();
                await command.DecryptAsync<Color>();
                await command.DecryptAsync<Country>();
                await command.DecryptAsync<Document>();
                await command.DecryptAsync<GlobalParameter>();
                await command.DecryptAsync<Grapes>();
                await command.DecryptAsync<Order>();
                await command.DecryptAsync<Owner>();
                await command.DecryptAsync<Rack>();
                await command.DecryptAsync<RackItem>();
                await command.DecryptAsync<Region>();
                await command.DecryptAsync<TastingNotes>();
                await command.DecryptAsync<Vendor>();
                await command.DecryptAsync<Wine>();

                command.Parameters.Clear();
                command.CommandText = "DELETE FROM mbw;";
                await command.ExecuteNonQueryWithRetryAsync();

                command.Parameters.Clear();
                command.CommandText = "DELETE FROM globalparameter where title = 'deviceIdentifier';";
                await command.ExecuteNonQueryWithRetryAsync();

                command.CommandText = "VACUUM;";
                await command.ExecuteNonQueryWithRetryAsync();
            }

            await this.CloseWriteableConnectionAsync(sqliteConnection);
            
            return true;
        }

        public bool CopyTo(string targetCellarPath)
        {
            SQLiteConnection sqliteConnection = this.GetWriteableConnection();

            if (sqliteConnection == null)
            {
                return false;
            }
            
            bool status = false;
            
            try
            {
                using (SQLiteConnection destConnection = new SQLiteConnection(GetConnectionString(targetCellarPath)))
                {
                    destConnection.Open();
                    sqliteConnection.BackupDatabase(destConnection, "main", "main", -1, null, -1);
                    destConnection.Close();
                }

                status = true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            this.CloseWriteableConnection(sqliteConnection);
            
            return status;
        }

        public async Task<bool> CopyToAsync(string targetCellarPath)
        {
            SQLiteConnection sqliteConnection = await this.GetWriteableConnectionAsync();

            if (sqliteConnection == null)
            {
                return false;
            }

            bool status = false;
            
            try
            {
                using (SQLiteConnection destConnection = new SQLiteConnection(GetConnectionString(targetCellarPath)))
                {
                    destConnection.Open();
                    sqliteConnection.BackupDatabase(destConnection, "main", "main", -1, null, -1);
                    destConnection.Close();
                }

                status = true;
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }

            await this.CloseWriteableConnectionAsync(sqliteConnection);
            
            return status;
        }

        public Dashboard GetDashboard()
        {
            SQLiteConnection sqliteConnection = this.GetReadableConnection();

            if (sqliteConnection == null)
            {
                return null;
            }
            
            Dashboard dashboard = new Dashboard();
            
            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;
                dashboard.Wines = command.GetCount<Wine>();
                dashboard.Cellars = command.GetCount<Rack>();
                dashboard.TastingNotes = command.GetCount<TastingNotes>();
            }

            dashboard.FileSize = new FileInfo(this.SqlitePath).Length;
            
            this.CloseReadableConnection(sqliteConnection);
            
            return dashboard;
        }

        public async Task<Dashboard> GetDashboardAsync()
        {
            SQLiteConnection sqliteConnection = await this.GetReadableConnectionAsync();
            
            if (sqliteConnection == null)
            {
                return null;
            }
            
            Dashboard dashboard = new Dashboard();
            
            using (SQLiteCommand command = sqliteConnection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandTimeout = CommandTimeout;

                dashboard.Wines = await command.GetCountAsync<Wine>();
                dashboard.Cellars = await command.GetCountAsync<Rack>();
                dashboard.TastingNotes = await command.GetCountAsync<TastingNotes>();
            }

            dashboard.FileSize = new FileInfo(this.SqlitePath).Length;
            
            await this.CloseReadableConnectionAsync(sqliteConnection);
            
            return dashboard;
        }
    }
}
