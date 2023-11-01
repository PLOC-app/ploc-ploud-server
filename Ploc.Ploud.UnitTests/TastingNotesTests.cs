using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class TastingNotesTests
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
        public void GetAllTastingNotesShoudNotReturnObjects()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<TastingNotes> items = cellar.GetAll<TastingNotes>();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        public async Task GetAllTastingNotesShoudNotReturnObjectsAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<TastingNotes> items = await cellar.GetAllAsync<TastingNotes>();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        public void AddTastingNotes()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<TastingNotes> items1 = cellar.GetAll<TastingNotes>();
            TastingNotes item = cellar.CreateObject<TastingNotes>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();
            IList<TastingNotes> items2 = cellar.GetAll<TastingNotes>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public async Task AddTastingNotesAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<TastingNotes> items1 = await cellar.GetAllAsync<TastingNotes>();
            TastingNotes item = cellar.CreateObject<TastingNotes>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            IList<TastingNotes> items2 = await cellar.GetAllAsync<TastingNotes>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void SaveAndGetTastingNotes()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            TastingNotes item = cellar.CreateObject<TastingNotes>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();

            Assert.IsNotNull(cellar.Get<TastingNotes>(item.Identifier));
            Assert.IsTrue(Shared.ObjectName == cellar.Get<TastingNotes>(item.Identifier).Name);
        }

        [TestMethod]
        public async Task SaveAndGetTastingNotesAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            TastingNotes item = cellar.CreateObject<TastingNotes>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            var item2 = await cellar.GetAsync<TastingNotes>(item.Identifier);
            Assert.IsNotNull(item2);

            item = await cellar.GetAsync<TastingNotes>(item.Identifier);
            Assert.IsTrue(Shared.ObjectName == item.Name);
        }

        [TestMethod]
        public void DeleteTastingNotes()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            TastingNotes item = cellar.CreateObject<TastingNotes>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            item.Save();

            IList<TastingNotes> items1 = cellar.GetAll<TastingNotes>();
            items1[0].Delete();
            IList<TastingNotes> items2 = cellar.GetAll<TastingNotes>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }

        [TestMethod]
        public async Task DeleteTastingNotesAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            TastingNotes item = cellar.CreateObject<TastingNotes>();
            item.Identifier = Shared.ObjectIdentifier;
            item.Name = Shared.ObjectName;
            await item.SaveAsync();

            IList<TastingNotes> items1 = await cellar.GetAllAsync<TastingNotes>();
            await items1[0].DeleteAsync();
            IList<TastingNotes> items2 = await cellar.GetAllAsync<TastingNotes>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
