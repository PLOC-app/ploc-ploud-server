using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public static class Config
    {
        public const String Success = "success";

        public const String Error = "error";

        public const String ApiUrl = "https://api.ploc.co/v1/";

        public const String CurrentVersion = "v1";

        public static class Folders
        {
            public const String Data = "data";
        }

        public static class Actions
        {
            public const String Grant = "Grant";

            public const String Revoke = "Revoke";
        }
    }
}
