using System.Text.Json.Serialization;

namespace Ploc.Ploud.Library
{
    [DataStore("bottleformat")]
    public class BottleFormat : ListItem
    {
        [JsonPropertyName("vo")]
        [DataStore("volume")]
        public double Volume { get; set; }
    }
}
