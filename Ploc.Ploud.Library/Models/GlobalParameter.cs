using System.Text.Json.Serialization;

namespace Ploc.Ploud.Library
{
    [DataStore("globalparameter")]
    public class GlobalParameter : PloudObject
    {
        [JsonPropertyName("ty")]
        [DataStore("type")]
        public int Type { get; set; }

        [JsonPropertyName("va")]
        [DataStore("value", true)]
        public string Value { get; set; }
    }
}
