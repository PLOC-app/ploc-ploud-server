using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ploc.Ploud.Library
{
    interface ICryptoServiceProvider : IDisposable
    {
        String Encrypt(byte[] data);

        byte[] Decrypt(String value);

        String Export();

        void Import(String data);
    }
}
