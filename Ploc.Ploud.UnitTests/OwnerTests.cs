using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class OwnerTests
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
        public void GetAllOwnersShoudNotReturnObjects()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Owner> items = cellar.GetAll<Owner>();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        public void AddOwner()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<Owner> items1 = cellar.GetAll<Owner>();
            Owner item = cellar.CreateObject<Owner>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save();
            IList<Owner> items2 = cellar.GetAll<Owner>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void DeleteOwner()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);

            Owner item = cellar.CreateObject<Owner>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save(); 
            
            IList<Owner> items1 = cellar.GetAll<Owner>();
            items1[0].Delete();
            IList<Owner> items2 = cellar.GetAll<Owner>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
