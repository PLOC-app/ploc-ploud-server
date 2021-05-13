using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public class PloudSettings
    {
        public String PublicKey { get; set; }

        public String PrivateKey { get; set; }

        public String HmacKey { get; set; }

        public String Directory { get; set; }

        public String VirtualDirectory { get; set; }
    }
}
