using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class ClassificationTests
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
        public void GetAllClassificationsShoudReturnObjects()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Classification> items = cellar.GetAll<Classification>();
            Assert.IsTrue(items.Count > 0);
        }

        [TestMethod]
        public async Task GetAllClassificationsShoudReturnObjectsAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Classification> items = await cellar.GetAllAsync<Classification>();
            Assert.IsTrue(items.Count > 0);
        }

        [TestMethod]
        public void AddClassification()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Classification> items1 = cellar.GetAll<Classification>();
            Classification item = cellar.CreateObject<Classification>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();
            IList<Classification> items2 = cellar.GetAll<Classification>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public async Task AddClassificationAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Classification> items1 = await cellar.GetAllAsync<Classification>();
            Classification item = cellar.CreateObject<Classification>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            IList<Classification> items2 = await cellar.GetAllAsync<Classification>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void SaveAndGetClassification()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Classification item = cellar.CreateObject<Classification>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();

            Assert.IsNotNull(cellar.Get<Classification>(item.Identifier));
            Assert.IsTrue(Shared.ObjectName == cellar.Get<Classification>(item.Identifier).Name);
        }

        [TestMethod]
        public async Task SaveAndGetClassificationAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Classification item = cellar.CreateObject<Classification>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            var item2 = await cellar.GetAsync<Classification>(item.Identifier);
            Assert.IsNotNull(item2);

            item = await cellar.GetAsync<Classification>(item.Identifier);
            Assert.IsTrue(Shared.ObjectName == item.Name);
        }

        [TestMethod]
        public void DeleteClassification()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Classification> items1 = cellar.GetAll<Classification>();
            Classification item = items1[0];
            item.Delete();
            IList<Classification> items2 = cellar.GetAll<Classification>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }

        [TestMethod]
        public async Task DeleteClassificationAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Classification> items1 = await cellar.GetAllAsync<Classification>();
            Classification item = items1[0];
            await item.DeleteAsync();
            IList<Classification> items2 = await cellar.GetAllAsync<Classification>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
