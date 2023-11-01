using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class RegionTests
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
        public void GetAllRegionsShoudReturnObjects()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Region> items = cellar.GetAll<Region>();
            Assert.IsTrue(items.Count > 0);
        }

        [TestMethod]
        public async Task GetAllRegionsShoudReturnObjectsAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Region> items = await cellar.GetAllAsync<Region>();
            Assert.IsTrue(items.Count > 0);
        }

        [TestMethod]
        public void AddRegion()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Region> items1 = cellar.GetAll<Region>();
            Region item = cellar.CreateObject<Region>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();
            IList<Region> items2 = cellar.GetAll<Region>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public async Task AddRegionAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Region> items1 = await cellar.GetAllAsync<Region>();
            Region item = cellar.CreateObject<Region>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            IList<Region> items2 = await cellar.GetAllAsync<Region>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void SaveAndGetRegion()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Region item = cellar.CreateObject<Region>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();

            Assert.IsNotNull(cellar.Get<Region>(item.Identifier));
            Assert.IsTrue(Shared.ObjectName == cellar.Get<Region>(item.Identifier).Name);
        }

        [TestMethod]
        public async Task SaveAndGetRegionAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Region item = cellar.CreateObject<Region>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            var item2 = await cellar.GetAsync<Region>(item.Identifier);
            Assert.IsNotNull(item2);

            item = await cellar.GetAsync<Region>(item.Identifier);
            Assert.IsTrue(Shared.ObjectName == item.Name);
        }

        [TestMethod]
        public void DeleteRegion()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Region> items1 = cellar.GetAll<Region>();
            Region item = items1[0];
            item.Delete();
            IList<Region> items2 = cellar.GetAll<Region>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }

        [TestMethod]
        public async Task DeleteRegionAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Region> items1 = await cellar.GetAllAsync<Region>();
            Region item = items1[0];
            await item.DeleteAsync();

            IList<Region> items2 = await cellar.GetAllAsync<Region>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
