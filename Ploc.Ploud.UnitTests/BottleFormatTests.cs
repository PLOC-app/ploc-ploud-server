using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System;
using System.Collections.Generic;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class BottleFormatTests
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
        public void GetAllBottlesFormatsShoudReturnObjects()
        {
            ICellar cellar = Shared.Cellar();
            IList<BottleFormat> bottleFormats = cellar.GetAll<BottleFormat>();
            Assert.IsTrue(bottleFormats.Count > 0);
        }

        [TestMethod]
        public void AddBottleFormat()
        {
            ICellar cellar = Shared.Cellar();
            IList<BottleFormat> bottleFormats = cellar.GetAll<BottleFormat>();
            BottleFormat bottleFormat = cellar.CreateObject<BottleFormat>();
            bottleFormat.Identifier = "HELLO";
            bottleFormat.Volume = 1.25;
            bottleFormat.Name = "Hello World";
            bottleFormat.Save();
            IList<BottleFormat> bottleFormats2 = cellar.GetAll<BottleFormat>();
            Assert.IsTrue((bottleFormats.Count + 1) == bottleFormats2.Count);
        }

        [TestMethod]
        public void DeleteBottleFormat()
        {
            ICellar cellar = Shared.Cellar();
            IList<BottleFormat> bottleFormats = cellar.GetAll<BottleFormat>();
            BottleFormat bottleFormat = bottleFormats[0];
            bottleFormat.Delete();

            IList<BottleFormat> bottleFormats2 = cellar.GetAll<BottleFormat>();
            Assert.IsTrue((bottleFormats.Count - 1) == bottleFormats2.Count);
        }
    }
}
