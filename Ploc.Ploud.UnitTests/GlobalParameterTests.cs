using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class GlobalParameterTests
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
        public void GetAllGlobalParametersShoudNotReturnObjects()
        {
            ICellar cellar = Shared.Cellar();
            IList<GlobalParameter> items = cellar.GetAll<GlobalParameter>();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        public void AddGlobalParameter()
        {
            ICellar cellar = Shared.Cellar();
            IList<GlobalParameter> items1 = cellar.GetAll<GlobalParameter>();
            GlobalParameter item = cellar.CreateObject<GlobalParameter>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save();
            IList<GlobalParameter> items2 = cellar.GetAll<GlobalParameter>();
            Assert.IsTrue((items1.Count + 1) == items2.Count);
        }

        [TestMethod]
        public void DeleteGlobalParameter()
        {
            ICellar cellar = Shared.Cellar();
            IList<GlobalParameter> items1 = cellar.GetAll<GlobalParameter>();
            GlobalParameter item = items1[0];
            item.Delete();
            IList<GlobalParameter> items2 = cellar.GetAll<GlobalParameter>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
