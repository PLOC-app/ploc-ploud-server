using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    [DataStore("io")]
    public class Order : PloudObject
    {
        [DataStore("vendor")]
        public String Vendor { get; set; }

        public Vendor GetVendor()
        {
            throw new NotImplementedException();
        }

        [DataStore("comments", true)]
        public String Comments { get; set; }

        [DataStore("in")]
        public int Purchases { get; set; }

        [DataStore("out")]
        public int Consumptions { get; set; }

        [DataStore("unitprice")]
        public float UnitPrice { get; set; }

        [DataStore("wine")]
        public String Wine { get; set; }

        public Wine GetWine()
        {
            throw new NotImplementedException();
        }

        [DataStore("date")]
        public DateTime When { get; set; }

        [DataStore("currency")]
        public String Currency { get; set; }

        [DataStore("occasion")]
        public OccasionType Occasion { get; set; }

        [DataStore("app")]
        public int AppChoppe { get; set; }

        [DataStore("appsku")]
        public String Sku { get; set; }

        [DataStore("apppid")]
        public String WineNumber { get; set; }

        [DataStore("appproducturl")]
        public String ProductUrl { get; set; }
    }
}
