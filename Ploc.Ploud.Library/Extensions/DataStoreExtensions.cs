using System;
using System.Collections.Generic;
using System.Reflection;

namespace Ploc.Ploud.Library
{
    public static class DataStoreExtensions
    {
        public static String GetTableName(this Type type)
        {
            String dataStoreName = type.Name.ToLower();
            DataStoreAttribute attribute = type.GetAttribute<DataStoreAttribute>();
            if (attribute != null)
            {
                dataStoreName = attribute.Name;
            }
            return dataStoreName;
        }

        public static IList<DataStoreAttribute> GetDataStoreAttributes(this Type type)
        {
            IList<DataStoreAttribute> dataStoreAttributes = new List<DataStoreAttribute>();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                DataStoreAttribute dataStoreAttribute = propertyInfo.GetAttribute<DataStoreAttribute>();
                if (dataStoreAttribute == null)
                {
                    continue;
                }
                dataStoreAttributes.Add(dataStoreAttribute);
            }
            return dataStoreAttributes;
        }
    }
}
