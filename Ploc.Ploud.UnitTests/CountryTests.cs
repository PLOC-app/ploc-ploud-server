using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class CountryTests
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
        public void GetAllCountriesShoudReturnObjects()
        {
            ICellar cellar = Shared.Cellar();
            IList<Country> items = cellar.GetAll<Country>();
            Assert.IsTrue(items.Count > 0);
        }

        [TestMethod]
        public void AddCountry()
        {
            ICellar cellar = Shared.Cellar();
            IList<Country> items1 = cellar.GetAll<Country>();
            Country item = cellar.CreateObject<Country>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save();
            IList<Country> items2 = cellar.GetAll<Country>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void DeleteCountry()
        {
            ICellar cellar = Shared.Cellar();
            IList<Country> items1 = cellar.GetAll<Country>();
            Country item = items1[0];
            item.Delete();
            IList<Country> items2 = cellar.GetAll<Country>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
