using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class VendorTests
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
        public void GetAllVendorsShoudNotReturnObjects()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Vendor> items = cellar.GetAll<Vendor>();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        public async Task GetAllVendorsShoudNotReturnObjectsAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Vendor> items = await cellar.GetAllAsync<Vendor>();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        public void AddVendor()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Vendor> items1 = cellar.GetAll<Vendor>();
            Vendor item = cellar.CreateObject<Vendor>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();
            IList<Vendor> items2 = cellar.GetAll<Vendor>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public async Task AddVendorAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Vendor> items1 = await cellar.GetAllAsync<Vendor>();
            Vendor item = cellar.CreateObject<Vendor>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            IList<Vendor> items2 = await cellar.GetAllAsync<Vendor>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void SaveAndGetVendor()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Vendor item = cellar.CreateObject<Vendor>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();

            Assert.IsNotNull(cellar.Get<Vendor>(item.Identifier));
            Assert.IsTrue(Shared.ObjectName == cellar.Get<Vendor>(item.Identifier).Name);
        }

        [TestMethod]
        public async Task SaveAndGetVendorAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Vendor item = cellar.CreateObject<Vendor>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            var item2 = await cellar.GetAsync<Vendor>(item.Identifier);
            Assert.IsNotNull(item2);

            item = await cellar.GetAsync<Vendor>(item.Identifier);
            Assert.IsTrue(Shared.ObjectName == item.Name);
        }

        [TestMethod]
        public void DeleteVendor()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Vendor item = cellar.CreateObject<Vendor>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save(); 
            
            IList<Vendor> items1 = cellar.GetAll<Vendor>();
            items1[0].Delete();
            IList<Vendor> items2 = cellar.GetAll<Vendor>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }

        [TestMethod]
        public async Task DeleteVendorAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Vendor item = cellar.CreateObject<Vendor>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            IList<Vendor> items1 = await cellar.GetAllAsync<Vendor>();
            await items1[0].DeleteAsync();
            IList<Vendor> items2 = await cellar.GetAllAsync<Vendor>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
