using System;
using System.Diagnostics;

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
