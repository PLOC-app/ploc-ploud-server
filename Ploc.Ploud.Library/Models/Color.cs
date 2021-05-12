using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    [DataStore("color")]
    public class Color : ListItem
    {
        [DataStore("r")]
        public int Red { get; set; }

        [DataStore("g")]
        public int Green { get; set; }

        [DataStore("b")]
        public int Blue { get; set; }

        [DataStore("oc")]
        public int OcColor { get; set; }
    }
}
