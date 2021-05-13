using System;
using System.Collections.Generic;

namespace Ploc.Ploud.Library
{
    public static class CellarExtensions
    {
        public static bool Delete<T>(this ICellar cellar, T ploudObject) where T : IPloudObject
        {
            return cellar.Repository.Delete(ploudObject);
        }

        public static bool Delete<T>(this ICellar cellar, IEnumerable<T> ploudObjects) where T : IPloudObject
        {
            return cellar.Repository.Delete(ploudObjects);
        }

        public static bool Save<T>(this ICellar cellar, T ploudObject) where T : IPloudObject
        {
            return cellar.Repository.Save(ploudObject);
        }

        public static bool Save<T>(this ICellar cellar, IEnumerable<T> ploudObjects) where T : IPloudObject
        {
            return cellar.Repository.Save(ploudObjects);
        }

        public static T Get<T>(this ICellar cellar, String identifier) where T : IPloudObject
        {
            return cellar.Repository.Get<T>(identifier);
        }

        public static T Get<T>(this ICellar cellar, IQuery query) where T : IPloudObject
        {
            return cellar.Repository.Get<T>(query);
        }

        public static IList<T> GetAll<T>(this ICellar cellar, IQuery query) where T : IPloudObject
        {
            return cellar.Repository.GetAll<T>(query);
        }

        public static IList<T> GetAll<T>(this ICellar cellar) where T : IPloudObject
        {
            return cellar.Repository.GetAll<T>();
        }
    }
}
