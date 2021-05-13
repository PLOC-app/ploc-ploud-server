using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class VendorTests
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
        public void GetAllVendorsShoudNotReturnObjects()
        {
            ICellar cellar = Shared.Cellar();
            IList<Vendor> items = cellar.GetAll<Vendor>();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        public void AddVendor()
        {
            ICellar cellar = Shared.Cellar();
            IList<Vendor> items1 = cellar.GetAll<Vendor>();
            Vendor item = cellar.CreateObject<Vendor>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save();
            IList<Vendor> items2 = cellar.GetAll<Vendor>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void DeleteVendor()
        {
            ICellar cellar = Shared.Cellar();
            IList<Vendor> items1 = cellar.GetAll<Vendor>();
            Vendor item = items1[0];
            item.Delete();
            IList<Vendor> items2 = cellar.GetAll<Vendor>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
