using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ploc.Ploud.Library
{
    public class RsaCryptoProvider : ICryptoProvider, IDisposable
    {
        private const String KeyContainerName = "PLOUD";
        private const int KeySize = 1024;

        private RSACryptoServiceProvider rsaCryptoServiceProvider;

        public RsaCryptoProvider()
        {
            this.rsaCryptoServiceProvider = CreateProvider();
        }

        String ICryptoProvider.Decrypt(String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }
            byte[] data = Convert.FromBase64String(value);
            byte[] textData = rsaCryptoServiceProvider.Decrypt(data, false);
            String decryptedValue = Encoding.Unicode.GetString(textData);
            return decryptedValue;
        }

        String ICryptoProvider.Encrypt(String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }
            byte[] data = Encoding.Unicode.GetBytes(value);
            byte[] cypherText = rsaCryptoServiceProvider.Encrypt(data, false);
            String encryptedValue = Convert.ToBase64String(cypherText);
            return encryptedValue;
        }

        public void ExportTo(String filePath)
        {
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            String xml = rsaCryptoServiceProvider.ToXmlString(true);
            File.WriteAllText(filePath, xml);
        }

        private RSACryptoServiceProvider CreateProvider()
        {
            if (OperatingSystem.IsWindows())
            {
                CspParameters cspParameters = new CspParameters()
                {
                    KeyContainerName = KeyContainerName
                };
                return new RSACryptoServiceProvider(KeySize, cspParameters);
            }
            throw new NotSupportedException();
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
