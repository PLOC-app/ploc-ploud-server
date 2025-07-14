namespace Ploc.Ploud.Api
{
    public class SignatureRequest : RequestBase
    {
        public static class Methods
        {
            public const string Initialize = "initialize";

            public const string Uninitialize = "uninitialize";

            public const string EraseData = "erasedata";

            public const string Download = "download";

            public const string Status = "status";

            public const string Sync = "sync";

            public const string GetDocument = "getdocument";

            public const string GetDashboard = "getdashboard";
        }

        public string Method { get; set; }

        public string ObjectId { get; set; }
    }
}
