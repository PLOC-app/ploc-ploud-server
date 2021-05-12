using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public interface ICellarRepository
    {
        bool Delete<T>(T ploudObject) where T : IPloudObject;

        bool Delete<T>(IEnumerable<T> ploudObjects) where T : IPloudObject;

        bool Save<T>(T ploudObject) where T : IPloudObject;

        bool Save<T>(IEnumerable<T> ploudObjects) where T : IPloudObject;

        T Get<T>(String identifier) where T : IPloudObject;

        T Get<T>(IQuery query) where T : IPloudObject;

        IEnumerable<T> GetAll<T>(IQuery query) where T : IPloudObject;

        IEnumerable<T> GetAll<T>() where T : IPloudObject;
    }
}
