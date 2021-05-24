using Microsoft.AspNetCore.Http;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ploc.Ploud.Api
{
    public class RsaKeyRequest
    {
        public String Data { get; set; }

        public String PrivateKey { get; set; }
    }
}
