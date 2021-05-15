using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        public String Value { get; set; }
    }
}
