using System;
using System.IO;
using System.Security.Cryptography;

namespace Ploc.Ploud.Library
{
    public class AesCryptoProvider : ICryptoProvider, IDisposable
    {
        private const string KeyContainerName = "PLOUD";
        private const int KeySize = 2048;
        private byte[] aesKey;
        private byte[] aesIv;

        private ICryptoServiceProvider cryptoServiceProvider;

        public AesCryptoProvider(string encryptedKey, string encryptedIv)
        {
            this.cryptoServiceProvider = CreateProvider();
            this.aesKey = this.DecryptRsa(encryptedKey);
            this.aesIv = this.DecryptRsa(encryptedIv);
        }

        public AesCryptoProvider()
        {
            this.cryptoServiceProvider = CreateProvider();
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

        public string EncryptedKey { get; private set; }

        public string EncryptedIv { get; private set; }

        private byte[] DecryptRsa(string value)
        {
            return this.cryptoServiceProvider.Decrypt(value);
        }

        private string EncryptRsa(byte[] data)
        {
            return this.cryptoServiceProvider.Encrypt(data);
        }

        public string Decrypt(string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return String.Empty;
            }

            byte[] data = Convert.FromBase64String(value);
            string plainText = null;
            
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

        public string Encrypt(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
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

        public string ExportRsaKey()
        {
            return this.cryptoServiceProvider.Export();
        }

        public bool ImportRsaKey(string data)
        {
            if (string.IsNullOrEmpty(data))
            {
                return false;
            }

            bool success = false;
            
            try
            {
                this.Dispose();
                this.cryptoServiceProvider = CreateProvider(true);
                this.cryptoServiceProvider.Import(data);

                success = true;
            }
            catch
            {
            }

            return success;
        }

        private ICryptoServiceProvider CreateProvider(bool excludeFlags)
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
                
                return new WindowsCryptoServiceProvider(KeySize, cspParameters);
            }

            return new LinuxCryptoServiceProvider(KeySize);
        }

        private ICryptoServiceProvider CreateProvider()
        {
            return this.CreateProvider(false);
        }

        public void Dispose()
        {
            if (this.cryptoServiceProvider != null)
            {
                this.cryptoServiceProvider.Dispose();
                this.cryptoServiceProvider = null;
            }
        }
    }
}
