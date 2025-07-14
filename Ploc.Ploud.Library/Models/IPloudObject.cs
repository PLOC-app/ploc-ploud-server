using System;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public interface IPloudObject
    {
        ICellar Cellar { get; set; }

        string Identifier { get; set; }

        string DeviceIdentifier { get; set; }

        string Name { get; set; }

        DateTime TimeCreated { get; set; }

        DateTime TimeLastModified { get; set; }

        long Timestamp { get; set; }

        bool Save();

        Task<bool> SaveAsync();

        bool Delete();

        Task<bool> DeleteAsync();
    }
}
