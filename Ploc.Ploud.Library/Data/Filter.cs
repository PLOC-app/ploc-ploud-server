using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public sealed class Filter
    {
        public String Column { get; set; }

        public FilterType Type { get; set; }

        public Object Value { get; set; }
    }
}
