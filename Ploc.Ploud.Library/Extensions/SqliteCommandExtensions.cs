﻿using System;
using System.Data.SQLite;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public static class SqliteCommandExtensions
    {
        public static int ExecuteNonQueryWithRetry(this SQLiteCommand command)
        {
            int ret = 0;
            int retryCount = 0;
            while (true)
            {
                try
                {
                    ret = command.ExecuteNonQuery();
                    break;
                }
                catch (Exception ex)
                {
                    Logger.Error(new Exception(command.CommandText, ex));
                    Thread.Sleep(Config.Data.RetryDelay);
                    if (++retryCount > Config.Data.MaxRetries)
                    {
                        break;
                    }
                }
            }
            return ret;
        }

        public static async Task<Int32> ExecuteNonQueryWithRetryAsync(this SQLiteCommand command)
        {
            int ret = 0;
            int retryCount = 0;
            while (true)
            {
                try
                {
                    ret = await command.ExecuteNonQueryAsync();
                    break;
                }
                catch (Exception ex)
                {
                    Logger.Error(new Exception(command.CommandText, ex));
                    await Task.Delay(Config.Data.RetryDelay);
                    if (++retryCount > Config.Data.MaxRetries)
                    {
                        break;
                    }
                }
            }
            return ret;
        }

        public static Int64 ExecuteScalarWithRetry(this SQLiteCommand command)
        {
            Int64 ret = 0;
            int retryCount = 0;
            while (true)
            {
                try
                {
                    ret = Convert.ToInt64(command.ExecuteScalar());
                    break;
                }
                catch (Exception ex)
                {
                    Logger.Error(new Exception(command.CommandText, ex));
                    Thread.Sleep(Config.Data.RetryDelay);
                    if (++retryCount > Config.Data.MaxRetries)
                    {
                        break;
                    }
                }
            }
            return ret;
        }

        public static async Task<Int64> ExecuteScalarWithRetryAsync(this SQLiteCommand command)
        {
            Int64 ret = 0;
            int retryCount = 0;
            while (true)
            {
                try
                {
                    ret = Convert.ToInt64(command.ExecuteScalar());
                    break;
                }
                catch (Exception ex)
                {
                    Logger.Error(new Exception(command.CommandText, ex));
                    await Task.Delay(Config.Data.RetryDelay);
                    if (++retryCount > Config.Data.MaxRetries)
                    {
                        break;
                    }
                }
            }
            return ret;
        }

        public static void AsInsert(this SQLiteCommand command, IPloudObject ploudObject)
        {
            command.Parameters.Clear();

            String tableName = ploudObject.GetType().GetTableName();

            StringBuilder columnsBuilder = new StringBuilder();
            StringBuilder valuesBuilder = new StringBuilder();
            columnsBuilder.AppendFormat("INSERT INTO \"{0}\" ", tableName);

            columnsBuilder.Append(" (");
            valuesBuilder.Append(" VALUES (");

            PropertyInfo[] properties = ploudObject.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                DataStoreAttribute dataStoreAttribute = propertyInfo.GetAttribute<DataStoreAttribute>();
                if (dataStoreAttribute == null)
                {
                    continue;
                }

                String paramName = String.Format("@{0}", dataStoreAttribute.Name);

                columnsBuilder.AppendFormat("\"{0}\"", dataStoreAttribute.Name);
                valuesBuilder.AppendFormat(paramName);

                columnsBuilder.Append(",");
                valuesBuilder.Append(",");

                Object rawValue = propertyInfo.GetValue(ploudObject);
                command.Bind(propertyInfo.PropertyType, paramName, rawValue, dataStoreAttribute.IsEncrypted, ploudObject.Cellar.CryptoProvider);
            }

            // Remove last ,
            columnsBuilder.Remove(columnsBuilder.Length - 1, 1);
            valuesBuilder.Remove(valuesBuilder.Length - 1, 1);

            columnsBuilder.Append(")");
            valuesBuilder.Append(")");

            command.CommandText = String.Concat(columnsBuilder, valuesBuilder);
        }

        public static long GetCount<T>(this SQLiteCommand command) where T : IPloudObject
        {
            command.Parameters.Clear();
            String tableName = typeof(T).GetTableName();
            command.CommandText = String.Format("SELECT COUNT(*) FROM \"{0}\" ", tableName);
            return command.ExecuteScalarWithRetry();
        }

        public static async Task<long> GetCountAsync<T>(this SQLiteCommand command) where T : IPloudObject
        {
            command.Parameters.Clear();
            String tableName = typeof(T).GetTableName();
            command.CommandText = String.Format("SELECT COUNT(*) FROM \"{0}\" ", tableName);
            return await command.ExecuteScalarWithRetryAsync();
        }

        public static void Encrypt<T>(this SQLiteCommand command) where T : IPloudObject
        {
            command.Parameters.Clear();
            String tableName = typeof(T).GetTableName();
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("UPDATE \"{0}\" SET ", tableName);
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                DataStoreAttribute dataStoreAttribute = propertyInfo.GetAttribute<DataStoreAttribute>();
                if ((dataStoreAttribute == null)
                    || (!dataStoreAttribute.IsEncrypted))
                {
                    continue;
                }
                queryBuilder.AppendFormat("{0} = ploudEncrypt({1}.{0}),", dataStoreAttribute.Name, tableName);
            }
            queryBuilder.Remove(queryBuilder.Length - 1, 1);
            command.CommandText = queryBuilder.ToString();
            Console.WriteLine(command.CommandText);
            command.ExecuteNonQueryWithRetry();
        }

        public static async Task EncryptAsync<T>(this SQLiteCommand command) where T : IPloudObject
        {
            command.Parameters.Clear();
            String tableName = typeof(T).GetTableName();
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("UPDATE \"{0}\" SET ", tableName);
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                DataStoreAttribute dataStoreAttribute = propertyInfo.GetAttribute<DataStoreAttribute>();
                if ((dataStoreAttribute == null)
                    || (!dataStoreAttribute.IsEncrypted))
                {
                    continue;
                }
                queryBuilder.AppendFormat("{0} = ploudEncrypt({1}.{0}),", dataStoreAttribute.Name, tableName);
            }
            queryBuilder.Remove(queryBuilder.Length - 1, 1);
            command.CommandText = queryBuilder.ToString();
            await command.ExecuteNonQueryWithRetryAsync();
        }

        public static void Decrypt<T>(this SQLiteCommand command) where T : IPloudObject
        {
            command.Parameters.Clear();
            String tableName = typeof(T).GetTableName();
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("UPDATE \"{0}\" SET ", tableName);
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                DataStoreAttribute dataStoreAttribute = propertyInfo.GetAttribute<DataStoreAttribute>();
                if ((dataStoreAttribute == null)
                    || (!dataStoreAttribute.IsEncrypted))
                {
                    continue;
                }
                queryBuilder.AppendFormat("\"{0}\" = ploudDecrypt({0}),", dataStoreAttribute.Name);
            }
            queryBuilder.Remove(queryBuilder.Length - 1, 1);
            command.CommandText = queryBuilder.ToString();
            command.ExecuteNonQueryWithRetry();
        }

        public static async Task DecryptAsync<T>(this SQLiteCommand command) where T : IPloudObject
        {
            command.Parameters.Clear();
            String tableName = typeof(T).GetTableName();
            StringBuilder queryBuilder = new StringBuilder();
            queryBuilder.AppendFormat("UPDATE \"{0}\" SET ", tableName);
            PropertyInfo[] properties = typeof(T).GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                DataStoreAttribute dataStoreAttribute = propertyInfo.GetAttribute<DataStoreAttribute>();
                if ((dataStoreAttribute == null)
                    || (!dataStoreAttribute.IsEncrypted))
                {
                    continue;
                }
                queryBuilder.AppendFormat("\"{0}\" = ploudDecrypt({0}),", dataStoreAttribute.Name);
            }
            queryBuilder.Remove(queryBuilder.Length - 1, 1);
            command.CommandText = queryBuilder.ToString();
            await command.ExecuteNonQueryWithRetryAsync();
        }

        public static void AsUpdate(this SQLiteCommand command, IPloudObject ploudObject)
        {
            command.Parameters.Clear();

            String tableName = ploudObject.GetType().GetTableName();
            StringBuilder valuesBuilder = new StringBuilder();
            StringBuilder whereBuilder = new StringBuilder();
            valuesBuilder.AppendFormat("UPDATE \"{0}\" SET ", tableName);

            bool isWhereClauseSet = false;

            PropertyInfo[] properties = ploudObject.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                DataStoreAttribute dataStoreAttribute = propertyInfo.GetAttribute<DataStoreAttribute>();
                if (dataStoreAttribute == null)
                {
                    continue;
                }

                String paramName = String.Format("@{0}", dataStoreAttribute.Name);

                if (dataStoreAttribute.IsPrimaryKey)
                {
                    whereBuilder.Append(isWhereClauseSet ? " AND " : " WHERE ");
                    whereBuilder.AppendFormat("\"{0}\" = {1} ", dataStoreAttribute.Name, paramName);
                    isWhereClauseSet = true;
                }
                else
                {
                    valuesBuilder.AppendFormat("\"{0}\" = {1},", dataStoreAttribute.Name, paramName);
                }

                Object rawValue = propertyInfo.GetValue(ploudObject);
                command.Bind(propertyInfo.PropertyType, paramName, rawValue, dataStoreAttribute.IsEncrypted, ploudObject.Cellar.CryptoProvider);
            }

            // Remove last ,
            valuesBuilder.Remove(valuesBuilder.Length - 1, 1);
            command.CommandText = String.Concat(valuesBuilder, whereBuilder);
            Console.WriteLine(command.CommandText);
        }

        public static void AsCreate(this SQLiteCommand command, Type ploudObjectType)
        {
            String tableName = ploudObjectType.GetTableName();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.AppendFormat("CREATE TABLE IF NOT EXISTS \"{0}\" (", tableName);

            PropertyInfo[] properties = ploudObjectType.GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                DataStoreAttribute dataStoreAttribute = propertyInfo.GetAttribute<DataStoreAttribute>();
                if (dataStoreAttribute == null)
                {
                    continue;
                }

                sqlBuilder.AppendFormat("\"{0}\" ", dataStoreAttribute.Name);
                if (propertyInfo.PropertyType == typeof(String))
                {
                    sqlBuilder.Append(" TEXT ");
                }
                else
                {
                    sqlBuilder.Append(" NUMERIC ");
                }

                if (dataStoreAttribute.IsPrimaryKey)
                {
                    sqlBuilder.Append(" PRIMARY KEY  NOT NULL ");
                }

                sqlBuilder.Append(",");
            }

            // Remove last ,
            sqlBuilder.Remove(sqlBuilder.Length - 1, 1);
            sqlBuilder.Append(")");
            command.CommandText = sqlBuilder.ToString();
        }

        public static bool Exists(this SQLiteCommand command, IPloudObject ploudObject)
        {
            command.Parameters.Clear();

            String tableName = ploudObject.GetType().GetTableName();
            StringBuilder valuesBuilder = new StringBuilder();
            StringBuilder whereBuilder = new StringBuilder();
            valuesBuilder.AppendFormat("SELECT Count(*) from \"{0}\"", tableName);

            bool isWhereClauseSet = false;

            PropertyInfo[] properties = ploudObject.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                DataStoreAttribute dataStoreAttribute = propertyInfo.GetAttribute<DataStoreAttribute>();
                if ((dataStoreAttribute == null)
                    || (!dataStoreAttribute.IsPrimaryKey))
                {
                    continue;
                }

                String paramName = String.Format("@{0}", dataStoreAttribute.Name);
                whereBuilder.Append(isWhereClauseSet ? " AND " : " WHERE ");
                whereBuilder.AppendFormat("\"{0}\" = {1} ", dataStoreAttribute.Name, paramName);
                isWhereClauseSet = true;
                Object rawValue = propertyInfo.GetValue(ploudObject);
                command.Bind(propertyInfo.PropertyType, paramName, rawValue, dataStoreAttribute.IsEncrypted, ploudObject.Cellar.CryptoProvider);
            }
            command.CommandText = String.Concat(valuesBuilder, whereBuilder);
            long count = command.ExecuteScalarWithRetry();
            return count > 0;
        }

        private static void Bind(this SQLiteCommand command, Type type, String name, Object value, bool encrypt, ICryptoProvider cryptoProvider)
        {
            if (value == null)
            {
                command.Parameters.AddWithValue(name, DBNull.Value);
                return;
            }
            if (type == typeof(DateTime))
            {
                long longValue = ((DateTime)value).GetSecondsSince1970();
                command.Parameters.AddWithValue(name, longValue);
            }
            else if (type.IsEnum)
            {
                int enumValue = (int)value;
                command.Parameters.AddWithValue(name, enumValue);
            }
            else if (type == typeof(byte[]))
            {
                byte[] data = (byte[])value;
                command.Parameters.Add(name, System.Data.DbType.Binary).Value = data;
            }
            else
            {
                command.Parameters.AddWithValue(name, (encrypt ? cryptoProvider.Encrypt(value.ToString()) : value));
            }
        }
    }
}
