using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();
            IList<RackItem> items2 = cellar.GetAll<RackItem>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public async Task AddRackItemAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<RackItem> items1 = await cellar.GetAllAsync<RackItem>();
            RackItem item = cellar.CreateObject<RackItem>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            IList<RackItem> items2 = await cellar.GetAllAsync<RackItem>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void SaveAndGetRackItem()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            RackItem item = cellar.CreateObject<RackItem>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();

            Assert.IsNotNull(cellar.Get<RackItem>(item.Identifier));
            Assert.IsTrue(Shared.ObjectName == cellar.Get<RackItem>(item.Identifier).Name);
        }

        [TestMethod]
        public async Task SaveAndGetRackItemAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            RackItem item = cellar.CreateObject<RackItem>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            var item2 = await cellar.GetAsync<RackItem>(item.Identifier);
            Assert.IsNotNull(item2);

            item = await cellar.GetAsync<RackItem>(item.Identifier);
            Assert.IsTrue(Shared.ObjectName == item.Name);
        }

        [TestMethod]
        public void DeleteRackItem()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            RackItem item = cellar.CreateObject<RackItem>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();

            IList<RackItem> items1 = cellar.GetAll<RackItem>();
            items1[0].Delete();
            IList<RackItem> items2 = cellar.GetAll<RackItem>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }

        [TestMethod]
        public async Task DeleteRackItemAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            RackItem item = cellar.CreateObject<RackItem>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            IList<RackItem> items1 = await cellar.GetAllAsync<RackItem>();
            await items1[0].DeleteAsync();
            IList<RackItem> items2 = await cellar.GetAllAsync<RackItem>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
