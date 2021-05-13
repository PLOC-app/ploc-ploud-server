using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class ClassificationTests
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
        public void GetAllClassificationsShoudReturnObjects()
        {
            ICellar cellar = Shared.Cellar();
            IList<Classification> items = cellar.GetAll<Classification>();
            Assert.IsTrue(items.Count > 0);
        }

        [TestMethod]
        public void AddClassification()
        {
            ICellar cellar = Shared.Cellar();
            IList<Classification> items1 = cellar.GetAll<Classification>();
            Classification item = cellar.CreateObject<Classification>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save();
            IList<Classification> items2 = cellar.GetAll<Classification>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void DeleteClassification()
        {
            ICellar cellar = Shared.Cellar();
            IList<Classification> items1 = cellar.GetAll<Classification>();
            Classification item = items1[0];
            item.Delete();
            IList<Classification> items2 = cellar.GetAll<Classification>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
