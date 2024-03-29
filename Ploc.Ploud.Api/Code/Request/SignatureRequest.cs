﻿using System;

namespace Ploc.Ploud.Api
{
    public class SignatureRequest : RequestBase
    {
        public static class Methods
        {
            public const String Initialize = "initialize";

            public const String Uninitialize = "uninitialize";

            public const String EraseData = "erasedata";

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
