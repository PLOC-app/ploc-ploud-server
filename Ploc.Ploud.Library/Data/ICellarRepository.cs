using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        T Get<T>(String identifier) where T : IPloudObject;

        T Get<T>(IQuery query) where T : IPloudObject;

        Task<T> GetAsync<T>(String identifier) where T : IPloudObject;

        Task<T> GetAsync<T>(IQuery query) where T : IPloudObject;

        PloudObjectCollection<T> GetAll<T>(IQuery query) where T : IPloudObject;

        PloudObjectCollection<T> GetAll<T>() where T : IPloudObject;

        Task<PloudObjectCollection<T>> GetAllAsync<T>(IQuery query) where T : IPloudObject;

        Task<PloudObjectCollection<T>> GetAllAsync<T>() where T : IPloudObject;

        bool Execute(CellarOperation cellarOperation);
    }
}
