using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class AppellationTests
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
        public void GetAllAppellationsShoudReturnObjects()
        {
            ICellar cellar = Shared.Cellar();
            IList<Appellation> items = cellar.GetAll<Appellation>();
            Assert.IsTrue(items.Count > 0);
        }

        [TestMethod]
        public void AddAppellation()
        {
            ICellar cellar = Shared.Cellar();
            IList<Appellation> items1 = cellar.GetAll<Appellation>();
            Appellation item = cellar.CreateObject<Appellation>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save();
            IList<Appellation> items2 = cellar.GetAll<Appellation>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void DeleteAppellation()
        {
            ICellar cellar = Shared.Cellar();
            IList<Appellation> items1 = cellar.GetAll<Appellation>();
            Appellation item = items1[0];
            item.Delete();
            IList<Appellation> items2 = cellar.GetAll<Appellation>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
