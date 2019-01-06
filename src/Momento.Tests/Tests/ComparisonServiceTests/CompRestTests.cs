namespace Momento.Tests.Tests.ComparisonServiceTests
{
    using FluentAssertions;
    using Momento.Models.Comparisons;
    using Momento.Services.Contracts.Comparisons;
    using Momento.Services.Exceptions;
    using Momento.Services.Implementations.Comparisons;
    using Momento.Services.Models.ComparisonModels;
    using Momento.Tests.Contracts;
    using Momento.Tests.Seeding;
    using Momento.Tests.Utilities;
    using NUnit.Framework;
    using System;

    public class CompRestTests : BaseTestsSqliteInMemory
    {
        private IComparisonService comparisonService;

        public override void Setup()
        {
            base.Setup();
            this.comparisonService = new ComparisonService(this.context);
        }

        #region GetForEdit
        [Test]
        public void GetForEditShouldThrow()
        {

        }

        [Test]
        public void GetForEditShouldReturnTheCorrectData()
        {
            //UserS.SeedPeshoAndGosho(this.context);
            //var comps = CompS.SeedTwoCompsToUser(this.context, UserS.GoshoId);
            //var usedComp = comps[0];
            //CompS.SeedThreeItemsToComp(this.context, usedComp);
        }
        #endregion

        #region Create
        [Test]
        public void CreateShouldThrowIfUserNotFound()
        {
            const string nonExistantUsername = "Definetly not Pesho";

            UserS.SeedPeshoAndGosho(this.context);

            var compCreate = new ComparisonCreate
            {
                Description = "",
                Name = "",
                ParentDirId = 12,
                SourceLanguage = "",
                TargetLanguage = "",
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Func<Comparison> action = () => this.comparisonService.Create(compCreate,nonExistantUsername);
            action.Should().Throw<UserNotFound>();
        }

        [Test]
        public void CreateShouldThrowIfDirectoryNotFound()
        {
            const int nonExistantDirId = 42;

            UserS.SeedPeshoAndGosho(this.context);

            var compCreate = new ComparisonCreate
            {
                Description = "",
                Name = "",
                ParentDirId = nonExistantDirId,
                SourceLanguage = "",
                TargetLanguage = "",
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Func<Comparison> action = () => this.comparisonService.Create(compCreate, UserS.PeshoUsername);
            action.Should().Throw<ItemNotFound>().WithMessage("The parent directory for the comparison you are trying to create does not exist!");
        }

        [Test]
        public void CreateShouldThrowIfDirectoryDoesNotBelonToTheUser()
        {
            UserS.SeedPeshoAndGosho(this.context);

            var compCreate = new ComparisonCreate
            {
                Description = "",
                Name = "",
                ParentDirId = UserS.GoshoRootDirId,
                SourceLanguage = "",
                TargetLanguage = "",
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Func<Comparison> action = () => this.comparisonService.Create(compCreate, UserS.PeshoUsername);
            action.Should().Throw<AccessDenied>().WithMessage("The directory you are trying to put the comparison in does not belong to you!");
        }

        [Test]
        public void CreateShouldPersistANewCompWithTheRightProperties()
        {
            const string initDescription = "superb init description";
            const string initName = "superb init Name";
            const string initSourceLanguage = "superb init Source language";
            const string initTargetLanguage = "superb init Target language";

            UserS.SeedPeshoAndGosho(this.context);

            var compCreate = new ComparisonCreate
            {
                Description = initDescription,
                Name = initName,
                ParentDirId = UserS.PeshoRootDirId,
                SourceLanguage = initSourceLanguage,
                TargetLanguage = initTargetLanguage,
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Func<Comparison> action = () => this.comparisonService.Create(compCreate, UserS.PeshoUsername);
            var result = action.Invoke();

            result.Description.Should().Be(initDescription);
            result.Name.Should().Be(initName);
            result.SourceLanguage.Should().Be(initSourceLanguage);
            result.TargetLanguage.Should().Be(initTargetLanguage);

            result.DirectoryId.Should().Be(UserS.PeshoRootDirId);
            result.UserId.Should().Be(UserS.PeshoId);
        }
        #endregion

        #region Delete
        [Test]
        public void DeleteShouldThrowIfUserNotFound()
        {
            const int nonExistantCompId = 42;
            const string nonExistantUsername = "Definetly not Pesho";

            UserS.SeedPeshoAndGosho(this.context);

            ChangeTrackerOperations.DetachAll(this.context);
            Func<Comparison> action = () => this.comparisonService.Delete(nonExistantCompId, nonExistantUsername);
            action.Should().Throw<UserNotFound>();
        }

        [Test]
        public void DeleteShouldThrowIfComparisonNotFound()
        {
            const int nonExistantCompId = 42;

            UserS.SeedPeshoAndGosho(this.context);
            var comps = CompS.SeedTwoCompsToUser(this.context, UserS.PeshoId);

            ChangeTrackerOperations.DetachAll(this.context);
            Func<Comparison> action = () => this.comparisonService.Delete(nonExistantCompId, UserS.PeshoUsername);
            action.Should().Throw<ItemNotFound>().WithMessage("The comparison you are trying to delete does not exist!");
        }

        [Test]
        public void DeleteShouldThrowIfComparisonDoesNotBelongToUser()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var comps = CompS.SeedTwoCompsToUser(this.context, UserS.PeshoId);
            var usedComp = comps[0];

            ChangeTrackerOperations.DetachAll(this.context);
            Func<Comparison> action = () => this.comparisonService.Delete(usedComp.Id, UserS.GoshoUsername);
            action.Should().Throw<AccessDenied>().WithMessage("The comparison you are trying to delete does not belong to you!");
        }
        #endregion
    }
}
