using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ploc.Ploud.Library;
using System.Threading.Tasks;

namespace Ploc.Ploud.UnitTests
{
    [TestClass]
    public class DashboardTests
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
        public async Task GetDashboardShoudReturnEmptyAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Dashboard dashboard = await cellar.GetDashboardAsync();
            Assert.IsTrue(dashboard.Wines == 0);
            Assert.IsTrue(dashboard.Cellars == 0);
            Assert.IsTrue(dashboard.TastingNotes == 0);
            Assert.IsTrue(dashboard.FileSize > 0);
        }

        [TestMethod]
        public async Task GetDashboardShoudReturnResultsAsync()
        {
            ICellar cellar = Shared.Cellar(GetType().Name);
            Wine wine = cellar.CreateObject<Wine>();
            wine.Identifier = Shared.ObjectIdentifier;
            wine.Name = Shared.ObjectName;
            await wine.SaveAsync();

            TastingNotes tastingNotes = cellar.CreateObject<TastingNotes>();
            tastingNotes.Identifier = Shared.ObjectIdentifier;
            tastingNotes.Name = Shared.ObjectName;
            await tastingNotes.SaveAsync();

            Rack rack = cellar.CreateObject<Rack>();
            rack.Identifier = Shared.ObjectIdentifier;
            rack.Name = Shared.ObjectName;
            await rack.SaveAsync();

            Dashboard dashboard = await cellar.GetDashboardAsync();
            Assert.IsTrue(dashboard.Wines == 1);
            Assert.IsTrue(dashboard.Cellars == 1);
            Assert.IsTrue(dashboard.TastingNotes == 1);
            Assert.IsTrue(dashboard.FileSize > 0);
        }
    }
}
