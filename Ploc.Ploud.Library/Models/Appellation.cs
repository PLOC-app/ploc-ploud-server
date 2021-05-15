using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
