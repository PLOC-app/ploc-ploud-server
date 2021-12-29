using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class RackTests
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
        public void AddRack()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Rack> items1 = cellar.GetAll<Rack>();
            Rack item = cellar.CreateObject<Rack>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();
            IList<Rack> items2 = cellar.GetAll<Rack>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public async Task AddRackAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Rack> items1 = await cellar.GetAllAsync<Rack>();
            Rack item = cellar.CreateObject<Rack>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            IList<Rack> items2 = await cellar.GetAllAsync<Rack>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void SaveAndGetRack()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Rack item = cellar.CreateObject<Rack>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();

            Assert.IsNotNull(cellar.Get<Rack>(item.Identifier));
            Assert.IsTrue(Shared.ObjectName == cellar.Get<Rack>(item.Identifier).Name);
        }

        [TestMethod]
        public async Task SaveAndGetRackItemAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Rack item = cellar.CreateObject<Rack>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            var item2 = await cellar.GetAsync<Rack>(item.Identifier);
            Assert.IsNotNull(item2);

            item = await cellar.GetAsync<Rack>(item.Identifier);
            Assert.IsTrue(Shared.ObjectName == item.Name);
        }

        [TestMethod]
        public void DeleteRack()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Rack item = cellar.CreateObject<Rack>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save(); 
            IList<Rack> items1 = cellar.GetAll<Rack>();
            items1[0].Delete();
            IList<Rack> items2 = cellar.GetAll<Rack>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }

        [TestMethod]
        public async Task DeleteRackAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Rack item = cellar.CreateObject<Rack>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();
            IList<Rack> items1 = await cellar.GetAllAsync<Rack>();
            await items1[0].DeleteAsync();

            IList<Rack> items2 = await cellar.GetAllAsync<Rack>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
