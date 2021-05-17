﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public static class CellarExtensions
    {
        public static bool Delete<T>(this ICellar cellar, T ploudObject) where T : IPloudObject
        {
            return cellar.Repository.Delete(ploudObject);
        }

        public static Task<bool> DeleteAsync<T>(this ICellar cellar, T ploudObject) where T : IPloudObject
        {
            return cellar.Repository.DeleteAsync(ploudObject);
        }

        public static bool Delete<T>(this ICellar cellar, IEnumerable<T> ploudObjects) where T : IPloudObject
        {
            return cellar.Repository.Delete(ploudObjects);
        }

        public static Task<bool> DeleteAsync<T>(this ICellar cellar, IEnumerable<T> ploudObjects) where T : IPloudObject
        {
            return cellar.Repository.DeleteAsync(ploudObjects);
        }

        public static bool Save<T>(this ICellar cellar, T ploudObject) where T : IPloudObject
        {
            return cellar.Repository.Save(ploudObject);
        }

        public static Task<bool> SaveAsync<T>(this ICellar cellar, T ploudObject) where T : IPloudObject
        {
            return cellar.Repository.SaveAsync(ploudObject);
        }

        public static bool Save<T>(this ICellar cellar, IEnumerable<T> ploudObjects) where T : IPloudObject
        {
            return cellar.Repository.Save(ploudObjects);
        }

        public static Task<bool> SaveAsync<T>(this ICellar cellar, IEnumerable<T> ploudObjects) where T : IPloudObject
        {
            return cellar.Repository.SaveAsync(ploudObjects);
        }

        public static T Get<T>(this ICellar cellar, String identifier) where T : IPloudObject
        {
            return cellar.Repository.Get<T>(identifier);
        }

        public static Task<T> GetAsync<T>(this ICellar cellar, String identifier) where T : IPloudObject
        {
            return cellar.Repository.GetAsync<T>(identifier);
        }

        public static T Get<T>(this ICellar cellar, IQuery query) where T : IPloudObject
        {
            return cellar.Repository.Get<T>(query);
        }

        public static Task<T> GetAsync<T>(this ICellar cellar, IQuery query) where T : IPloudObject
        {
            return cellar.Repository.GetAsync<T>(query);
        }

        public static PloudObjectCollection<T> GetAll<T>(this ICellar cellar, IQuery query) where T : IPloudObject
        {
            return cellar.Repository.GetAll<T>(query);
        }

        public static Task<PloudObjectCollection<T>> GetAllAsync<T>(this ICellar cellar, IQuery query) where T : IPloudObject
        {
            return cellar.Repository.GetAllAsync<T>(query);
        }

        public static PloudObjectCollection<T> GetAll<T>(this ICellar cellar) where T : IPloudObject
        {
            return cellar.Repository.GetAll<T>();
        }

        public static Task<PloudObjectCollection<T>> GetAllAsync<T>(this ICellar cellar) where T : IPloudObject
        {
            return cellar.Repository.GetAllAsync<T>();
        }

        public static bool Execute(this ICellar cellar, CellarOperation cellarOperation)
        {
            return cellar.Execute(cellarOperation);
        }
    }
}
