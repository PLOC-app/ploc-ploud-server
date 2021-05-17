using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    [DataStore("ploud")]
    public sealed class DeletedObject : IPloudObject
    {
        [JsonPropertyName("id")]
        [DataStore("id", false, true)]
        public String Identifier { get; set; }

        [JsonPropertyName("sid")]
        [DataStore("sid")]
        public String DeviceIdentifier { get; set; }

        [JsonPropertyName("tp")]
        [DataStore("tp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("ty")]
        [DataStore("type", false, true)]
        public int Type { get; set; }

        [JsonIgnore]
        public ICellar Cellar { get; set; }

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
            throw new NotSupportedException();
        }

        public Task<bool> DeleteAsync()
        {
            throw new InvalidOperationException();
        }

        [JsonIgnore]
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
    }
}
