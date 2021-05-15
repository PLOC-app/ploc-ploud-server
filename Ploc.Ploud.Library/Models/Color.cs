using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    [DataStore("color")]
    public class Color : ListItem
    {
        [JsonPropertyName("r")]
        [DataStore("r")]
        public int Red { get; set; }

        [JsonPropertyName("g")]
        [DataStore("g")]
        public int Green { get; set; }

        [JsonPropertyName("b")]
        [DataStore("b")]
        public int Blue { get; set; }

        [JsonPropertyName("oc")]
        [DataStore("oc")]
        public int OcColor { get; set; }
    }
}
