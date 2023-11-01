using System;
using System.Text.Json.Serialization;

namespace Ploc.Ploud.Library
{
    [DataStore("rack")]
    public class Rack : PloudObject
    {
        [DataStore("model")]
        [JsonPropertyName("mo")]
        public String Model { get; set; }

        [DataStore("comments", true)]
        [JsonPropertyName("cm")]
        public String Comments { get; set; }

        [DataStore("rows")]
        [JsonPropertyName("ro")]
        public int Rows { get; set; }

        [DataStore("columns")]
        [JsonPropertyName("cl")]
        public int Columns { get; set; }

        [DataStore("sizemode")]
        [JsonPropertyName("si")]
        public RackSizeMode SizeMode { get; set; }

        [DataStore("naming")]
        [JsonPropertyName("na")]
        public RackNamingType Naming { get; set; }

        [JsonPropertyName("cx")]
        [DataStore("customnamingx")]
        public int CustomNamingX { get; set; }

        [JsonPropertyName("cy")]
        [DataStore("customnamingy")]
        public int CustomNamingY { get; set; }

        [JsonPropertyName("le")]
        [DataStore("legend")]
        public RackLegendType Legend { get; set; }

        public RackItemCollection GetItems()
        {
            throw new NotImplementedException();
        }

        [JsonPropertyName("in")]
        [DataStore("intervals")]
        public String Intervals { get; set; }
        
        public RackIntervalCollection GetRackIntervals()
        {
            throw new NotImplementedException();
        }
    }
}
