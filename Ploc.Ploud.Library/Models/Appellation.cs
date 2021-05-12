using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    [DataStore("appellation")]
    public class Appellation : ListItem
    {
        [DataStore("parent")]
        public String Region { get; set; }

        public Region GetRegion()
        {
            throw new NotImplementedException();
        }
    }
}
