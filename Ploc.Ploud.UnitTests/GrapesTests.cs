using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class GrapesTests
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
        public void GetAllGrapesShoudReturnObjects()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Grapes> items = cellar.GetAll<Grapes>();
            Assert.IsTrue(items.Count > 0);
        }

        [TestMethod]
        public async Task GetAllGrapesShoudReturnObjectsAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Grapes> items = await cellar.GetAllAsync<Grapes>();
            Assert.IsTrue(items.Count > 0);
        }

        [TestMethod]
        public void AddGrapes()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Grapes> items1 = cellar.GetAll<Grapes>();
            Grapes item = cellar.CreateObject<Grapes>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();
            IList<Grapes> items2 = cellar.GetAll<Grapes>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public async Task AddGrapesAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Grapes> items1 = await cellar.GetAllAsync<Grapes>();
            Grapes item = cellar.CreateObject<Grapes>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            IList<Grapes> items2 = await cellar.GetAllAsync<Grapes>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void SaveAndGetGrapes()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Grapes item = cellar.CreateObject<Grapes>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();

            Assert.IsNotNull(cellar.Get<Grapes>(item.Identifier));
            Assert.IsTrue(Shared.ObjectName == cellar.Get<Grapes>(item.Identifier).Name);
        }

        [TestMethod]
        public async Task SaveAndGetGrapesAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Grapes item = cellar.CreateObject<Grapes>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            var item2 = await cellar.GetAsync<Grapes>(item.Identifier);
            Assert.IsNotNull(item2);

            item = await cellar.GetAsync<Grapes>(item.Identifier);
            Assert.IsTrue(Shared.ObjectName == item.Name);
        }

        [TestMethod]
        public void DeleteGrapes()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Grapes> items1 = cellar.GetAll<Grapes>();
            Grapes item = items1[0];
            item.Delete();
            IList<Grapes> items2 = cellar.GetAll<Grapes>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }

        [TestMethod]
        public async Task DeleteGrapesAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Grapes> items1 = await cellar.GetAllAsync<Grapes>();
            Grapes item = items1[0];
            await item.DeleteAsync();
            IList<Grapes> items2 = await cellar.GetAllAsync<Grapes>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
