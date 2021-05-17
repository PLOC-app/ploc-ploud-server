using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        public async Task GetAllWinesShoudNotReturnObjectsAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Wine> items = await cellar.GetAllAsync<Wine>();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        public void AddWine()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Wine> items1 = cellar.GetAll<Wine>();
            Wine item = cellar.CreateObject<Wine>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();
            IList<Wine> items2 = cellar.GetAll<Wine>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public async Task AddWineAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Wine> items1 = await cellar.GetAllAsync<Wine>();
            Wine item = cellar.CreateObject<Wine>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            IList<Wine> items2 = await cellar.GetAllAsync<Wine>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void SaveAndGetWine()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Wine item = cellar.CreateObject<Wine>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();

            Assert.IsNotNull(cellar.Get<Wine>(item.Identifier));
            Assert.IsTrue(Shared.ObjectName == cellar.Get<Wine>(item.Identifier).Name);
        }

        [TestMethod]
        public async Task SaveAndGetWineAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Wine item = cellar.CreateObject<Wine>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            var item2 = await cellar.GetAsync<Wine>(item.Identifier);
            Assert.IsNotNull(item2);

            item = await cellar.GetAsync<Wine>(item.Identifier);
            Assert.IsTrue(Shared.ObjectName == item.Name);
        }

        [TestMethod]
        public void DeleteWine()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Wine item = cellar.CreateObject<Wine>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();

            IList<Wine> items1 = cellar.GetAll<Wine>();
            items1[0].Delete();
            IList<Wine> items2 = cellar.GetAll<Wine>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }

        [TestMethod]
        public async Task DeleteWineAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Wine item = cellar.CreateObject<Wine>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            IList<Wine> items1 = await cellar.GetAllAsync<Wine>();
            items1[0].Delete();
            IList<Wine> items2 = await cellar.GetAllAsync<Wine>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
