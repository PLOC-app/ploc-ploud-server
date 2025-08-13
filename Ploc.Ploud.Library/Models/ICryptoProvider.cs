using System;

namespace Ploc.Ploud.Library
{
    public interface ICryptoProvider : IDisposable
    {
        string Encrypt(string value);

        string Decrypt(string value);

        string ExportRsaKey();

        bool ImportRsaKey(string data);
    }
}
