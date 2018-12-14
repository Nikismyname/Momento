namespace Momento.Tests.Tests.TestServoceTests
{
    using Momento.Services.Contracts.Test;
    using Momento.Services.Implementations.Test;
    using Momento.Tests.Contracts;
    using Momento.Tests.Seeding;
    using NUnit.Framework;

    public class TestServiceTests : BaseTestsSqliteInMemory
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
            VideoS.SeedPeshoAndGosho(this.context);
            var video = VideoS.SeedVideosToUser(context,VideoS.GoshoId);
            this.testService.Test(video.Id);
        }
    }
}
