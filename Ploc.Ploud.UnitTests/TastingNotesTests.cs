using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;

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
        public void AddTastingNotes()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<TastingNotes> items1 = cellar.GetAll<TastingNotes>();
            TastingNotes item = cellar.CreateObject<TastingNotes>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save();
            IList<TastingNotes> items2 = cellar.GetAll<TastingNotes>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void DeleteTastingNotes()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            TastingNotes item = cellar.CreateObject<TastingNotes>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save();

            IList<TastingNotes> items1 = cellar.GetAll<TastingNotes>();
            items1[0].Delete();
            IList<TastingNotes> items2 = cellar.GetAll<TastingNotes>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
