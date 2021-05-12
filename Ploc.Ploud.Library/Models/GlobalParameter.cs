using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    [DataStore("globalparameter")]
    public class GlobalParameter : PloudObject
    {
        [DataStore("type")]
        public int Type { get; set; }

        [DataStore("value", true)]
        public String Value { get; set; }
    }
}
