using System;
using System.Text.Json.Serialization;

namespace Ploc.Ploud.Library
{
    [DataStore("rackitem")]
    public class RackItem : PloudObject
    {
        [JsonPropertyName("rk")]
        [DataStore("rack")]
        public string Rack { get; set; }

        [JsonPropertyName("wi")]
        [DataStore("wine")]
        public string Wine { get; set; }

        [JsonPropertyName("rw")]
        [DataStore("row")]
        public int Row { get; set; }

        [JsonPropertyName("cl")]
        [DataStore("column")]
        public int Column { get; set; }

        [JsonPropertyName("dp")]
        [DataStore("display")]
        public RackItemDisplayType Display { get; set; }

        [JsonPropertyName("ty")]
        [DataStore("type")]
        public RackItemType Type { get; set; }

        [JsonPropertyName("al")]
        [DataStore("alignment")]
        public RackItemContentAlignment ContentAlignment { get; set; }

        [JsonPropertyName("tg")]
        [DataStore("tags")]
        public string Tags { get; set; }

        [JsonPropertyName("qr")]
        [DataStore("qrcode")]
        public string QrCode { get; set; }

        public Rack GetRack()
        {
            throw new NotImplementedException();
        }

        public Wine GetWine()
        {
            throw new NotImplementedException();
        }
    }
}
