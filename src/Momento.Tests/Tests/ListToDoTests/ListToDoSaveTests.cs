namespace Momento.Tests.Tests.ListToDoTests
{
    using FluentAssertions;
    using Momento.Services.Contracts.ListToDo;
    using Momento.Services.Exceptions;
    using Momento.Services.Implementations.ListToDo;
    using Momento.Services.Implementations.Shared;
    using Momento.Services.Models.ListToDoModels;
    using Momento.Tests.Contracts;
    using Momento.Tests.Seeding;
    using Momento.Tests.Utilities;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ListToDoSaveTests : BaseTestsSqliteInMemory
    {
        private IListToDoService listToDoService;

        public override void Setup()
        {
            base.Setup();
            ///It is a very simple and tested service, doesn't count as dependancy!!
            var trackableService = new TrackableService(this.context);
            this.listToDoService = new ListToDoService(this.context, trackableService);
        }

        #region Verification
        [Test]
        public void SaveShouldThrowForNonexistantUser()
        {
            const string nonExistantUsername = "DefinetlyNotPesho";

            UserS.SeedPeshoAndGosho(this.context);
            var dbList = ListTDS.SeedListToUser(this.context, UserS.PeshoUsername);

            var saveData = new ListToDoUse
            {
                Id = dbList.Id,
                Categories = "",
                Description = "",
                ///NotUsedInThis
                DirectoryId = 1,
                Items = new List<ListToDoItemUse>(),
                Name = "",
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.listToDoService.Save(saveData, nonExistantUsername);

            action.Should().Throw<UserNotFound>();
        }

        [Test]
        public void SaveShouldThrowForNonexistantList()
        {
            const int nonExistantListId = 42;

            UserS.SeedPeshoAndGosho(this.context);
            var dbList = ListTDS.SeedListToUser(this.context, UserS.PeshoUsername);

            var saveData = new ListToDoUse
            {
                Id = nonExistantListId,
                Categories = "",
                Description = "",
                ///NotUsedInThis
                DirectoryId = 1,
                Items = new List<ListToDoItemUse>(),
                Name = "",
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.listToDoService.Save(saveData, UserS.PeshoUsername);

            action.Should().Throw<ItemNotFound>().WithMessage("The list you are trying to modify does not exist!");
        }

        [Test]
        public void SaveShouldThrowIfCurrentUserIsNotTheOwnerOfTheList()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var dbList = ListTDS.SeedListToUser(this.context, UserS.PeshoUsername);

            var saveData = new ListToDoUse
            {
                Id = dbList.Id,
                Categories = "",
                Description = "",
                ///NotUsedInThis
                DirectoryId = 1,
                Items = new List<ListToDoItemUse>(),
                Name = "",
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.listToDoService.Save(saveData, UserS.GoshoUsername);

            action.Should().Throw<AccessDenied>().WithMessage("The list you are trying to modify does not beling to you!");
        }

        [Test]
        public void SaveShouldThrowIfIfTheUserHasSomehowDeletedTheUnassignedCategory()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var dbList = ListTDS.SeedListToUser(this.context, UserS.PeshoUsername);

            var saveData = new ListToDoUse
            {
                Id = dbList.Id,
                Categories = "cat1;cat2;cat3;cat4;",
                Description = "",
                ///NotUsedInThis
                DirectoryId = 1,
                Items = new List<ListToDoItemUse>(),
                Name = "",
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.listToDoService.Save(saveData, UserS.PeshoUsername);

            action.Should().Throw<Exception>().WithMessage("The user somehow deleted the Unassigned tab, this is not Right!");
        }
        #endregion

        #region SaveUpdatesListProperties_categories_desction_name
        [Test]
        public void SaveUpdatesListProperties()
        {
            const string newName = "newNameOriginalIKnow";
            const string newDescription = "newDescription";
            const string newCategories = "cat1;cat2;cat3;cat4;Unassigned";

            UserS.SeedPeshoAndGosho(this.context);
            var dbList = ListTDS.SeedListToUser(this.context, UserS.PeshoUsername);

            var saveData = new ListToDoUse
            {
                Id = dbList.Id,
                Categories = newCategories,
                Description = newDescription,
                ///NotUsedInThis
                DirectoryId = 1,
                Items = new List<ListToDoItemUse>(),
                Name = newName,
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.listToDoService.Save(saveData, UserS.PeshoUsername);
            action.Invoke();

            ///Reataching the list
            dbList = context.ListsTodo.Single(x => x.Id == dbList.Id);

            dbList.Description.Should().Be(newDescription);
            dbList.Name.Should().Be(newName);
            dbList.Categories.Should().Be(newCategories);
        }
        #endregion

        #region SaveUpdatesExistionItems
        [Test]
        public void SaveThrowsIfIdOfItemToBeUpdatedDoesNotBelongToTheGivenList()
        {
            const int listItemIdThatDoesNotBelongToCurrentList = 42;

            UserS.SeedPeshoAndGosho(this.context);
            var dbList = ListTDS.SeedListToUser(this.context, UserS.PeshoUsername);
            var items = ListTDS.SeedTwoItemsToList(this.context, dbList);

            var item1 = items[0];
            var item2 = items[1];

            var saveData = new ListToDoUse
            {
                Id = dbList.Id,
                Categories = "Unassigned",
                Description = "",
                ///NotUsedInThis
                DirectoryId = 1,
                Items = new List<ListToDoItemUse> {
                    new ListToDoItemUse
                    {
                        ///We are correct here
                         Id = item1.Id,
                         Changed = true,
                         Deleted = false,

                         Comment = "",
                         Content = "",
                         Order = 1,
                         Status = "",
                    },

                    new ListToDoItemUse
                    {
                         ///Sending an id that does not belong to the list
                         Id = listItemIdThatDoesNotBelongToCurrentList,
                         Changed = true,
                         Deleted = false,

                         Comment = "",
                         Content = "",
                         Order = 1,
                         Status = "",
                    },
                },
                Name = "",
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.listToDoService.Save(saveData, UserS.PeshoUsername);

            action.Should().Throw<AccessDenied>().WithMessage("The items you are trying to modify do not belong to the current list!");
            ///Reataching the list
        }
        [Test]
        public void SaveUpdatesExistingItems()
        {
            const string item1NewComment = "itemOneNewComment";
            const string item1NewContent = "itemOneNewContent";
            const string item1NewStatus = "itemOneStatus";
            const int item1NewOrder = 1;

            const string item2NewComment = "itemTwoNewComment";
            const string item2NewContent = "itemTwoNewContent";
            const string item2NewStatus = "itemTwoNewStatus";
            const int item2NewOrder = 0;

            UserS.SeedPeshoAndGosho(this.context);
            var dbList = ListTDS.SeedListToUser(this.context, UserS.PeshoUsername);
            var items = ListTDS.SeedTwoItemsToList(this.context, dbList);

            var item1 = items[0];
            var item2 = items[1];

            var saveData = new ListToDoUse
            {
                Id = dbList.Id,
                Categories = "Unassigned",
                Description = "",
                ///NotUsedInThis
                DirectoryId = 1,
                Items = new List<ListToDoItemUse> {
                    new ListToDoItemUse
                    {
                         Id = item1.Id,
                         Changed = true,
                         Deleted = false,

                         Comment = item1NewComment,
                         Content = item1NewContent,
                         Order = item1NewOrder,
                         Status = item1NewStatus,
                    },

                    new ListToDoItemUse
                    {
                         Id = item2.Id,
                         Changed = true,
                         Deleted = false,

                         Comment = item2NewComment,
                         Content = item2NewContent,
                         Order = item2NewOrder,
                         Status = item2NewStatus,
                    },
                },
                Name = "",
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.listToDoService.Save(saveData, UserS.PeshoUsername);
            action.Invoke();

            ///Reataching the list
            item1 = context.ListToDoItems.Single(x => x.Id == item1.Id);
            item2 = context.ListToDoItems.Single(x => x.Id == item2.Id);

            item1.Comment.Should().Be(item1NewComment);
            item1.Content.Should().Be(item1NewContent);
            item1.Status.Should().Be(item1NewStatus);
            item1.Order.Should().Be(item1NewOrder);

            item2.Comment.Should().Be(item2NewComment);
            item2.Content.Should().Be(item2NewContent);
            item2.Status.Should().Be(item2NewStatus);
            item2.Order.Should().Be(item2NewOrder);
        }
        #endregion

        #region SavePersistsNewItems
        [Test]
        public void SavePersistsNewItems()
        {
            const string newItem1Comment = "itemOneNewComment1";
            const string newItem1Content = "itemOneNewContent1";
            const string newItem1Status = "itemOneStatus1";
            const int newItem1Order = 5;

            const string newItem2Comment = "itemTwoNewComment1";
            const string newItem2Content = "itemTwoNewContent1";
            const string newItem2Status = "itemTwoNewStatus1";
            const int newItem2Order = 6;

            UserS.SeedPeshoAndGosho(this.context);
            var dbList = ListTDS.SeedListToUser(this.context, UserS.PeshoUsername);

            var saveData = new ListToDoUse
            {
                Id = dbList.Id,
                Categories = "Unassigned",
                Description = "",
                ///NotUsedInThis
                DirectoryId = 1,
                Items = new List<ListToDoItemUse> {
                    new ListToDoItemUse
                    {
                         ///Id of 0 means new item
                         Id = 0,
                         Changed = true,
                         Deleted = false,

                         Comment = newItem1Comment,
                         Content = newItem1Content,
                         Order = newItem1Order,
                         Status = newItem1Status,
                    },

                    new ListToDoItemUse
                    {
                         Id = 0,
                         Changed = true,
                         Deleted = false,

                         Comment = newItem2Comment,
                         Content = newItem2Content,
                         Order = newItem2Order,
                         Status = newItem2Status,
                    },
                },
                Name = "",
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.listToDoService.Save(saveData, UserS.PeshoUsername);
            action.Invoke();

            ///Those tests garantee there are only two items with the right properties we passed them
            this.context.ListToDoItems.Count().Should().Be(2);///The two we just meant to place in
            var item1 = this.context.ListToDoItems.Single(x => x.Comment == newItem1Comment);
            var item2 = this.context.ListToDoItems.Single(x => x.Comment == newItem2Comment);

            item1.Comment.Should().Be(newItem1Comment);
            item1.Content.Should().Be(newItem1Content);
            item1.Status.Should().Be(newItem1Status);
            item1.Order.Should().Be(newItem1Order);

            item2.Comment.Should().Be(newItem2Comment);
            item2.Content.Should().Be(newItem2Content);
            item2.Status.Should().Be(newItem2Status);
            item2.Order.Should().Be(newItem2Order);
        }
        #endregion

        #region SaveDeletesRemovedItems
        [Test]
        public void SaveDeletesRemovedItems()
        {
            const string item1NewComment = "itemOneNewComment";
            const string item1NewContent = "itemOneNewContent";
            const string item1NewStatus = "itemOneStatus";
            const int item1NewOrder = 1;

            UserS.SeedPeshoAndGosho(this.context);
            var dbList = ListTDS.SeedListToUser(this.context, UserS.PeshoUsername);
            var items = ListTDS.SeedTwoItemsToList(this.context, dbList);

            var item1 = items[0];
            var item2 = items[1];

            ///We have two items in the db, but we do not send page item  
            ///with the id of the second one so it should be deleted
            var saveData = new ListToDoUse
            {
                Id = dbList.Id,
                Categories = "Unassigned",
                Description = "",
                DirectoryId = 1,
                Items = new List<ListToDoItemUse> {
                    new ListToDoItemUse
                    {
                         Id = item1.Id,
                         Changed = true,
                         Deleted = false,

                         Comment = item1NewComment,
                         Content = item1NewContent,
                         Order = item1NewOrder,
                         Status = item1NewStatus,
                    },
                },
                Name = "",
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.listToDoService.Save(saveData, UserS.PeshoUsername);
            action.Invoke();

            ///Reataching the list
            item1 = context.ListToDoItems.SingleOrDefault(x => x.Id == item1.Id);
            item2 = context.ListToDoItems.SingleOrDefault(x => x.Id == item2.Id);

            item1.Should().NotBeNull();
            item2.Should().BeNull();///this is beacause there is query selector that filters deleted items
        }
        #endregion
    }
}
