using System;
using System.Text.Json.Serialization;

namespace Ploc.Ploud.Library
{
    [DataStore("appellation")]
    public class Appellation : ListItem
    {
        [JsonPropertyName("pa")]
        [DataStore("parent")]
        public String Region { get; set; }

        public Region GetRegion()
        {
            throw new NotImplementedException();
        }
    }
}
