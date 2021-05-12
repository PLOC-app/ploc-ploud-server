using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public static class Config
    {
        public const String Version = "0.1";

        public const String SqliteLockFileExtension = ".plock";

        public static class Data
        {
            public const int MaxRetries = 25;

            public const int RetryDelay = 200;
        }
    }
}
