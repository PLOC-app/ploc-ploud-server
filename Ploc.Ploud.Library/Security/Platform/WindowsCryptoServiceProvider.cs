using System;
using System.Security.Cryptography;

namespace Ploc.Ploud.Library
{
    public sealed class WindowsCryptoServiceProvider : ICryptoServiceProvider
    {
        private RSACryptoServiceProvider rsaCryptoServiceProvider;

        public WindowsCryptoServiceProvider(int keySize, CspParameters cspParameters)
        {
            if (!OperatingSystem.IsWindows())
            {
                throw new PlatformNotSupportedException();
            }

            this.rsaCryptoServiceProvider = new RSACryptoServiceProvider(keySize, cspParameters);
        }

        public string Encrypt(byte[] data)
        {
            byte[] cypherText = rsaCryptoServiceProvider.Encrypt(data, false);
            string encryptedValue = Convert.ToBase64String(cypherText);

            return encryptedValue;
        }

        public byte[] Decrypt(string value)
        {
            byte[] data = Convert.FromBase64String(value);

            return this.rsaCryptoServiceProvider.Decrypt(data, false);
        }

        public string Export()
        {
            return this.rsaCryptoServiceProvider.ToXmlString(true);
        }

        public void Import(string data)
        {
            this.rsaCryptoServiceProvider.FromXmlString(data);
        }

        public void Dispose()
        {
            if (this.rsaCryptoServiceProvider != null)
            {
                this.rsaCryptoServiceProvider.Dispose();
                this.rsaCryptoServiceProvider = null;
            }
        }
    }
}
