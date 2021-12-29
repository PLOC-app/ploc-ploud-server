using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class OwnerTests
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
        public void GetAllOwnersShoudNotReturnObjects()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Owner> items = cellar.GetAll<Owner>();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        public async Task GetAllOwnersShoudNotReturnObjectsAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Owner> items = await cellar.GetAllAsync<Owner>();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        public void AddOwner()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Owner> items1 = cellar.GetAll<Owner>();
            Owner item = cellar.CreateObject<Owner>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();
            IList<Owner> items2 = cellar.GetAll<Owner>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public async Task AddOwnerAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Owner> items1 = await cellar.GetAllAsync<Owner>();
            Owner item = cellar.CreateObject<Owner>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();
            IList<Owner> items2 = await cellar.GetAllAsync<Owner>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void SaveAndGetOwner()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Owner item = cellar.CreateObject<Owner>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();

            Assert.IsNotNull(cellar.Get<Owner>(item.Identifier));
            Assert.IsTrue(Shared.ObjectName == cellar.Get<Owner>(item.Identifier).Name);
        }

        [TestMethod]
        public async Task SaveAndGetOwnerAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Owner item = cellar.CreateObject<Owner>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            var item2 = await cellar.GetAsync<Owner>(item.Identifier);
            Assert.IsNotNull(item2);

            item = await cellar.GetAsync<Owner>(item.Identifier);
            Assert.IsTrue(Shared.ObjectName == item.Name);
        }

        [TestMethod]
        public void DeleteOwner()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);

            Owner item = cellar.CreateObject<Owner>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save(); 
            
            IList<Owner> items1 = cellar.GetAll<Owner>();
            items1[0].Delete();
            IList<Owner> items2 = cellar.GetAll<Owner>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }

        [TestMethod]
        public async Task DeleteOwnerAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);

            Owner item = cellar.CreateObject<Owner>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            IList<Owner> items1 = await cellar.GetAllAsync<Owner>();
            await items1[0].DeleteAsync();
            IList<Owner> items2 = await cellar.GetAllAsync<Owner>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
