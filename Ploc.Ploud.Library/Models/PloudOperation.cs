using System;

namespace Ploc.Ploud.Library
{
    public sealed class PloudOperation : IPloudObject
    {
        [DataStore("id", false, true)]
        public String Identifier { get; set; }

        [DataStore("sid")]
        public String DeviceIdentifier { get; set; }

        [DataStore("tp")]
        public long Timestamp { get; set; }

        [DataStore("type", false, true)]
        public int Type { get; set; }

        public ICellar Cellar { get; internal set; }

        public bool Save()
        {
            return this.Cellar.Save(this);
        }

        public bool Delete()
        {
            return this.Cellar.Delete(this);
        }

        public String Name
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public DateTime TimeCreated
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        public DateTime TimeLastModified
        {
            get
            {
                throw new NotSupportedException();
            }
            set
            {
                throw new NotSupportedException();
            }
        }
    }
}
