namespace Momento.Tests.Tests.ComparisonServiceTests
{
    using Momento.Services.Contracts.Comparisons;
    using Momento.Services.Implementations.Comparisons;
    using Momento.Tests.Contracts;
    using Momento.Tests.Seeding;
    using NUnit.Framework;

    public class RestTests: BaseTestsSqliteInMemory
    {
        private IComparisonService comparisonService;

        public override void Setup()
        {
            base.Setup();
            this.comparisonService = new ComparisonService(this.context);
        }

        [Test]
        public void GetForEditShouldThrow()
        {

        }

        [Test]
        public void GetForEditShouldReturnTheCorrectData()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var comps = CompS.SeedTwoCompsToUser(this.context, UserS.GoshoId);
            var usedComp = comps[0];
            CompS.SeedThreeItemsToComp(this.context,usedComp);
        }
    }
}
