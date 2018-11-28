namespace Momento.Tests.Tests.TestServoceTests
{
    using Momento.Services.Contracts.Test;
    using Momento.Services.Implementations.Test;
    using Momento.Tests.Contracts;
    using Momento.Tests.Seeding;
    using NUnit.Framework;

    public class TestServiceTests : BaseTestsSqlLiteInMemory
    {
        private ITestService testService;

        public override void Setup()
        {
            base.Setup();
            this.testService = new TestService(this.context);
        }

        [Test]
        public void Test()
        {
            Seeder.SeedPeshoAndGosho(this.context);
            var video = Seeder.SeedVideosToUser(context,Seeder.GoshoId);
            this.testService.Test(video.Id);
        }
    }
}
