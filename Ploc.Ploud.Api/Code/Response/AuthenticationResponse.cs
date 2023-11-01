using System;

namespace Ploc.Ploud.Api
{
    public class AuthenticationResponse
    {
        public bool IsAuthenticated { get; set; }

        public String ErrorMessage { get; set; }

        public String HashedMd5Email { get; set; } // If you want to add a security layer.

        public String FolderName { get; set; }

        public String FileName { get; set; }
    }
}
