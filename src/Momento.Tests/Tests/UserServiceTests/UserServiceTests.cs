namespace Momento.Tests.Tests.UserServiceTests
{
    using NUnit.Framework;
    using FluentAssertions;
    using Momento.Services.Contracts.Other;
    using Momento.Services.Implementations.Other;
    using System;
    using Momento.Services.Exceptions;
    using Momento.Tests.Contracts;
    using Momento.Tests.Seeding;

    ///Do not use Equals, use be or equal

    public class UserSeviceTests : BaseTestsSqliteInMemory
    {
        private IUserService userService;

        public override void Setup()
        {
            base.Setup();
            base.Setup();
            this.userService = new UserService(this.context);
            UserS.SeedPeshoAndGosho(this.context);
        }

        [Test]
        public void GetUserIdThrowsIfUserNotFound()
        {
            var username = "NonExistantUsername";
            //var username = "GoshoGoshev";
            Action action = () => this.userService.GetUserId(username);
            action.Should().Throw<UserNotFound>();
        }

        [Test]
        public void GetUserIdShouldReturnTheRightIdIfUserExists()
        {
            var username = UserS.GoshoUsername;
            var correctId = UserS.GoshoId;
            var id = userService.GetUserId(username);
            id.Should().Be(correctId);
        }

        [Test]
        public void ByUsernameShouldThrowIfUserIsNotFound()
        {
            var username = "NonExistantUsername";
            //var username = "GoshoGoshev";

            Action action = () => userService.ByUsername(username);
            action.Should().Throw<UserNotFound>();
        }

        [Test]
        public void ByUsernameShouldReturnTheRightUser()
        {
            var username = UserS.PeshoUsername;
            var correctId = UserS.PeshoId;
            var user = userService.ByUsername(username);
            user.Id.Should().Be(correctId);
        }
    }
}
