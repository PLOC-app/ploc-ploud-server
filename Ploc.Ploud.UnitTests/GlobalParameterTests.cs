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
            Shared.CopyDatabase(GetType().Name);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Shared.DeleteDatabase(GetType().Name);
        }

        [TestMethod]
        public void GetAllGlobalParametersShoudNotReturnObjects()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            IList<GlobalParameter> items = cellar.GetAll<GlobalParameter>();
            Assert.IsTrue(items.Count == 0);
        }

        [TestMethod]
        public void AddGlobalParameter()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
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
            ICellar cellar = Shared.Cellar(GetType().Name);
            GlobalParameter item = cellar.CreateObject<GlobalParameter>();
            item.Identifier = "HELLO";
            item.Name = "Hello World";
            item.Save();
            IList<GlobalParameter> items1 = cellar.GetAll<GlobalParameter>();
            items1[0].Delete();
            IList<GlobalParameter> items2 = cellar.GetAll<GlobalParameter>();
            Assert.IsTrue((items1.Count - 1) == items2.Count);
        }
    }
}
