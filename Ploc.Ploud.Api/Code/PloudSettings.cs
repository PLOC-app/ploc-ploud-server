using System;

namespace Ploc.Ploud.Api
{
    public class PloudSettings
    {
        public Guid PublicKey { get; set; }

        public string PrivateKey { get; set; }

        public string HmacKey { get; set; }

        public string Directory { get; set; }

        public bool VerifySignature { get; set; }
    }
}
