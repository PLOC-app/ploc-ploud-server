using System;
using System.Text.Json.Serialization;

namespace Ploc.Ploud.Library
{
    [DataStore("appellation")]
    public class Appellation : ListItem
    {
        [JsonPropertyName("pa")]
        [DataStore("parent")]
        public string Region { get; set; }

        public Region GetRegion()
        {
            throw new NotImplementedException();
        }
    }
}
