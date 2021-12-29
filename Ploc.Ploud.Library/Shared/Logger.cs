using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public static class Logger
    {
        public static void Error(Exception ex)
        {
            Trace.TraceError(ex.ToString());
            Console.WriteLine(ex);
        }
    }
}
