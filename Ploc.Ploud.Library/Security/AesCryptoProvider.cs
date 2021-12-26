using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ploc.Ploud.Library
{
    public class AesCryptoProvider : ICryptoProvider, IDisposable
    {
        private const String KeyContainerName = "PLOUD";
        private const int KeySize = 2048;
        private byte[] aesKey;
        private byte[] aesIv;

        private RSACryptoServiceProvider rsaCryptoServiceProvider;

        public AesCryptoProvider(String encryptedKey, String encryptedIv)
        {
            this.rsaCryptoServiceProvider = CreateProvider();
            this.aesKey = this.DecryptRsa(encryptedKey);
            this.aesIv = this.DecryptRsa(encryptedIv);
        }

        public AesCryptoProvider()
        {
            this.rsaCryptoServiceProvider = CreateProvider();
            this.InitializeAes();
        }

        private void InitializeAes()
        {
            using (Aes aes = Aes.Create())
            {
                aes.GenerateKey();
                aes.GenerateIV();
                this.aesKey = aes.Key;
                this.aesIv = aes.IV;

                this.EncryptedKey = this.EncryptRsa(this.aesKey);
                this.EncryptedIv = this.EncryptRsa(this.aesIv);
            }
        }

        public String EncryptedKey { get; private set; }

        public String EncryptedIv { get; private set; }

        private byte[] DecryptRsa(String value)
        {
            byte[] data = Convert.FromBase64String(value);
            return rsaCryptoServiceProvider.Decrypt(data, false);
        }

        private String EncryptRsa(byte[] data)
        {
            byte[] cypherText = rsaCryptoServiceProvider.Encrypt(data, false);
            String encryptedValue = Convert.ToBase64String(cypherText);
            return encryptedValue;
        }

        public String Decrypt(String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }
            byte[] data = Convert.FromBase64String(value);
            String plainText = null;
            using (Aes aes = Aes.Create())
            {
                aes.Key = this.aesKey;
                aes.IV = this.aesIv;
                using (ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                {
                    using (MemoryStream memoryStream = new MemoryStream(data))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader(cryptoStream))
                            {
                                plainText = streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            return plainText;
        }

        public String Encrypt(String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }
            byte[] cypherText;
            using (Aes aes = Aes.Create())
            {
                aes.Key = this.aesKey;
                aes.IV = this.aesIv;
                using (ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                            {
                                streamWriter.Write(value);
                            }
                            cypherText = memoryStream.ToArray();
                        }
                    }
                }
            }
            return Convert.ToBase64String(cypherText);
        }

        public String ExportRsaKey()
        {
            return rsaCryptoServiceProvider.ToXmlString(true);
        }

        public bool ImportRsaKey(String data)
        {
            if (String.IsNullOrEmpty(data))
            {
                return false;
            }
            bool success = false;
            try
            {
                this.Dispose();
                this.rsaCryptoServiceProvider = CreateProvider(true);
                this.rsaCryptoServiceProvider.FromXmlString(data);
                success = true;
            }
            catch
            {
               
            }
            return success;
        }

        private RSACryptoServiceProvider CreateProvider(bool excludeFlags)
        {
            if (OperatingSystem.IsWindows())
            {
                CspParameters cspParameters = new CspParameters()
                {
                    KeyContainerName = KeyContainerName
                };
                cspParameters.Flags |= CspProviderFlags.UseMachineKeyStore;
                if (!excludeFlags) 
                {
                    cspParameters.Flags |= CspProviderFlags.UseArchivableKey | CspProviderFlags.NoPrompt;
                }
                return new RSACryptoServiceProvider(KeySize, cspParameters);
            }
            throw new NotSupportedException();
        }

        private RSACryptoServiceProvider CreateProvider()
        {
            return CreateProvider(false);
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
