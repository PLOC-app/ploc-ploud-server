using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public class SignatureRequest : RequestBase
    {
        public static class Methods
        {
            public const String Initialize = "initialize";

            public const String Uninitialize = "uninitialize";

            public const String Download = "download";

            public const String Status = "status";

            public const String Sync = "sync";

            public const String GetDocument = "getdocument";

            public const String GetDashboard = "getdashboard";
        }

        public String Method { get; set; }

        public String ObjectId { get; set; }
    }
}
