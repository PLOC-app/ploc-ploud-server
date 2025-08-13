using System;
using System.Diagnostics;

namespace Ploc.Ploud.Library
{
    public static class Logger
    {
        public static void Error(Exception ex)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            string errorMessage = $"[{timestamp}] {ex}";

            Trace.TraceError(errorMessage);
            Console.WriteLine(errorMessage);
        }
    }
}