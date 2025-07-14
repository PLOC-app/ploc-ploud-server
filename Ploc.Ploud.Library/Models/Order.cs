using System;
using System.Text.Json.Serialization;

namespace Ploc.Ploud.Library
{
    [DataStore("io")]
    public class Order : PloudObject
    {
        [JsonPropertyName("ve")]
        [DataStore("vendor")]
        public string Vendor { get; set; }

        [JsonPropertyName("cm")]
        [DataStore("comments", true)]
        public string Comments { get; set; }

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
        public string Wine { get; set; }

        [JsonPropertyName("dt")]
        [DataStore("date")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime When { get; set; }

        [JsonPropertyName("cy")]
        [DataStore("currency")]
        public string Currency { get; set; }

        [JsonPropertyName("oc")]
        [DataStore("occasion")]
        public OccasionType Occasion { get; set; }

        [JsonPropertyName("ap")]
        [DataStore("app")]
        public int AppChoppe { get; set; }

        [JsonPropertyName("sk")]
        [DataStore("appsku")]
        public string Sku { get; set; }

        [JsonPropertyName("wn")]
        [DataStore("apppid")]
        public string WineNumber { get; set; }

        [JsonPropertyName("pu")]
        [DataStore("producturl", true)]
        public string ProductUrl { get; set; }

        public Wine GetWine()
        {
            throw new NotImplementedException();
        }

        public Vendor GetVendor()
        {
            throw new NotImplementedException();
        }
    }
}
