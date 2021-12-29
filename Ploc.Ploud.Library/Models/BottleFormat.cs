using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
