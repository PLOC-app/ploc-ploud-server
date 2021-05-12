using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    [DataStore("region")]
    public class Region : ListItem
    {
        [DataStore("parent")]
        public String Country { get; set; }

        public Country GetCountry()
        {
            throw new NotImplementedException();
        }
    }
}
