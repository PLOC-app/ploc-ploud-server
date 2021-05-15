using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class SyncObjectsTests
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
        public void GetSyncObjecsShoudNotReturnObjects()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            SyncObjects syncObjects = cellar.GetSyncObjects(new SyncObjectsOptions(DateTime.UtcNow.LongValue(), "UnitTest"));
            Assert.IsTrue(syncObjects.Count == 0);
        }

        [TestMethod]
        public void GetSyncObjecsShoudReturnManyObjects()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            SyncObjects syncObjects = cellar.GetSyncObjects(new SyncObjectsOptions(1, "UnitTest"));
            Console.WriteLine("GetSyncObjecsShoudReturnManyObjects.Count = {0}", syncObjects.Count);
            Console.WriteLine("GetSyncObjecsShoudReturnManyObjects.Color[0].Name = {0}", syncObjects.Colors[0].Name);
            Assert.IsTrue(syncObjects.Count > 0);
        }
    }
}
