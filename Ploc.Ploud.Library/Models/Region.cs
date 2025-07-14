using System;
using System.Text.Json.Serialization;

namespace Ploc.Ploud.Library
{
    [DataStore("region")]
    public class Region : ListItem
    {
        [JsonPropertyName("pa")]
        [DataStore("parent")]
        public string Country { get; set; }

        public Country GetCountry()
        {
            throw new NotImplementedException();
        }
    }
}
