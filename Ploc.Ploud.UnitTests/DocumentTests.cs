using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.IO;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class DocumentTests
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
        public void GetAllDocumentsShoudNotReturnObjects()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Document> items = cellar.GetAll<Document>();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        public void AddDocument()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Document> items1 = cellar.GetAll<Document>();
            Document item = cellar.CreateObject<Document>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save();
            IList<Document> items2 = cellar.GetAll<Document>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void DeleteDocument()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Document item = cellar.CreateObject<Document>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save();
            IList<Document> items1 = cellar.GetAll<Document>();
            items1[0].Delete();
            IList<Document> items2 = cellar.GetAll<Document>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }

        [TestMethod]
        public void AddDocumentThenSave()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Document> items1 = cellar.GetAll<Document>();
            Document item = cellar.CreateObject<Document>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Data = GetLogo();
            item.Save();

            Console.WriteLine("Document.Length = {0}", item.Data.Length);
            Document item2 = cellar.Get<Document>(item.Identifier);
            Assert.AreEqual(item.Data.Length, item2.Data.Length);

            File.WriteAllBytes(@"c:\temp\ploc.png", item2.Data);
        }

        private byte[] GetLogo()
        {
            using (var resource = typeof(Shared).Assembly.GetManifestResourceStream("Ploc.Ploud.UnitTests.Resources.logo.webp"))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    resource.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
