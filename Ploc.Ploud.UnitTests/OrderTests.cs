using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class OrderTests
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
        public void GetAllOrdersShoudNotReturnObjects()
        {
            ICellar cellar = Shared.Cellar();
            IList<Order> items = cellar.GetAll<Order>();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        public void AddOrder()
        {
            ICellar cellar = Shared.Cellar();
            IList<Order> items1 = cellar.GetAll<Order>();
            Order item = cellar.CreateObject<Order>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save();
            IList<Order> items2 = cellar.GetAll<Order>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void DeleteOrder()
        {
            ICellar cellar = Shared.Cellar();
            IList<Order> items1 = cellar.GetAll<Order>();
            Order item = items1[0];
            item.Delete();
            IList<Order> items2 = cellar.GetAll<Order>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
