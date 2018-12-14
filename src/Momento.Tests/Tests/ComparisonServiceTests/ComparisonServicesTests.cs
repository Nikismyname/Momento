namespace Momento.Tests.Tests.ComparisonServiceTests
{
    using FluentAssertions;
    using Momento.Services.Contracts.Comparisons;
    using Momento.Services.Exceptions;
    using Momento.Services.Implementations.Comparisons;
    using Momento.Services.Mapping;
    using Momento.Services.Models.VideoModels;
    using Momento.Tests.Contracts;
    using Momento.Tests.Seeding;
    using Momento.Services.Models.ComparisonModels;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ComparisonServicesTests : BaseTestsSqliteInMemory
    {
        private IComparisonService comparisonService;

        public override void Setup()
        {
            base.Setup();
            this.comparisonService = new ComparisonService(this.context);
        }

        [Test]
        public void SaveShouldThrowIfInvalidUsername()
        {
            UserS.SeedPeshoAndGosho(context);
            var invalidUsername = "invalidUsername";
            Action action = () => this.comparisonService.Save(new ComparisonSave(), invalidUsername);
            action.Should().Throw<UserNotFound>();
        }

        [Test]
        public void SaveShouldThrowIfNonexistantComparison()
        {
            UserS.SeedPeshoAndGosho(context);
            CompS.SeedTwoCompsToUser(context, UserS.GoshoId);
            var invalidCompId = 42;
            var compSave = new ComparisonSave
            {
                Id = invalidCompId,
            };
            Action action = () => this.comparisonService.Save(compSave, UserS.GoshoUsername);
            action.Should().Throw<ItemNotFound>().WithMessage("The comparison you are trying to modify does not exist!");
        }

        [Test]
        public void SaveShouldThrowIfCompDoesNotBelongToUser()
        {
            UserS.SeedPeshoAndGosho(context);
            var comps = CompS.SeedTwoCompsToUser(context, UserS.GoshoId);
            var compSave = new ComparisonSave
            {
                Id = comps[0].Id,
            };
            Action action = () => this.comparisonService.Save(compSave, UserS.PeshoUsername);
            action.Should().Throw<AccessDenied>().WithMessage("The comparison does not belong to you!");
        }

        [Test]
        public void SaveShouldChangeCompFieldsIfTheyAreNotNull()
        {
            const string newDescription = "New Description";
            const string newName = "NewName";
            const string newTargetLanguage = "New Target Language";
            const string newSourceLanguage = "New Source Language";

            UserS.SeedPeshoAndGosho(context);
            var comps = CompS.SeedTwoCompsToUser(context, UserS.GoshoId);
            var compSave = new ComparisonSave
            {
                Id = comps[0].Id,
                Description = newDescription,
                Name = newName,
                TargetLanguage = newTargetLanguage,
                SourceLanguage = newSourceLanguage,
            };
            Action action = () => this.comparisonService.Save(compSave, UserS.GoshoUsername);
            action.Invoke();

            comps[0].Description.Should().Be(newDescription);
            comps[0].Name.Should().Be(newName);
            comps[0].TargetLanguage.Should().Be(newTargetLanguage);
            comps[0].SourceLanguage.Should().Be(newSourceLanguage);
        }

        [Test]
        public void SaveShouldNotChangeCompFieldsIfTheyAreNull()
        {
            UserS.SeedPeshoAndGosho(context);
            var comps = CompS.SeedTwoCompsToUser(context, UserS.GoshoId);

            string initialDescription = comps[0].Description;
            string initialName = comps[0].Name;
            string initialTargetLanguage = comps[0].TargetLanguage;
            string initialSourceLanguage = comps[0].SourceLanguage;

            var compSave = new ComparisonSave
            {
                Id = comps[0].Id,
                Description = null,
                Name = null,
                TargetLanguage = null,
                SourceLanguage = null,
            };
            Action action = () => this.comparisonService.Save(compSave, UserS.GoshoUsername);
            action.Invoke();

            comps[0].Description.Should().Be(initialDescription);
            comps[0].Name.Should().Be(initialName);
            comps[0].TargetLanguage.Should().Be(initialTargetLanguage);
            comps[0].SourceLanguage.Should().Be(initialSourceLanguage);
        }

        [Test]
        public void SaveShouldThrow_IfSomeOfToBeAlteredItems_HaveIdsNotInTheComparisonsItems()
        {
            UserS.SeedPeshoAndGosho(context);
            var comps = CompS.SeedTwoCompsToUser(context, UserS.GoshoId);
            var usedComp = comps[0];
            var items = CompS.SeedItemsToComp(this.context, usedComp);

            var compSave = new ComparisonSave
            {
                Id = usedComp.Id,
                Description = null,
                Name = null,
                TargetLanguage = null,
                SourceLanguage = null,
                AlteredItems = new HashSet<ComparisonItemChange>
                {
                    new ComparisonItemChange
                    {
                        Id = 1,
                        NewValue = "new test value",
                        PropertyChanged = "Comment"
                    },
                    new ComparisonItemChange
                    {
                        Id = 2,
                        NewValue = "new test value",
                        PropertyChanged = "Order"
                    },
                    new ComparisonItemChange
                    {
                        Id = 3,
                        NewValue = "new test value",
                        PropertyChanged = "Source"
                    },
                    new ComparisonItemChange
                    {
                        Id = 4,
                        NewValue = "new test value",
                        PropertyChanged = "Target"
                    }
                }
            };

            Action action = () => this.comparisonService.Save(compSave, UserS.GoshoUsername);
            action.Should().Throw<AccessDenied>().WithMessage("The comparison items you are trying to alter does not belong the comparison you are altering!");
        }

        [Test]
        public void SaveShouldAlterExistingItems_forAValidRequest()
        {
            const string item1NewSource = "itemOneNewSource";
            const string item1NewTarget = "itemOneNewTarget";
            const string item1NewComment = "itemOneNewComment";
            const int item1NewOrder = 4;

            const string item2NewSource = "itemTwoNewSource";
            const string item2NewTarget = "itemTwoNewTarget";
            const string item2NewComment = "itemTwoNewComment";
            const int item2NewOrder = 3;


            UserS.SeedPeshoAndGosho(context);
            var comps = CompS.SeedTwoCompsToUser(context, UserS.GoshoId);
            var usedComp = comps[0];
            var items = CompS.SeedItemsToComp(this.context, usedComp);

            var compSave = new ComparisonSave
            {
                Id = usedComp.Id,
                Description = null,
                Name = null,
                TargetLanguage = null,
                SourceLanguage = null,
                AlteredItems = new HashSet<ComparisonItemChange>
                {
                    new ComparisonItemChange
                    {
                        Id = 1,
                        NewValue = item1NewComment,
                        PropertyChanged = "Comment"
                    },
                    new ComparisonItemChange
                    {
                        Id = 1,
                        NewValue = item1NewOrder.ToString(),
                        PropertyChanged = "Order"
                    },
                    new ComparisonItemChange
                    {
                        Id = 1,
                        NewValue = item1NewSource,
                        PropertyChanged = "Source"
                    },
                    new ComparisonItemChange
                    {
                        Id = 1,
                        NewValue = item1NewTarget,
                        PropertyChanged = "Target"
                    },


                    new ComparisonItemChange
                    {
                        Id = 2,
                        NewValue = item2NewComment,
                        PropertyChanged = "Comment"
                    },
                    new ComparisonItemChange
                    {
                        Id = 2,
                        NewValue = item2NewOrder.ToString(),
                        PropertyChanged = "Order"
                    },
                    new ComparisonItemChange
                    {
                        Id = 2,
                        NewValue = item2NewSource,
                        PropertyChanged = "Source"
                    },
                    new ComparisonItemChange
                    {
                        Id = 2,
                        NewValue = item2NewTarget,
                        PropertyChanged = "Target"
                    },
                }
            };

            Action action = () => this.comparisonService.Save(compSave, UserS.GoshoUsername);
            action.Invoke();

            var item1 = items.SingleOrDefault(x => x.Id == 1);
            var item2 = items.SingleOrDefault(x => x.Id == 2);

            item1.Comment.Should().Be(item1NewComment);
            item1.Order.Should().Be(item1NewOrder);
            item1.Source.Should().Be(item1NewSource);
            item1.Target.Should().Be(item1NewTarget);

            item2.Comment.Should().Be(item2NewComment);
            item2.Order.Should().Be(item2NewOrder);
            item2.Source.Should().Be(item2NewSource);
            item2.Target.Should().Be(item2NewTarget);
        }

        [Test]
        public void SaveShouldSaveNewItems()
        {
            AutoMapperConfig.RegisterMappings(typeof(VideoCreate).Assembly);

            const string item1Comment = "Comment1";
            const int item1Order = 4;
            const string item1Source = "Source1";
            const string item1Target = "Target1";

            const string item2Comment = "Comment2";
            const int item2Order = 3;
            const string item2Source = "Source2";
            const string item2Target = "Target2";

            UserS.SeedPeshoAndGosho(context);
            var comps = CompS.SeedTwoCompsToUser(context, UserS.GoshoId);
            var usedComp = comps[0];

            var compSave = new ComparisonSave
            {
                Id = usedComp.Id,
                Description = null,
                Name = null,
                TargetLanguage = null,
                SourceLanguage = null,
                NewItems = new HashSet<ComparisonItemEdit>
                {
                    new ComparisonItemEdit
                    {
                        Id = 33,
                        Comment = item1Comment,
                        Order = item1Order,
                        Source = item1Source,
                        Target = item1Target,
                    },
                     new ComparisonItemEdit
                    {
                        Id = 44,
                        Comment = item2Comment,
                        Order = item2Order,
                        Source = item2Source,
                        Target = item2Target,
                    },
                }
            };

            Action action = () => this.comparisonService.Save(compSave, UserS.GoshoUsername);
            ///The mapping should set preSaved db items ids to 0; It Does
            action.Invoke();

            var newCompItems = usedComp.Items;

            var item1 = newCompItems.SingleOrDefault(x => x.Comment == item1Comment);
            item1.Should().NotBe(null);
            item1.Source.Should().Be(item1Source);
            item1.Target.Should().Be(item1Target);
            item1.Order.Should().Be(item1Order);

            var item2 = newCompItems.SingleOrDefault(x => x.Comment == item2Comment);
            item2.Should().NotBe(null);
            item2.Source.Should().Be(item2Source);
            item2.Target.Should().Be(item2Target);
            item2.Order.Should().Be(item2Order);
        }
    }
}
