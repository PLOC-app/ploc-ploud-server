using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class WineTests
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
        public void GetAllWinesShoudNotReturnObjects()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Wine> items = cellar.GetAll<Wine>();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        public void AddWine()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Wine> items1 = cellar.GetAll<Wine>();
            Wine item = cellar.CreateObject<Wine>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save();
            IList<Wine> items2 = cellar.GetAll<Wine>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void DeleteWine()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Wine item = cellar.CreateObject<Wine>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save();

            IList<Wine> items1 = cellar.GetAll<Wine>();
            items1[0].Delete();
            IList<Wine> items2 = cellar.GetAll<Wine>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
