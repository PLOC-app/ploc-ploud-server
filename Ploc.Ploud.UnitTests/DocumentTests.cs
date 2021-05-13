using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class DocumentTests
    {
        [TestInitialize]
        public void TestInitialize()
        {
            Shared.CopyDatabase();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Shared.DeleteDatabase();
        }

        [TestMethod]
        public void GetAllDocumentsShoudNotReturnObjects()
        {
            ICellar cellar = Shared.Cellar();
            IList<Document> items = cellar.GetAll<Document>();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        public void AddDocument()
        {
            ICellar cellar = Shared.Cellar();
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
            ICellar cellar = Shared.Cellar();
            IList<Document> items1 = cellar.GetAll<Document>();
            Document item = items1[0];
            item.Delete();
            IList<Document> items2 = cellar.GetAll<Document>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
