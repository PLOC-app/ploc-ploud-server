using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public interface IPloudObject
    {
        ICellar Cellar { get; set; }

        String Identifier { get; set; }

        String DeviceIdentifier { get; set; }

        String Name { get; set; }

        DateTime TimeCreated { get; set; }

        DateTime TimeLastModified { get; set; }

        long Timestamp { get; set; }

        bool Save();

        Task<bool> SaveAsync();

        bool Delete();

        Task<bool> DeleteAsync();
    }
}
