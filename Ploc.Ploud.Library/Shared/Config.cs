using System;

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

            internal static readonly String[] TableNames = new String[] { "appellation", "bottleformat", "classification", "color", "country", "document", "globalparameter", "grape", "io", "owner", "rack", "rackitem", "region", "tastingnotes", "vendor", "wine" };
        }
    }
}
