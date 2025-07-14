using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public interface ICellarRepository
    {
        bool CreateStorage<T>() where T : IPloudObject;

        bool Delete<T>(T ploudObject) where T : IPloudObject;

        Task<bool> DeleteAsync<T>(T ploudObject) where T : IPloudObject;

        bool Delete<T>(IEnumerable<T> ploudObjects) where T : IPloudObject;

        Task<bool> DeleteAsync<T>(IEnumerable<T> ploudObjects) where T : IPloudObject;

        bool Save<T>(T ploudObject) where T : IPloudObject;

        Task<bool> SaveAsync<T>(T ploudObject) where T : IPloudObject;

        bool Save<T>(IEnumerable<T> ploudObjects) where T : IPloudObject;

        Task<bool> SaveAsync<T>(IEnumerable<T> ploudObjects) where T : IPloudObject;

        IPloudObject Get(string identifier, Type ploudObjectType);

        T Get<T>(string identifier) where T : IPloudObject;

        T Get<T>(IQuery query) where T : IPloudObject;

        Task<T> GetAsync<T>(string identifier) where T : IPloudObject;

        Task<T> GetAsync<T>(IQuery query) where T : IPloudObject;

        PloudObjectCollection<T> GetAll<T>(IQuery query) where T : IPloudObject;

        PloudObjectCollection<T> GetAll<T>() where T : IPloudObject;

        Task<PloudObjectCollection<T>> GetAllAsync<T>(IQuery query) where T : IPloudObject;

        Task<PloudObjectCollection<T>> GetAllAsync<T>() where T : IPloudObject;

        bool Execute(CellarOperation cellarOperation);

        Task<bool> ExecuteAsync(CellarOperation cellarOperation);

        bool CopyTo(string targetCellarPath);

        Task<bool> CopyToAsync(string targetCellarPath);

        Dashboard GetDashboard();

        Task<Dashboard> GetDashboardAsync();
    }
}
