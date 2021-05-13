using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class RegionTests
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
        public void GetAllRegionsShoudReturnObjects()
        {
            ICellar cellar = Shared.Cellar();
            IList<Region> items = cellar.GetAll<Region>();
            Assert.IsTrue(items.Count > 0);
        }

        [TestMethod]
        public void AddCountry()
        {
            ICellar cellar = Shared.Cellar();
            IList<Region> items1 = cellar.GetAll<Region>();
            Region item = cellar.CreateObject<Region>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save();
            IList<Region> items2 = cellar.GetAll<Region>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void DeleteRegion()
        {
            ICellar cellar = Shared.Cellar();
            IList<Region> items1 = cellar.GetAll<Region>();
            Region item = items1[0];
            item.Delete();
            IList<Region> items2 = cellar.GetAll<Region>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
