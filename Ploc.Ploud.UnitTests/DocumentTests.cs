using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

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
        public async Task GetAllDocumentsShoudNotReturnObjectsAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Document> items = await cellar.GetAllAsync<Document>();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        public void AddDocument()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Document> items1 = cellar.GetAll<Document>();
            Document item = cellar.CreateObject<Document>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();
            IList<Document> items2 = cellar.GetAll<Document>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public async Task AddDocumentAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Document> items1 = await cellar.GetAllAsync<Document>();
            Document item = cellar.CreateObject<Document>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            IList<Document> items2 = await cellar.GetAllAsync<Document>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void SaveAndGetDocument()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Document item = cellar.CreateObject<Document>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();

            Assert.IsNotNull(cellar.Get<Document>(item.Identifier));
            Assert.IsTrue(Shared.ObjectName == cellar.Get<Document>(item.Identifier).Name);
        }

        [TestMethod]
        public async Task SaveAndGetDocumentAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Document item = cellar.CreateObject<Document>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            var item2 = await cellar.GetAsync<Document>(item.Identifier);
            Assert.IsNotNull(item2);

            item = await cellar.GetAsync<Document>(item.Identifier);
            Assert.IsTrue(Shared.ObjectName == item.Name);
        }

        [TestMethod]
        public void DeleteDocument()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Document item = cellar.CreateObject<Document>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();
            IList<Document> items1 = cellar.GetAll<Document>();
            items1[0].Delete();
            IList<Document> items2 = cellar.GetAll<Document>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }

        [TestMethod]
        public async Task DeleteDocumentAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Document item = cellar.CreateObject<Document>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();
            IList<Document> items1 = await cellar.GetAllAsync<Document>();
            await items1[0].DeleteAsync();
            IList<Document> items2 = await cellar.GetAllAsync<Document>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }

        [TestMethod]
        public void AddDocumentThenSave()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Document item = cellar.CreateObject<Document>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Data = GetLogo();
            item.Save();

            Console.WriteLine("Document.Length = {0}", item.Data.Length);
            Document item2 = cellar.Get<Document>(item.Identifier);
            Assert.AreEqual(item.Data.Length, item2.Data.Length);

            File.WriteAllBytes(@"c:\temp\ploc.png", item2.Data);
        }

        [TestMethod]
        public async Task AddDocumentThenSaveAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Document item = cellar.CreateObject<Document>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Data = GetLogo();
            await item.SaveAsync();

            Console.WriteLine("Document.Length = {0}", item.Data.Length);
            Document item2 = await cellar.GetAsync<Document>(item.Identifier);
            Assert.AreEqual(item.Data.Length, item2.Data.Length);

            await File.WriteAllBytesAsync(@"c:\temp\ploc.png", item2.Data);
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
