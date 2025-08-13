using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Ploc.Ploud.Library
{
    public sealed class LinuxCryptoServiceProvider : ICryptoServiceProvider
    {
        private static readonly RSAEncryptionPadding RSAEncryptionPadding = RSAEncryptionPadding.OaepSHA256;
        private RSA rsa;

        public LinuxCryptoServiceProvider(int keySize)
        {
            this.Initialize(keySize);
        }

        private void Initialize(int keySize)
        {
            string rsaFilePath = GetRsaFilePath();
            
            if (File.Exists(rsaFilePath))
            {
                string xmlData = File.ReadAllText(rsaFilePath, Encoding.UTF8);
                this.rsa = RSA.Create(keySize);
                this.Import(xmlData);
            }
            else
            {
                this.rsa = RSA.Create(keySize);
                this.SaveToFile();
            }
        }

        private void SaveToFile()
        {
            string rsaFilePath = GetRsaFilePath();
            string xmlData = this.Export();
            
            File.WriteAllText(rsaFilePath, xmlData);
        }

        public string Encrypt(byte[] data)
        {
            byte[] cypherText = this.rsa.Encrypt(data, RSAEncryptionPadding);
            string encryptedValue = Convert.ToBase64String(cypherText);
            
            return encryptedValue;
        }

        public byte[] Decrypt(string value)
        {
            byte[] data = Convert.FromBase64String(value);
            
            return this.rsa.Decrypt(data, RSAEncryptionPadding);
        }

        public string Export()
        {
            return this.rsa.ToXmlString(true);
        }

        public void Import(string data)
        {
            this.rsa.FromXmlString(data);

            this.SaveToFile();
        }

        private string GetRsaFilePath()
        {
            string assemblyLocation = typeof(ICryptoProvider).Assembly.Location;
            string appDirectory = Path.GetDirectoryName(assemblyLocation);
            string configDirectory = Path.Combine(appDirectory, "config");
            
            if (!Directory.Exists(configDirectory))
            {
                Directory.CreateDirectory(configDirectory);
            }
            
            return Path.Combine(configDirectory, "ploud.bin");
        }

        public void Dispose()
        {
            if (this.rsa != null)
            {
                this.rsa.Dispose();
                this.rsa = null;
            }
        }
    }
}
