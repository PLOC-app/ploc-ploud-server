using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            SyncObjects syncObjects = cellar.GetSyncObjects(new SyncObjectsOptions(DateTime.UtcNow.GetSecondsSince1970(), "UnitTest"));
            Assert.IsTrue(syncObjects.Count == 0);
        }

        [TestMethod]
        public async Task GetSyncObjecsShoudNotReturnObjectsAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            SyncObjects syncObjects = await cellar.GetSyncObjectsAsync(new SyncObjectsOptions(DateTime.UtcNow.GetSecondsSince1970(), "UnitTest"));
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

        [TestMethod]
        public async Task GetSyncObjecsShoudReturnManyObjectsAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            SyncObjects syncObjects = await cellar.GetSyncObjectsAsync(new SyncObjectsOptions(1, "UnitTest"));
            Console.WriteLine("GetSyncObjecsShoudReturnManyObjects.Count = {0}", syncObjects.Count);
            Console.WriteLine("GetSyncObjecsShoudReturnManyObjects.Color[0].Name = {0}", syncObjects.Colors[0].Name);
            Assert.IsTrue(syncObjects.Count > 0);
        }
    }
}
