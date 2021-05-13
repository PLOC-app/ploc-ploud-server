using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public static class SqliteDataReaderExtensions
    {
        public static void MapDataToObject<T>(this SQLiteDataReader dataReader, T obj, ICryptoProvider cryptoProvider)
        {
            Type typeOfT = typeof(T);
            PropertyInfo[] properties = typeOfT.GetProperties(); // TODO à mettre en cache
            foreach (PropertyInfo propertyInfo in properties)
            {
                Console.WriteLine("{0}", propertyInfo.Name);
                DataStoreAttribute dataStoreAttribute = propertyInfo.GetAttribute<DataStoreAttribute>();
                if(dataStoreAttribute == null)
                {
                    Console.WriteLine("\tDataStoreAttribute == NULL");
                    continue;
                }
                int ordinal = dataReader.GetOrdinal(dataStoreAttribute.Name);
                if(ordinal == -1)
                {
                    Console.WriteLine("\tOrdinal == -1");
                    continue;
                }
                Object rawValue = dataReader[ordinal];
                if(rawValue == null)
                {
                    Console.WriteLine("\tRawValue == NULL");
                    continue;
                }
                if (propertyInfo.PropertyType.IsEnum)
                {
                    propertyInfo.SetValue(obj, Convert.ToInt32(rawValue));
                }
                else if (propertyInfo.PropertyType == typeof(DateTime))
                {
                    long longValue = Convert.ToInt64(rawValue);
                    propertyInfo.SetValue(obj, longValue.DateTimeValue());
                }
                else if (propertyInfo.PropertyType == typeof(Boolean))
                {
                    int intValue = Convert.ToInt32(rawValue);
                    propertyInfo.SetValue(obj, intValue == 0 ? false : true);
                }
                else if ((propertyInfo.PropertyType == typeof(Int16))
                    | (propertyInfo.PropertyType == typeof(Int32))
                    | (propertyInfo.PropertyType == typeof(Int64)))
                {
                    Int64 longValue = Convert.ToInt64(rawValue);
                    propertyInfo.SetValue(obj, longValue);
                }
                else if ((propertyInfo.PropertyType == typeof(Single))
                    | (propertyInfo.PropertyType == typeof(Double)))
                {
                    double doubleValue = Convert.ToDouble(rawValue);
                    propertyInfo.SetValue(obj, doubleValue);
                } 
                else if (propertyInfo.PropertyType == typeof(String))
                {
                    if(dataStoreAttribute.IsEncrypted)
                    {
                        propertyInfo.SetValue(obj, cryptoProvider.Decrypt(rawValue.ToString()));
                    } 
                    else
                    {
                        propertyInfo.SetValue(obj, rawValue.ToString());
                    }
                    
                }
                else
                {
                    throw new NotImplementedException();
                }
            }
        }

        public static SQLiteDataReader ExecuteReaderWithRetry(this SQLiteCommand command)
        {
            SQLiteDataReader sqliteDataReader = null;
            int retryCount = 0;
            while (true)
            {
                try
                {
                    sqliteDataReader = command.ExecuteReader();
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
            return sqliteDataReader;
        }
    }
}
