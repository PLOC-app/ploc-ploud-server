using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public class PloudObjectCollection<T> : List<T> where T : IPloudObject
    {
    }
}
