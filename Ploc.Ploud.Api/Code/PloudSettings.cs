using System;

namespace Ploc.Ploud.Api
{
    public class PloudSettings
    {
        public PloudSettings() { }

        public Guid PublicKey { get; set; }

        public String PrivateKey { get; set; }

        public String HmacKey { get; set; }

        public String Directory { get; set; }

        public bool VerifySignature { get; set; }
    }
}
