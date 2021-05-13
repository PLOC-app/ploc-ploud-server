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
            Shared.CopyDatabase();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Shared.DeleteDatabase();
        }

        [TestMethod]
        public void GetAllTastingNotesShoudNotReturnObjects()
        {
            ICellar cellar = Shared.Cellar();
            IList<TastingNotes> items = cellar.GetAll<TastingNotes>();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        public void AddTastingNotes()
        {
            ICellar cellar = Shared.Cellar();
            IList<TastingNotes> items1 = cellar.GetAll<TastingNotes>();
            Wine item = cellar.CreateObject<Wine>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save();
            IList<TastingNotes> items2 = cellar.GetAll<TastingNotes>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void DeleteTastingNotes()
        {
            ICellar cellar = Shared.Cellar();
            IList<TastingNotes> items1 = cellar.GetAll<TastingNotes>();
            TastingNotes item = items1[0];
            item.Delete();
            IList<TastingNotes> items2 = cellar.GetAll<TastingNotes>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
