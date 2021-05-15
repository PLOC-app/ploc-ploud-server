using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    [DataStore("document")]
    public class Document : PloudObject
    {
        [JsonPropertyName("kw")]
        [DataStore("keywords")]
        public String Keywords { get; set; }

        [JsonPropertyName("pa")]
        [DataStore("parent")]
        public String Parent { get; set; }

        [JsonPropertyName("pt")]
        [DataStore("parenttype")]
        public int ParentType { get; set; }

        [JsonPropertyName("tg")]
        [DataStore("tags")]
        public String Tags { get; set; }

        [JsonPropertyName("ct")]
        [DataStore("contenttype")]
        public String ContentType { get; set; }

        [JsonPropertyName("op")]
        [DataStore("originalpath")]
        public String OriginalPath { get; set; }

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
        public String Url { get; set; }
    }
}
