using System;

namespace Ploc.Ploud.Library
{
    [DataStore("mbw")]
    public sealed class PloudSecret : IPloudObject
    {
        public const String GlobalIdentifier = "__global__";

        public PloudSecret()
        {
            this.Identifier = GlobalIdentifier;
        }

        [DataStore("id", false, true)]
        public String Identifier { get; set; }

        [DataStore("cwc", false)]
        public String Key { get; set; }

        [DataStore("kzf", false)]
        public String Iv { get; set; }

        [DataStore("version", false)]
        public String Version { get; set; }

        public ICellar Cellar { get; set; }

        public bool Save()
        {
            return this.Cellar.Save(this);
        }

        public bool Delete()
        {
            throw new InvalidOperationException();
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

        public string DeviceIdentifier
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

        public long Timestamp
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
