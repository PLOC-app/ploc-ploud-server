using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Text.Json;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class AesCryptoProviderTests
    {
        [TestMethod]
        public void TestEncryptDescryptAreEqualUsingSameInstance()
        {
            
            ICryptoProvider aesCryptoProvider = new AesCryptoProvider();

            Rack rack = new Rack();
            rack.Name = "hello";
            rack.Legend = RackLegendType.LetterOnXNumericOnY;

            JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
            {
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault
            };
            
            String jsonContent = JsonSerializer.Serialize(rack, serializerOptions);
            Console.WriteLine(jsonContent);

            string textToEncrypt = "hello WORLD";
            string encryptedText = aesCryptoProvider.Encrypt(textToEncrypt);
            Console.WriteLine(encryptedText);
            string descryptedText = aesCryptoProvider.Decrypt(encryptedText);
            Console.WriteLine(descryptedText);
            Assert.AreEqual(textToEncrypt, descryptedText);
        }

        [TestMethod]
        public void TestEncryptDescryptAreEqualUsingDistinctInstances()
        {
            AesCryptoProvider aesCryptoProvider = new AesCryptoProvider();
            string textToEncrypt = "hello WORLD";
            string encryptedText = aesCryptoProvider.Encrypt(textToEncrypt);
            Console.WriteLine(encryptedText);

            Console.WriteLine(aesCryptoProvider.EncryptedKey);
            Console.WriteLine(aesCryptoProvider.EncryptedIv);

            AesCryptoProvider copyOfAesCryptoProvider = new AesCryptoProvider(aesCryptoProvider.EncryptedKey, aesCryptoProvider.EncryptedIv);
            string descryptedText = copyOfAesCryptoProvider.Decrypt(encryptedText);
            Console.WriteLine(descryptedText);
            Assert.AreEqual(textToEncrypt, descryptedText);
        }

        [TestMethod]
        public void TestExportRsaKey()
        {
            AesCryptoProvider aesCryptoProvider = new AesCryptoProvider();
            String rsaKey = aesCryptoProvider.ExportRsaKey();
            Console.WriteLine(rsaKey);

            AesCryptoProvider copyOfAesCryptoProvider = new AesCryptoProvider();
            String copyOfRsaKey2 = copyOfAesCryptoProvider.ExportRsaKey();

            Assert.AreEqual(rsaKey, copyOfRsaKey2);
        }

        [TestMethod]
        public void TestExportImportRsaKey()
        {
            AesCryptoProvider aesCryptoProvider = new AesCryptoProvider();
            String rsaKey = aesCryptoProvider.ExportRsaKey();
            Console.WriteLine(rsaKey);

            AesCryptoProvider copyOfAesCryptoProvider = new AesCryptoProvider();
            copyOfAesCryptoProvider.ImportRsaKey(rsaKey);
            String copyOfRsaKey2 = copyOfAesCryptoProvider.ExportRsaKey();

            Assert.AreEqual(rsaKey, copyOfRsaKey2);
        }
    }
}
