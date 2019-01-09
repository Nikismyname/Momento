using FluentAssertions;
using Momento.Services.Contracts.Directory;
using Momento.Services.Exceptions;
using Momento.Services.Implementations.Directory;
using Momento.Tests.Contracts;
using Momento.Tests.Seeding;
using NUnit.Framework;
using System;
using System.Linq;

namespace Momento.Tests.Tests.ReorderServiceTests
{
    public class ReorderServiceTests : BaseTestsSqliteInMemory
    {
        private IReorderingService reorderingService;

        public override void Setup()
        {
            base.Setup();
            this.reorderingService = new ReorderingService(this.context);
        }

        #region Validation
        [Test]
        public void ReorderThrowsIfUserNotFound()
        {
            const string NonExistantUsername = "Have you seen pesho";
            ///Only The username matters here
            Action action = () => this.reorderingService.Reorder(
                "", 42, new int[0][], NonExistantUsername);

            action.Should().Throw<UserNotFound>();
        }

        [Test]
        public void ReorderThrowsIfDirectoryNotFound()
        {
            const int NonExistantDirId = 42;

            UserS.SeedPeshoAndGosho(this.context);

            Action action = () => this.reorderingService.Reorder(
                "", NonExistantDirId, new int[0][], UserS.PeshoUsername);

            action.Should().Throw<ItemNotFound>().WithMessage("the directorie in witch you want to reorder items does not exist!");
        }

        [Test]
        public void ReorderThrowsIfDirectoryDoesNotBelongToUser()
        {
            UserS.SeedPeshoAndGosho(this.context);

            Action action = () => this.reorderingService.Reorder(
                "", UserS.GoshoRootDirId, new int[0][], UserS.PeshoUsername);

            action.Should().Throw<AccessDenied>().WithMessage("The directory you are trying to reorder things in does not belong to you!");
        }
        #endregion

        #region Functionality 
        [Test]///TODO: Works for comparisons, do the rest
        public void ReorderReordersItemsWithCorrectInput()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var comps = CompS.SeedTwoCompsToUser(this.context, UserS.PeshoId);
            var comp1 = comps.Single(x => x.Order == 0);
            var comp2 = comps.Single(x => x.Order == 1);

            var orderInfo = new int[2][];
            orderInfo[0] = new int[] {comp1.Id, 1};
            orderInfo[1] = new int[] {comp2.Id, 0};

            Action action = () => this.reorderingService.Reorder(
                ReorderingService.ComparisonType, UserS.PeshoRootDirId, orderInfo, UserS.PeshoUsername);

            action.Invoke();

            comp1.Order.Should().Be(1);
            comp2.Order.Should().Be(0);
        }
        #endregion
    }
}
