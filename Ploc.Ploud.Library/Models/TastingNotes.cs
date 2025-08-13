using System;
using System.Text.Json.Serialization;

namespace Ploc.Ploud.Library
{
    [DataStore("tastingnotes")]
    public class TastingNotes : PloudObject
    {
        [JsonPropertyName("wi")]
        [DataStore("wine")]
        public string Wine { get; set; }

        [JsonPropertyName("no")]
        [DataStore("note")]
        public int Note { get; set; }

        [JsonPropertyName("vi")]
        [DataStore("vintage")]
        public int Vintage { get; set; }

        [JsonPropertyName("lt")]
        [DataStore("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("lg")]
        [DataStore("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("lid")]
        [DataStore("lid")]
        public string LocalTemplateIdentifier { get; set; }

        [JsonPropertyName("dt")]
        [JsonConverter(typeof(DateTimeConverter))]
        [DataStore("date")]
        public DateTime When { get; set; }

        [JsonPropertyName("pid")]
        [DataStore("pid")]
        public string PlocIdentifier { get; set; }

        [JsonPropertyName("wpid")]
        [DataStore("wpid")]
        public string WinePlocIdentifier { get; set; }

        [JsonPropertyName("so")]
        [DataStore("source")]
        public int Source { get; set; }

        [JsonPropertyName("sn")]
        [DataStore("sourcename")]
        public string SourceName { get; set; }

        [JsonPropertyName("spid")]
        [DataStore("spid")]
        public string SourcePlocIdentifier { get; set; }

        [JsonPropertyName("ln")]
        [DataStore("ln")]
        public string Language { get; set; }

        [JsonPropertyName("im")]
        [DataStore("image")]
        public string Image { get; set; }

        [JsonPropertyName("iw")]
        [DataStore("imagewidth")]
        public int ImageWidth { get; set; }

        [JsonPropertyName("ih")]
        [DataStore("imageheight")]
        public int ImageHeight { get; set; }

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
        [DataStore("occolor")]
        public int OcColor { get; set; }

        [JsonPropertyName("mo")]
        [DataStore("mood")]
        public MoodType Mood { get; set; }

        [JsonPropertyName("fl")]
        [DataStore("fields", true)]
        public string Fields { get; set; }

        public Wine GetWine()
        {
            throw new NotImplementedException();
        }
    }
}
