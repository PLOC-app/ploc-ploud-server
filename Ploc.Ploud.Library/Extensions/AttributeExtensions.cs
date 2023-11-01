using System;
using System.Reflection;

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

        public static T GetAttribute<T>(this Enum enumValue) where T : Attribute
        {
            Type type = enumValue.GetType();
            MemberInfo[] memberInfos = type.GetMember(enumValue.ToString());
            T ret = null;
            foreach (Attribute atrs in memberInfos[0].GetCustomAttributes(typeof(T), false))
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
