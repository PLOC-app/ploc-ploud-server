using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    [DataStore("rackitem")]
    public class RackItem : PloudObject
    {
        [DataStore("rack")]
        public String Rack { get; set; }

        public Rack GetRack()
        {
            throw new NotImplementedException();
        }

        [DataStore("wine")]
        public String Wine { get; set; }

        public Wine GetWine()
        {
            throw new NotImplementedException();
        }

        [DataStore("row")]
        public int Row { get; set; }

        [DataStore("column")]
        public int Column { get; set; }

        [DataStore("display")]
        public RackItemDisplayType Display { get; set; }

        [DataStore("type")]
        public RackItemType Type { get; set; }

        [DataStore("alignment")]
        public RackItemContentAlignment ContentAlignment { get; set; }

        [DataStore("tags")]
        public String Tags { get; set; }

        [DataStore("qrcode")]
        public String QrCode { get; set; }
    }
}
