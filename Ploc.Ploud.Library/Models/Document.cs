using System.Text.Json.Serialization;

namespace Ploc.Ploud.Library
{
    [DataStore("document")]
    public class Document : PloudObject
    {
        [JsonPropertyName("kw")]
        [DataStore("keywords")]
        public string Keywords { get; set; }

        [JsonPropertyName("pa")]
        [DataStore("parent")]
        public string Parent { get; set; }

        [JsonPropertyName("pt")]
        [DataStore("parenttype")]
        public int ParentType { get; set; }

        [JsonPropertyName("tg")]
        [DataStore("tags")]
        public string Tags { get; set; }

        [JsonPropertyName("ct")]
        [DataStore("contenttype")]
        public string ContentType { get; set; }

        [JsonPropertyName("op")]
        [DataStore("originalpath")]
        public string OriginalPath { get; set; }

        [JsonPropertyName("lt")]
        [DataStore("length")]
        public long Length { get; set; }

        [JsonPropertyName("wd")]
        [DataStore("width")]
        public int Width { get; set; }

        [JsonPropertyName("ht")]
        [DataStore("height")]
        public int Height { get; set; }

        [JsonPropertyName("or")]
        [DataStore("ordr")]
        public int Order { get; set; }

        [JsonPropertyName("ty")]
        [DataStore("type")]
        public int Type { get; set; }

        [JsonIgnore]
        [DataStore("content")]
        public byte[] Data { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }
}
