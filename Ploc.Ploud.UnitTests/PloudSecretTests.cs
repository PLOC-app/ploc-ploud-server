using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class PloudSecretTests
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
        public void TestPloudSecretIsGenerated()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Assert.IsNotNull(cellar, "Cellar");
            PloudSecret ploudSecret = cellar.Get<PloudSecret>(PloudSecret.GlobalIdentifier);
            Assert.IsNotNull(ploudSecret, "PloudSecret");
            Assert.IsNotNull(ploudSecret.Key, "PloudSecret.Key");
            Assert.IsNotNull(ploudSecret.Iv, "PloudSecret.Iv");
            Console.WriteLine("PloudSecret.Key = {0}", ploudSecret.Key);
            Console.WriteLine("PloudSecret.Iv = {0}", ploudSecret.Iv);
        }
    }
}
