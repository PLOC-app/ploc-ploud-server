using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class AesCryptoProviderTests
    {
        [TestMethod]
        public void TestEncryptDescryptAreEqualUsingSameInstance()
        {
            ICryptoProvider aesCryptoProvider = new AesCryptoProvider();
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
    }
}
