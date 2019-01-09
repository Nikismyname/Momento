namespace Momento.Tests.Tests.ComparisonServiceTests
{
    #region Initialization
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
    using System.Collections.Generic;
    using System.Linq;

    public class CompRestTests : BaseTestsSqliteInMemory
    {
        private IComparisonService comparisonService;

        public override void Setup()
        {
            base.Setup();
            this.comparisonService = new ComparisonService(this.context);
        }
        #endregion

        #region GetForEdit
        [Test]
        public void GetForEditShouldThrowIfUserNotFound()
        {
            const string NonExistentUserName = "clearly not pesho";
            ///Does not matter for this test
            const int NonExistentId = 42;

            UserS.SeedPeshoAndGosho(this.context);

            ChangeTrackerOperations.DetachAll(this.context);
            Func<ComparisonEdit> action = () => this.comparisonService.GetForEdit(NonExistentId, NonExistentUserName);
            action.Should().Throw<UserNotFound>();
        }

        [Test]
        public void GetForEditShouldThrowIfItemNotFound()
        {
            const int NonExistentId = 42;

            UserS.SeedPeshoAndGosho(this.context);
            CompS.SeedTwoCompsToUser(this.context, UserS.GoshoId);

            ChangeTrackerOperations.DetachAll(this.context);
            Func<ComparisonEdit> action = () => this.comparisonService.GetForEdit(NonExistentId, UserS.GoshoUsername);
            action.Should().Throw<ItemNotFound>().WithMessage("The comparison you are looking for does not exist!");
        }

        [Test]
        public void GetForEditShouldThrowIfItemDoesNotBelongToUser()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var comps = CompS.SeedTwoCompsToUser(this.context, UserS.GoshoId);
            var usedComp = comps[0];

            ChangeTrackerOperations.DetachAll(this.context);
            Func<ComparisonEdit> action = () => this.comparisonService.GetForEdit(usedComp.Id, UserS.PeshoUsername);
            action.Should().Throw<AccessDenied>().WithMessage("This comparison does not belong to you!");
        }

        [Test]
        public void GetForEditShouldReturnTheCorrectData()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var allComps = CompS.SeedTwoCompsToUser(this.context, UserS.GoshoId);
            var usedComp = allComps.Single(x => x.Id == CompS.CompItem1Id);
            CompS.SeedThreeItemsToComp(this.context, usedComp);

            var expectedResult = new ComparisonEdit
            {
                Id = CompS.CompItem1Id,
                Description = CompS.CompItem1Description,
                DirectoryId = UserS.GoshoRootDirId,
                Name = CompS.CompItem1Name,
                Order = CompS.CompItem1Order,
                SourceLanguage = CompS.CompItem1Source,
                TargetLanguage = CompS.CompItem1Target,

                Items = new HashSet<ComparisonItemEdit>
                {
                    new ComparisonItemEdit
                    {
                         Id = CompS.Item1Id,
                         Comment =CompS.Item1Comment,
                         Order = CompS.Item1Order,
                         Source =CompS.Item1Source,
                         Target =CompS.Item1Target,
                    },
                    new ComparisonItemEdit
                    {
                         Id = CompS.Item2Id,
                         Comment =CompS.Item2Comment,
                         Order = CompS.Item2Order,
                         Source =CompS.Item2Source,
                         Target =CompS.Item2Target,
                    },
                    new ComparisonItemEdit
                    {
                         Id = CompS.Item3Id,
                         Comment =CompS.Item3Comment,
                         Order = CompS.Item3Order,
                         Source =CompS.Item3Source,
                         Target =CompS.Item3Target,
                    }
                }
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Func<ComparisonEdit> action = () => this.comparisonService.GetForEdit(usedComp.Id, UserS.GoshoUsername);
            var result = action.Invoke();

            result.Should().BeEquivalentTo(expectedResult);
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
            Func<Comparison> action = () => this.comparisonService.Create(compCreate, nonExistantUsername);
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

        [Test]
        public void DeleteSonftDeletesTheGivenComparisonAndAllItsChildren()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var comps = CompS.SeedTwoCompsToUser(this.context, UserS.PeshoId);
            var usedComp = comps[0];
            var items = CompS.SeedThreeItemsToComp(this.context, usedComp);

            ChangeTrackerOperations.DetachAll(this.context);
            Func<Comparison> action = () => this.comparisonService.Delete(usedComp.Id, UserS.PeshoUsername);
            var result = action.Invoke();

            result.IsDeleted.Should().Be(true);
            ///Checking that is materialised the childres so it could delete them 
            result.Items.Count.Should().Be(3);
            result.Items.Select(x => x.IsDeleted).Should().AllBeEquivalentTo(true);
        }
        #endregion
    }
}
