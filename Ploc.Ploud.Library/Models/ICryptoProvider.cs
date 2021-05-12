using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    public interface ICryptoProvider
    {
        String Encrypt(String value);

        String Decrypt(String value);
    }
}
