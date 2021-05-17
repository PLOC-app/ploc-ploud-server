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

            public const String Unnitialize = "uninitialize";

            public const String Status = "status";

            public const String Sync = "sync";
        }

        public String Method { get; set; }
    }
}
