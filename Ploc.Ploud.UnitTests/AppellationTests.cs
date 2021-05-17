using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class AppellationTests
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
        public void GetAllAppellationsShoudReturnObjects()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Appellation> items = cellar.GetAll<Appellation>();
            Assert.IsTrue(items.Count > 0);
        }

        [TestMethod]
        public async Task GetAllAppellationsShoudReturnObjectsAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Appellation> items = await cellar.GetAllAsync<Appellation>();
            Assert.IsTrue(items.Count > 0);
        }

        [TestMethod]
        public void AddAppellation()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Appellation> items1 = cellar.GetAll<Appellation>();
            Appellation item = cellar.CreateObject<Appellation>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();
            IList<Appellation> items2 = cellar.GetAll<Appellation>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public async Task AddAppellationAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Appellation> items1 = await cellar.GetAllAsync<Appellation>();
            Appellation item = cellar.CreateObject<Appellation>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();
            IList<Appellation> items2 = await cellar.GetAllAsync<Appellation>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void SaveAndGetAppellation()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Appellation item = cellar.CreateObject<Appellation>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();

            Assert.IsNotNull(cellar.Get<Appellation>(item.Identifier));
            Assert.IsTrue(Shared.ObjectName == cellar.Get<Appellation>(item.Identifier).Name);
        }

        [TestMethod]
        public async Task SaveAndGetAppellationAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Appellation item = cellar.CreateObject<Appellation>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            var item2 = await cellar.GetAsync<Appellation>(item.Identifier);
            Assert.IsNotNull(item2);

            item = await cellar.GetAsync<Appellation>(item.Identifier);
            Assert.IsTrue(Shared.ObjectName == item.Name);
        }

        [TestMethod]
        public void DeleteAppellation()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Appellation> items1 = cellar.GetAll<Appellation>();
            Appellation item = items1[0];
            item.Delete();
            IList<Appellation> items2 = cellar.GetAll<Appellation>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }

        [TestMethod]
        public async Task DeleteAppellationAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Appellation> items1 = await cellar.GetAllAsync<Appellation>();
            Appellation item = items1[0];
            await item.DeleteAsync();
            IList<Appellation> items2 = await cellar.GetAllAsync<Appellation>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
