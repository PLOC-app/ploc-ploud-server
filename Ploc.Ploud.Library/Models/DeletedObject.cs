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
        public string Identifier { get; set; }

        [JsonPropertyName("sid")]
        [DataStore("sid")]
        public string DeviceIdentifier { get; set; }

        [JsonPropertyName("tp")]
        [DataStore("tp")]
        public long Timestamp { get; set; }

        [JsonPropertyName("ty")]
        [DataStore("type", false, true)]
        public PloudObjectType Type { get; set; }

        [JsonIgnore]
        public ICellar Cellar { get; set; }

        [JsonIgnore]
        public IPloudObject PloudObject
        {
            get
            {
                MappingToAttribute mappingToAttribute = this.Type.GetAttribute<MappingToAttribute>();
                
                if (mappingToAttribute == null)
                {
                    throw new NotSupportedException(this.Type.ToString());
                }

                Type ploudObjectType = mappingToAttribute.Type;

                return this.Cellar.Get(this.Identifier, ploudObjectType);
            }
        }

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
                this.Timestamp = value.GetMillisecondsSince1970();
            }
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
            throw new NotSupportedException();
        }

        public Task<bool> DeleteAsync()
        {
            throw new InvalidOperationException();
        }
    }
}
