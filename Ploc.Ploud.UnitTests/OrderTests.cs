using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class OrderTests
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
        public void GetAllOrdersShoudNotReturnObjects()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Order> items = cellar.GetAll<Order>();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        public async Task GetAllOrdersShoudNotReturnObjectsAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Order> items = await cellar.GetAllAsync<Order>();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        public void AddOrder()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Order> items1 = cellar.GetAll<Order>();
            Order item = cellar.CreateObject<Order>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();
            IList<Order> items2 = cellar.GetAll<Order>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public async Task AddOrderAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Order> items1 = await cellar.GetAllAsync<Order>();
            Order item = cellar.CreateObject<Order>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            IList<Order> items2 = await cellar.GetAllAsync<Order>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void SaveAndGetOrder()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Order item = cellar.CreateObject<Order>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();

            Assert.IsNotNull(cellar.Get<Order>(item.Identifier));
            Assert.IsTrue(Shared.ObjectName == cellar.Get<Order>(item.Identifier).Name);
        }

        [TestMethod]
        public async Task SaveAndGetOrderAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Order item = cellar.CreateObject<Order>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            var item2 = await cellar.GetAsync<Order>(item.Identifier);
            Assert.IsNotNull(item2);

            item = await cellar.GetAsync<Order>(item.Identifier);
            Assert.IsTrue(Shared.ObjectName == item.Name);
        }

        [TestMethod]
        public void DeleteOrder()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Order item = cellar.CreateObject<Order>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save(); 
            
            IList<Order> items1 = cellar.GetAll<Order>();
            items1[0].Delete();
            IList<Order> items2 = cellar.GetAll<Order>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }

        [TestMethod]
        public async Task DeleteOrderAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Order item = cellar.CreateObject<Order>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            IList<Order> items1 = await cellar.GetAllAsync<Order>();
            await items1[0].DeleteAsync();

            IList<Order> items2 = await cellar.GetAllAsync<Order>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
