using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class CellarTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Shared.CopyDatabase(GetType().Name);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Shared.DeleteDatabase(GetType().Name);
        }

        [TestMethod]
        public void IsValidShouldReturnTrue()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Assert.IsTrue(cellar.IsValid());
        }

        [TestMethod]
        public void TestCopy()
        {
            String randomFileName = String.Concat(Guid.NewGuid(), ".config");
            String cellarFilePath = Path.Combine(Path.GetTempPath(), randomFileName);

            ICellar sourceCellar = Shared.Cellar(GetType().Name);
            sourceCellar.CopyTo(cellarFilePath);

            ICellar targetCellar = new Cellar(cellarFilePath);
            bool isValid = targetCellar.IsValid();
            Assert.IsTrue(isValid);
            File.Delete(cellarFilePath);
        }

        [TestMethod]
        public void TestCopyAndDecrypt()
        {
            String randomFileName = String.Concat(Guid.NewGuid(), ".config");
            String cellarFilePath = Path.Combine(Path.GetTempPath(), randomFileName);

            ICellar sourceCellar = Shared.Cellar(GetType().Name);
            sourceCellar.CopyTo(cellarFilePath);

            ICellar targetCellar = new Cellar(cellarFilePath);
            targetCellar.Execute(CellarOperation.Decrypt);

            bool isValid = targetCellar.IsValid();
            Assert.IsTrue(isValid);
            File.Delete(cellarFilePath);
        }

        [TestMethod]
        public void TestCopyAndCompress()
        {
            String randomFileName = String.Concat(Guid.NewGuid(), ".config");
            String cellarFilePath = Path.Combine(Path.GetTempPath(), randomFileName);

            ICellar sourceCellar = Shared.Cellar(GetType().Name);
            sourceCellar.CopyTo(cellarFilePath);

            ICellar targetCellar = new Cellar(cellarFilePath);
            targetCellar.Execute(CellarOperation.Compress);

            bool isValid = targetCellar.IsValid();
            Assert.IsTrue(isValid);
            File.Delete(cellarFilePath);
        }
    }
}
