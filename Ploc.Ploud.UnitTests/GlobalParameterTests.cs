using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class GlobalParameterTests
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
        public void GetAllGlobalParametersShoudNotReturnObjects()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<GlobalParameter> items = cellar.GetAll<GlobalParameter>();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        public async Task GetAllGlobalParametersShoudNotReturnObjectsAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<GlobalParameter> items = await cellar.GetAllAsync<GlobalParameter>();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        public void AddGlobalParameter()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<GlobalParameter> items1 = cellar.GetAll<GlobalParameter>();
            GlobalParameter item = cellar.CreateObject<GlobalParameter>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();
            IList<GlobalParameter> items2 = cellar.GetAll<GlobalParameter>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public async Task AddGlobalParameterAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<GlobalParameter> items1 = await cellar.GetAllAsync<GlobalParameter>();
            GlobalParameter item = cellar.CreateObject<GlobalParameter>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();
            IList<GlobalParameter> items2 = await cellar.GetAllAsync<GlobalParameter>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void SaveAndGetGlobalParameter()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            GlobalParameter item = cellar.CreateObject<GlobalParameter>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();

            Assert.IsNotNull(cellar.Get<GlobalParameter>(item.Identifier));
            Assert.IsTrue(Shared.ObjectName == cellar.Get<GlobalParameter>(item.Identifier).Name);
        }

        [TestMethod]
        public async Task SaveAndGetGlobalParameterAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            GlobalParameter item = cellar.CreateObject<GlobalParameter>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            var item2 = await cellar.GetAsync<GlobalParameter>(item.Identifier);
            Assert.IsNotNull(item2);

            item = await cellar.GetAsync<GlobalParameter>(item.Identifier);
            Assert.IsTrue(Shared.ObjectName == item.Name);
        }

        [TestMethod]
        public void DeleteGlobalParameter()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            GlobalParameter item = cellar.CreateObject<GlobalParameter>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();
            IList<GlobalParameter> items1 = cellar.GetAll<GlobalParameter>();
            items1[0].Delete();
            IList<GlobalParameter> items2 = cellar.GetAll<GlobalParameter>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }

        [TestMethod]
        public async Task DeleteGlobalParameterAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            GlobalParameter item = cellar.CreateObject<GlobalParameter>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            IList<GlobalParameter> items1 = await cellar.GetAllAsync<GlobalParameter>();
            await items1[0].DeleteAsync();

            IList<GlobalParameter> items2 = await cellar.GetAllAsync<GlobalParameter>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
