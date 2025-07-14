using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    [DataStore("mbw")]
    public sealed class PloudSecret : IPloudObject
    {
        public const string GlobalIdentifier = "__global__";

        [DataStore("id", false, true)]
        public string Identifier { get; set; }

        [DataStore("cwc", false)]
        public string Key { get; set; }

        [DataStore("kzf", false)]
        public string Iv { get; set; }

        [DataStore("version", false)]
        public string Version { get; set; }

        [JsonIgnore]
        public ICellar Cellar { get; set; }

        [JsonIgnore]
        public string Name
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

        [JsonIgnore]
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

        [JsonIgnore]
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

        [JsonIgnore]
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

        [JsonIgnore]
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

        public PloudSecret()
        {
            this.Identifier = GlobalIdentifier;
        }

        public bool Save()
        {
            return this.Cellar.Save(this);
        }

        public Task<bool> SaveAsync()
        {
            return this.Cellar.SaveAsync(this);
        }

        public bool Delete()
        {
            throw new InvalidOperationException();
        }

        public Task<bool> DeleteAsync()
        {
            throw new InvalidOperationException();
        }
    }
}
