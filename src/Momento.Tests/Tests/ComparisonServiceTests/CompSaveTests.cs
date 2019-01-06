namespace Momento.Tests.Tests.ComparisonServiceTests
{
    using FluentAssertions;
    using Momento.Services.Contracts.Comparisons;
    using Momento.Services.Exceptions;
    using Momento.Services.Implementations.Comparisons;
    using Momento.Tests.Contracts;
    using Momento.Tests.Seeding;
    using Momento.Services.Models.ComparisonModels;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Momento.Tests.Utilities;
    using Microsoft.EntityFrameworkCore;

    public class CompSaveTests : BaseTestsSqliteInMemory
    {
        private IComparisonService comparisonService;

        public override void Setup()
        {
            base.Setup();
            this.comparisonService = new ComparisonService(this.context);
        }

        [Test]///Checked
        public void SaveShouldThrowIfInvalidUsername()
        {
            UserS.SeedPeshoAndGosho(context);
            var invalidUsername = "invalidUsername";

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.comparisonService.Save(new ComparisonSave(), invalidUsername);

            action.Should().Throw<UserNotFound>();
        }

        [Test]///Checked
        public void SaveShouldThrowIfNonexistantComparison()
        {
            UserS.SeedPeshoAndGosho(context);
            CompS.SeedTwoCompsToUser(context, UserS.GoshoId);
            var invalidCompId = 42;
            var compSave = new ComparisonSave
            {
                Id = invalidCompId,
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.comparisonService.Save(compSave, UserS.GoshoUsername);

            action.Should().Throw<ItemNotFound>().WithMessage("The comparison you are trying to modify does not exist!");
        }

        [Test]///Checked
        public void SaveShouldThrowIfCompDoesNotBelongToUser()
        {
            UserS.SeedPeshoAndGosho(context);
            var comps = CompS.SeedTwoCompsToUser(context, UserS.GoshoId);
            var compSave = new ComparisonSave
            {
                Id = comps[0].Id,
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.comparisonService.Save(compSave, UserS.PeshoUsername);

            action.Should().Throw<AccessDenied>().WithMessage("The comparison does not belong to you!");
        }

        [Test]///Checked
        public void SaveShouldChangeCompFieldsIfTheyAreNotNull()
        {
            const string newDescription = "New Description";
            const string newName = "NewName";
            const string newTargetLanguage = "New Target Language";
            const string newSourceLanguage = "New Source Language";

            UserS.SeedPeshoAndGosho(context);
            var comps = CompS.SeedTwoCompsToUser(context, UserS.GoshoId);
            var usedComp = comps[0];
            var compSave = new ComparisonSave
            {
                Id = usedComp.Id,
                Description = newDescription,
                Name = newName,
                TargetLanguage = newTargetLanguage,
                SourceLanguage = newSourceLanguage,
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.comparisonService.Save(compSave, UserS.GoshoUsername);
            action.Invoke();

            ///reataching the entity we want to monitor
            usedComp = context.Comparisons.SingleOrDefault(x=>x.Id == usedComp.Id);

            usedComp.Description.Should().Be(newDescription);
            usedComp.Name.Should().Be(newName);
            usedComp.TargetLanguage.Should().Be(newTargetLanguage);
            usedComp.SourceLanguage.Should().Be(newSourceLanguage);
        }

        [Test]///Checked
        public void SaveShouldNotChangeCompFieldsIfTheyAreNull()
        {
            UserS.SeedPeshoAndGosho(context);
            var comps = CompS.SeedTwoCompsToUser(context, UserS.GoshoId);
            var usedComp = comps[0];

            string initialDescription = usedComp.Description;
            string initialName = usedComp.Name;
            string initialTargetLanguage = usedComp.TargetLanguage;
            string initialSourceLanguage = usedComp.SourceLanguage;

            var compSave = new ComparisonSave
            {
                Id = usedComp.Id,
                Description = null,
                Name = null,
                TargetLanguage = null,
                SourceLanguage = null,
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.comparisonService.Save(compSave, UserS.GoshoUsername);
            action.Invoke();

            usedComp = context.Comparisons.SingleOrDefault(x => x.Id == usedComp.Id);

            usedComp.Description.Should().Be(initialDescription);
            usedComp.Name.Should().Be(initialName);
            usedComp.TargetLanguage.Should().Be(initialTargetLanguage);
            usedComp.SourceLanguage.Should().Be(initialSourceLanguage);
        }

        [Test]///Checked 
        public void SaveShouldThrow_IfSomeOfToBeAlteredItems_HaveIdsNotInTheComparisonsItems()
        {
            UserS.SeedPeshoAndGosho(context);
            var comps = CompS.SeedTwoCompsToUser(context, UserS.GoshoId);
            var usedComp = comps[0];
            var items = CompS.SeedThreeItemsToComp(this.context, usedComp);

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

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.comparisonService.Save(compSave, UserS.GoshoUsername);

            action.Should().Throw<AccessDenied>().WithMessage("The comparison items you are trying to alter does not belong the comparison you are altering!");
        }

        [Test]///Checked 
        public void SaveShouldAlterExistingItems_ForAValidRequest()
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
            var items = CompS.SeedThreeItemsToComp(this.context, usedComp);

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
                        Id = CompS.Item1Id,
                        NewValue = item1NewComment,
                        PropertyChanged = "Comment"
                    },
                    new ComparisonItemChange
                    {
                        Id = CompS.Item1Id,
                        NewValue = item1NewOrder.ToString(),
                        PropertyChanged = "Order"
                    },
                    new ComparisonItemChange
                    {
                        Id = CompS.Item1Id,
                        NewValue = item1NewSource,
                        PropertyChanged = "Source"
                    },
                    new ComparisonItemChange
                    {
                        Id = CompS.Item1Id,
                        NewValue = item1NewTarget,
                        PropertyChanged = "Target"
                    },


                    new ComparisonItemChange
                    {
                        Id = CompS.Item2Id,
                        NewValue = item2NewComment,
                        PropertyChanged = "Comment"
                    },
                    new ComparisonItemChange
                    {
                        Id = CompS.Item2Id,
                        NewValue = item2NewOrder.ToString(),
                        PropertyChanged = "Order"
                    },
                    new ComparisonItemChange
                    {
                        Id = CompS.Item2Id,
                        NewValue = item2NewSource,
                        PropertyChanged = "Source"
                    },
                    new ComparisonItemChange
                    {
                        Id = CompS.Item2Id,
                        NewValue = item2NewTarget,
                        PropertyChanged = "Target"
                    },
                }
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.comparisonService.Save(compSave, UserS.GoshoUsername);
            action.Invoke();

            var item1 = context.ComparisonItems.SingleOrDefault(x => x.Id == CompS.Item1Id);
            var item2 = context.ComparisonItems.SingleOrDefault(x => x.Id == CompS.Item2Id);

            item1.Comment.Should().Be(item1NewComment);
            item1.Order.Should().Be(item1NewOrder);
            item1.Source.Should().Be(item1NewSource);
            item1.Target.Should().Be(item1NewTarget);

            item2.Comment.Should().Be(item2NewComment);
            item2.Order.Should().Be(item2NewOrder);
            item2.Source.Should().Be(item2NewSource);
            item2.Target.Should().Be(item2NewTarget);
        }

        [Test]///Checked 
        public void SaveShouldSaveNewItems()
        {
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

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.comparisonService.Save(compSave, UserS.GoshoUsername);
            ///The mapping should set preSaved db items ids to 0; It Does
            action.Invoke();

            ///The items are actually already tracked from the execution 
            ///of the action but I am including them for consistancy
            usedComp = context.Comparisons
                .Include(x=>x.Items)
                .SingleOrDefault(x => x.Id == usedComp.Id);

            var newCompItems = usedComp.Items;

            ///TODO: find better way to identify the items, maybe seed them with id
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
