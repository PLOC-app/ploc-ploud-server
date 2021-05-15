using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class RackTests
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
        public void AddRack()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Rack> items1 = cellar.GetAll<Rack>();
            Rack item = cellar.CreateObject<Rack>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save();
            IList<Rack> items2 = cellar.GetAll<Rack>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void DeleteRack()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Rack item = cellar.CreateObject<Rack>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save(); 
            IList<Rack> items1 = cellar.GetAll<Rack>();
            items1[0].Delete();
            IList<Rack> items2 = cellar.GetAll<Rack>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
