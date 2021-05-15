using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    [DataStore("rack")]
    public class Rack : PloudObject
    {
        [DataStore("model")]
        public String Model { get; set; }

        [DataStore("comments", true)]
        public String Comments { get; set; }

        [DataStore("rows")]
        public int Rows { get; set; }

        [DataStore("columns")]
        public int Columns { get; set; }

        [DataStore("sizemode")]
        public RackSizeMode SizeMode { get; set; }

        [DataStore("naming")]
        public RackNamingType Naming { get; set; }

        [DataStore("customnamingx")]
        public int CustomNamingX { get; set; }

        [DataStore("customnamingy")]
        public int CustomNamingY { get; set; }

        [DataStore("legend")]
        public RackLegendType Legend { get; set; }

        public RackItemCollection GetItems()
        {
            throw new NotImplementedException();
        }

        [DataStore("intervals")]
        public String Intervals { get; set; }
        
        public RackIntervalCollection GetRackIntervals()
        {
            throw new NotImplementedException();
        }
    }
}
