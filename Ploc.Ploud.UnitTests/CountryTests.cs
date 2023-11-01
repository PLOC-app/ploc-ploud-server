using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class CountryTests
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
        public void GetAllCountriesShoudReturnObjects()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Country> items = cellar.GetAll<Country>();
            Assert.IsTrue(items.Count > 0);
        }

        [TestMethod]
        public async Task GetAllCountriesShoudReturnObjectsAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Country> items = await cellar.GetAllAsync<Country>();
            Assert.IsTrue(items.Count > 0);
        }

        [TestMethod]
        public void AddCountry()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Country> items1 = cellar.GetAll<Country>();
            Country item = cellar.CreateObject<Country>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();
            IList<Country> items2 = cellar.GetAll<Country>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public async Task AddCountryAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Country> items1 = await cellar.GetAllAsync<Country>();
            Country item = cellar.CreateObject<Country>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();
            IList<Country> items2 = await cellar.GetAllAsync<Country>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void SaveAndGetCountry()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Country item = cellar.CreateObject<Country>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();

            Assert.IsNotNull(cellar.Get<Country>(item.Identifier));
            Assert.IsTrue(Shared.ObjectName == cellar.Get<Country>(item.Identifier).Name);
        }

        [TestMethod]
        public async Task SaveAndGetCountryAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Country item = cellar.CreateObject<Country>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            var item2 = await cellar.GetAsync<Country>(item.Identifier);
            Assert.IsNotNull(item2);

            item = await cellar.GetAsync<Country>(item.Identifier);
            Assert.IsTrue(Shared.ObjectName == item.Name);
        }

        [TestMethod]
        public void DeleteCountry()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Country> items1 = cellar.GetAll<Country>();
            Country item = items1[0];
            item.Delete();
            IList<Country> items2 = cellar.GetAll<Country>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }

        [TestMethod]
        public async Task DeleteCountryAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Country> items1 = await cellar.GetAllAsync<Country>();
            Country item = items1[0];
            await item.DeleteAsync();
            IList<Country> items2 = await cellar.GetAllAsync<Country>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
