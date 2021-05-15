using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class GrapesTests
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
        public void GetAllGrapesShoudReturnObjects()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Grapes> items = cellar.GetAll<Grapes>();
            Assert.IsTrue(items.Count > 0);
        }

        [TestMethod]
        public void AddGrapes()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Grapes> items1 = cellar.GetAll<Grapes>();
            Grapes item = cellar.CreateObject<Grapes>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save();
            IList<Grapes> items2 = cellar.GetAll<Grapes>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void DeleteGrapes()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Grapes> items1 = cellar.GetAll<Grapes>();
            Grapes item = items1[0];
            item.Delete();
            IList<Grapes> items2 = cellar.GetAll<Grapes>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
