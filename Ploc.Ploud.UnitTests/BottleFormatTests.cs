using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class BottleFormatTests
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
        public void GetAllBottlesFormatsShoudReturnObjects()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<BottleFormat> bottleFormats = cellar.GetAll<BottleFormat>();
            Assert.IsTrue(bottleFormats.Count > 0);
        }

        [TestMethod]
        public async Task GetAllBottlesFormatsShoudReturnObjectsAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<BottleFormat> bottleFormats = await cellar.GetAllAsync<BottleFormat>();
            Assert.IsTrue(bottleFormats.Count > 0);
        }

        [TestMethod]
        public void AddBottleFormat()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<BottleFormat> bottleFormats = cellar.GetAll<BottleFormat>();
            BottleFormat bottleFormat = cellar.CreateObject<BottleFormat>();
            bottleFormat.Identifier = Shared.ObjectIdentifier;
            bottleFormat.Volume = 1.25;
            bottleFormat.Name = Shared.ObjectName;
            bottleFormat.Save();
            IList<BottleFormat> bottleFormats2 = cellar.GetAll<BottleFormat>();
            Assert.IsTrue((bottleFormats.Count + 1) == bottleFormats2.Count);
        }

        [TestMethod]
        public async Task AddBottleFormatAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<BottleFormat> bottleFormats = await cellar.GetAllAsync<BottleFormat>();
            BottleFormat bottleFormat = cellar.CreateObject<BottleFormat>();
            bottleFormat.Identifier = Shared.ObjectIdentifier;
            bottleFormat.Volume = 1.25;
            bottleFormat.Name = Shared.ObjectName;
            await bottleFormat.SaveAsync();

            IList<BottleFormat> bottleFormats2 = await cellar.GetAllAsync<BottleFormat>();
            Assert.IsTrue((bottleFormats.Count + 1) == bottleFormats2.Count);
        }

        [TestMethod]
        public void SaveAndGetBottleFormat()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            BottleFormat bottleFormat = cellar.CreateObject<BottleFormat>();
            bottleFormat.Identifier = Shared.ObjectIdentifier;
            bottleFormat.Volume = 1.25;
            bottleFormat.Name = Shared.ObjectName;
            bottleFormat.Save();
            Assert.IsNotNull(cellar.Get<BottleFormat>(bottleFormat.Identifier));
            Assert.IsTrue(Shared.ObjectName == cellar.Get<BottleFormat>(bottleFormat.Identifier).Name);
        }

        [TestMethod]
        public async Task SaveAndGetBottleFormatAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            BottleFormat bottleFormat = cellar.CreateObject<BottleFormat>();
            bottleFormat.Identifier = Shared.ObjectIdentifier;
            bottleFormat.Volume = 1.25;
            bottleFormat.Name = Shared.ObjectName;
            await bottleFormat.SaveAsync();

            var item2 = await cellar.GetAsync<BottleFormat>(bottleFormat.Identifier);
            Assert.IsNotNull(item2);

            var item = await cellar.GetAsync<BottleFormat>(bottleFormat.Identifier);
            Assert.IsTrue(Shared.ObjectName == item.Name);
        }

        [TestMethod]
        public void DeleteBottleFormat()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<BottleFormat> bottleFormats = cellar.GetAll<BottleFormat>();
            BottleFormat bottleFormat = bottleFormats[0];
            bottleFormat.Delete();
            IList<BottleFormat> bottleFormats2 = cellar.GetAll<BottleFormat>();
            Assert.IsTrue((bottleFormats.Count - 1) == bottleFormats2.Count);
        }

        [TestMethod]
        public async Task DeleteBottleFormatAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<BottleFormat> bottleFormats = await cellar.GetAllAsync<BottleFormat>();
            BottleFormat bottleFormat = bottleFormats[0];
            await bottleFormat.DeleteAsync();
            IList<BottleFormat> bottleFormats2 = await cellar.GetAllAsync<BottleFormat>();
            Assert.IsTrue((bottleFormats.Count - 1) == bottleFormats2.Count);
        }
    }
}
