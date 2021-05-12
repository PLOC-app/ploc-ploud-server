using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public static class AttributeExtensions
    {
        public static T GetAttribute<T>(this PropertyInfo propertyInfo) where T : Attribute
        {
            T ret = null;
            foreach (Attribute atrs in propertyInfo.GetCustomAttributes(true))
            {
                if (atrs.GetType() == typeof(T))
                {
                    ret = atrs as T;
                    break;
                }
            }
            return ret;
        }

        public static T GetAttribute<T>(this Type type) where T : Attribute
        {
            T ret = null;
            foreach (Attribute atrs in type.GetCustomAttributes(true))
            {
                if (atrs.GetType() == typeof(T))
                {
                    ret = atrs as T;
                    break;
                }
            }
            return ret;
        }
    }
}
