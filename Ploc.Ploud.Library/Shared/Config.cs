namespace Ploc.Ploud.Library
{
    public static class Config
    {
        public const string Version = "0.1";

        public const string SqliteLockFileExtension = ".plock";

        public static class Data
        {
            public const int MaxRetries = 25;

            public const int RetryDelay = 200;

            internal static readonly string[] TableNames = new string[] { "appellation", "bottleformat", "classification", "color", "country", "document", "globalparameter", "grape", "io", "owner", "rack", "rackitem", "region", "tastingnotes", "vendor", "wine" };
        }
    }
}
