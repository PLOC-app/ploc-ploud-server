using System;

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
