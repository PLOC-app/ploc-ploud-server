using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public static class ICryptoProviderFactory
    {
        public static ICryptoProvider CreateProvider()
        {
            return new AesCryptoProvider();
        }
    }
}
