using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public abstract class PloudObject : IPloudObject
    {
        [DataStore("id", false, true)]
        public String Identifier { get; set; }

        [DataStore("sid")]
        public String DeviceIdentifier { get; set; }

        [DataStore("name", true)]
        public String Name { get; set; }

        [DataStore("tc")]
        public DateTime TimeCreated { get; set; }

        [DataStore("tm")]
        public DateTime TimeLastModified { get; set; }

        [DataStore("tp")]
        public long Timestamp { get; set; }

        [JsonIgnore]
        public ICellar Cellar { get; set; }

        public virtual bool Save()
        {
            return this.Cellar.Save(this);
        }

        public virtual bool Delete()
        {
            return this.Cellar.Delete(this);
        }
    }
}
