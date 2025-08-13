using System;

namespace Ploc.Ploud.Library
{
    interface ICryptoServiceProvider : IDisposable
    {
        string Encrypt(byte[] data);

        byte[] Decrypt(string value);

        string Export();

        void Import(string data);
    }
}
