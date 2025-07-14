using System;
using System.Data.Common;
using System.Data.SQLite;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public static class DbDataReaderExtensions
    {
        public static void MapDataToObject(this DbDataReader dataReader, IPloudObject obj, ICryptoProvider cryptoProvider)
        {
            MapDataToObject(dataReader, obj, cryptoProvider, false);
        }

        public static void MapDataToObject(this DbDataReader dataReader, IPloudObject obj, ICryptoProvider cryptoProvider, bool loadBinaryData)
        {
            Type typeOfT = obj.GetType();
            PropertyInfo[] properties = typeOfT.GetProperties(); // TODO à mettre en cache
            
            foreach (PropertyInfo propertyInfo in properties)
            {
                Console.WriteLine("{0}", propertyInfo.Name);
                DataStoreAttribute dataStoreAttribute = propertyInfo.GetAttribute<DataStoreAttribute>();
            
                if (dataStoreAttribute == null)
                {
                    Console.WriteLine("\tDataStoreAttribute == NULL");
                    continue;
                }
                
                int ordinal = dataReader.GetOrdinal(dataStoreAttribute.Name);
                
                if (ordinal == -1)
                {
                    Console.WriteLine("\tOrdinal == -1");
                    continue;
                }
                
                object rawValue = dataReader[ordinal];
                
                if (rawValue == null | rawValue == DBNull.Value)
                {
                    Console.WriteLine("\tRawValue == NULL");
                    continue;
                }

                Console.WriteLine("\t{0}", dataStoreAttribute.Name);
                
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
                else if (propertyInfo.PropertyType == typeof(Int16))
                {
                    TryConvert(() =>
                    {
                        Int16 shortValue = Convert.ToInt16(rawValue);
                        propertyInfo.SetValue(obj, shortValue);

                    }, typeOfT, propertyInfo.Name, rawValue);
                }
                else if (propertyInfo.PropertyType == typeof(Int32))
                {
                    TryConvert(() =>
                    {
                        long longValue = Convert.ToInt64(rawValue);
                        // Fix iOS Bug PLOC < v5.0
                        if (longValue >= Int32.MinValue && longValue <= Int32.MaxValue)
                        {
                            Int32 intValue = (Int32)(longValue);
                            propertyInfo.SetValue(obj, intValue);
                        }
                    }, typeOfT, propertyInfo.Name, rawValue);
                }
                else if (propertyInfo.PropertyType == typeof(Int64))
                {
                    TryConvert(() =>
                    {
                        Int64 longValue = Convert.ToInt64(rawValue);
                        propertyInfo.SetValue(obj, longValue);

                    }, typeOfT, propertyInfo.Name, rawValue);
                }
                else if (propertyInfo.PropertyType == typeof(Single))
                {
                    TryConvert(() =>
                    {
                        Single singleValue = Convert.ToSingle(rawValue);
                        propertyInfo.SetValue(obj, singleValue);

                    }, typeOfT, propertyInfo.Name, rawValue);
                }
                else if (propertyInfo.PropertyType == typeof(Double))
                {
                    TryConvert(() =>
                    {
                        double doubleValue = Convert.ToDouble(rawValue);
                        propertyInfo.SetValue(obj, doubleValue);

                    }, typeOfT, propertyInfo.Name, rawValue);
                }
                else if (propertyInfo.PropertyType == typeof(String))
                {
                    if (dataStoreAttribute.IsEncrypted)
                    {
                        propertyInfo.SetValue(obj, cryptoProvider.Decrypt(rawValue.ToString()));
                    }
                    else
                    {
                        propertyInfo.SetValue(obj, rawValue.ToString());
                    }
                }
                else if (propertyInfo.PropertyType == typeof(byte[]))
                {
                    propertyInfo.SetValue(obj, rawValue);
                }
                else
                {
                    Console.WriteLine("Not Implemented {0}", propertyInfo.PropertyType);
                    throw new NotImplementedException();
                }
            }
        }

        public static DbDataReader ExecuteReaderWithRetry(this SQLiteCommand command)
        {
            DbDataReader dbDataReader = null;
            int retryCount = 0;

            while (true)
            {
                try
                {
                    dbDataReader = command.ExecuteReader();
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

            return dbDataReader;
        }

        public static async Task<DbDataReader> ExecuteReaderWithRetryAsync(this SQLiteCommand command)
        {
            DbDataReader dbDataReader = null;
            int retryCount = 0;

            while (true)
            {
                try
                {
                    dbDataReader = await command.ExecuteReaderAsync();
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

            return dbDataReader;
        }

        private static void TryConvert(Action action, Type ploudObjectType, string propertyName, object rawValue)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}\n{ploudObjectType.Name}.{propertyName} = {rawValue}", ex);
            }
        }
    }
}
