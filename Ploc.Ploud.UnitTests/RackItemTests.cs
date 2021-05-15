using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class RackItemTests
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
        public void AddRackItem()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<RackItem> items1 = cellar.GetAll<RackItem>();
            RackItem item = cellar.CreateObject<RackItem>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save();
            IList<RackItem> items2 = cellar.GetAll<RackItem>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void DeleteRackItem()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            RackItem item = cellar.CreateObject<RackItem>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save(); 
            
            IList<RackItem> items1 = cellar.GetAll<RackItem>();
            items1[0].Delete();
            IList<RackItem> items2 = cellar.GetAll<RackItem>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
