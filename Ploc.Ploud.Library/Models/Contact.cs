using System.Text.Json.Serialization;

namespace Ploc.Ploud.Library
{
    public abstract class Contact : PloudObject
    {
        [JsonPropertyName("pid")]
        [DataStore("pid")]
        public string PlocIdentifier { get; set; }

        [JsonPropertyName("cm")]
        [DataStore("comments", true)]
        public string Comments { get; set; }

        [JsonPropertyName("ct")]
        [DataStore("contact", true)]
        public string ContactName { get; set; }

        [JsonPropertyName("ph")]
        [DataStore("phone")]
        public string Phone { get; set; }

        [JsonPropertyName("fa")]
        [DataStore("fax")]
        public string Fax { get; set; }

        [JsonPropertyName("mo")]
        [DataStore("mobile")]
        public string Mobile { get; set; }

        [JsonPropertyName("pc")]
        [DataStore("postalcode")]
        public string PostalCode { get; set; }

        [JsonPropertyName("ci")]
        [DataStore("city", true)]
        public string City { get; set; }

        [JsonPropertyName("co")]
        [DataStore("country", true)]
        public string Country { get; set; }

        [JsonPropertyName("a1")]
        [DataStore("address1")]
        public string Address1 { get; set; }

        [JsonPropertyName("a2")]
        [DataStore("address2")]
        public string Address2 { get; set; }

        [JsonPropertyName("ws")]
        [DataStore("website")]
        public string Website { get; set; }

        [JsonPropertyName("em")]
        [DataStore("email", true)]
        public string Email { get; set; }

        [JsonPropertyName("lt")]
        [DataStore("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("lg")]
        [DataStore("longitude")]
        public double Longitude { get; set; }
    }
}
