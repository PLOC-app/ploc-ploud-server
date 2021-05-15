using System;
using System.Text.Json.Serialization;

namespace Ploc.Ploud.Library
{
    public abstract class Contact : PloudObject
    {
        [JsonPropertyName("pid")]
        [DataStore("pid")]
        public String PlocIdentifier { get; set; }

        [JsonPropertyName("cm")]
        [DataStore("comments", true)]
        public String Comments { get; set; }

        [JsonPropertyName("ct")]
        [DataStore("contact", true)]
        public String ContactName { get; set; }

        [JsonPropertyName("ph")]
        [DataStore("phone")]
        public String Phone { get; set; }

        [JsonPropertyName("fa")]
        [DataStore("fax")]
        public String Fax { get; set; }

        [JsonPropertyName("mo")]
        [DataStore("mobile")]
        public String Mobile { get; set; }

        [JsonPropertyName("pc")]
        [DataStore("postalcode")]
        public String PostalCode { get; set; }

        [JsonPropertyName("ci")]
        [DataStore("city", true)]
        public String City { get; set; }

        [JsonPropertyName("a1")]
        [DataStore("address1")]
        public String Address1 { get; set; }

        [JsonPropertyName("a2")]
        [DataStore("address2")]
        public String Address2 { get; set; }

        [JsonPropertyName("ws")]
        [DataStore("website")]
        public String Website { get; set; }

        [JsonPropertyName("em")]
        [DataStore("email", true)]
        public String Email { get; set; }

        [JsonPropertyName("lt")]
        [DataStore("latitude")]
        public String Latitude { get; set; }

        [JsonPropertyName("lg")]
        [DataStore("longitude")]
        public String Longitude { get; set; }
    }
}
