using System;

namespace Ploc.Ploud.Library
{
    public interface ICryptoProvider : IDisposable
    {
        String Encrypt(String value);

        String Decrypt(String value);

        String ExportRsaKey();

        bool ImportRsaKey(String data);
    }
}
