using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    [DataStore("io")]
    public class Order : PloudObject
    {
        [JsonPropertyName("vn")]
        [DataStore("vendor")]
        public String Vendor { get; set; }

        public Vendor GetVendor()
        {
            throw new NotImplementedException();
        }

        [JsonPropertyName("cm")]
        [DataStore("comments", true)]
        public String Comments { get; set; }

        [JsonPropertyName("in")]
        [DataStore("in")]
        public int Purchases { get; set; }

        [JsonPropertyName("ot")]
        [DataStore("out")]
        public int Consumptions { get; set; }

        [JsonPropertyName("up")]
        [DataStore("unitprice")]
        public float UnitPrice { get; set; }

        [JsonPropertyName("wi")]
        [DataStore("wine")]
        public String Wine { get; set; }

        public Wine GetWine()
        {
            throw new NotImplementedException();
        }

        [JsonPropertyName("dt")]
        [DataStore("date")]
        public DateTime When { get; set; }

        [JsonPropertyName("cy")]
        [DataStore("currency")]
        public String Currency { get; set; }

        [JsonPropertyName("oc")]
        [DataStore("occasion")]
        public OccasionType Occasion { get; set; }

        [JsonPropertyName("ap")]
        [DataStore("app")]
        public int AppChoppe { get; set; }

        [JsonPropertyName("sk")]
        [DataStore("appsku")]
        public String Sku { get; set; }

        [JsonPropertyName("wn")]
        [DataStore("apppid")]
        public String WineNumber { get; set; }

        [JsonPropertyName("pu")]
        [DataStore("producturl", true)]
        public String ProductUrl { get; set; }
    }
}
