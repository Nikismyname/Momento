namespace Momento.Tests.Tests.ListToDoTests
{
    using FluentAssertions;
    using Momento.Models.ListsToDo;
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

    public class ListToDoRestTests : BaseTestsSqliteInMemory
    {
        private IListToDoService listToDoService;

        public override void Setup()
        {
            base.Setup();
            ///It is a very simple and tested service, doesn't count as dependancy!!
            var trackableService = new TrackableService(this.context);
            this.listToDoService = new ListToDoService(this.context, trackableService);
        }

        #region Create 
        [Test]
        public void CreateThrowsIfUserNotFound()
        {
            const string nonExistantUsername = "Definitely Not Pesho";

            ///Does not matter for this test
            var createInfo = new ListToDoCreate
            {
                Description = "",
                DirectoryId = 1,
                Name = "",
            };

            Action action = () => this.listToDoService.Create(createInfo, nonExistantUsername);

            action.Should().Throw<UserNotFound>();
        }

        [Test]
        public void CreatThrowsIfDirectoryNotFound()
        {
            const int nonExistantDirId = 42;

            UserS.SeedPeshoAndGosho(this.context);

            var createInfo = new ListToDoCreate
            {
                Description = "",
                DirectoryId = nonExistantDirId,
                Name = "",
            };

            Action action = () => this.listToDoService.Create(createInfo, UserS.PeshoUsername);

            action.Should().Throw<ItemNotFound>().WithMessage("The directory for this List does not exists");
        }

        [Test]
        public void CreateThrowsIfDirectoryDoesNotBelongToUser()
        {
            UserS.SeedPeshoAndGosho(this.context);

            var createInfo = new ListToDoCreate
            {
                Description = "",
                DirectoryId = UserS.PeshoRootDirId,
                Name = "",
            };

            Action action = () => this.listToDoService.Create(createInfo, UserS.GoshoUsername);

            action.Should().Throw<AccessDenied>().WithMessage("The directory you are trying to create you List does not belong to you!");
        }

        [Test]
        public void CreateCreatesListTheAppropriateProperties()
        {
            const string description = "new and original description";
            const string name = "new and original name";

            UserS.SeedPeshoAndGosho(this.context);

            var createInfo = new ListToDoCreate
            {
                Description = description,
                Name = name,
                DirectoryId = UserS.PeshoRootDirId,
            };

            Func<ListToDo> action = () => this.listToDoService.Create(createInfo, UserS.PeshoUsername);
            var resultList = action.Invoke();

            resultList.Name.Should().Be(name);
            resultList.Description.Should().Be(description);
            resultList.Categories.Should().Be(ListToDo.InitialCategories);
            resultList.DirectoryId.Should().Be(UserS.PeshoRootDirId);
            resultList.Items.Count.Should().Be(0);
            resultList.UserId.Should().Be(UserS.PeshoId);
            resultList.Order.Should().Be(0);
        }

        [Test]
        public void CreateShouldAssignTheCorrectOrderToNewLists()
        {
            UserS.SeedPeshoAndGosho(this.context);

            var createInfo = new ListToDoCreate
            {
                Description = "",
                DirectoryId = UserS.PeshoRootDirId,
                Name = "",
            };

            Func<ListToDo> action = () => this.listToDoService.Create(createInfo, UserS.PeshoUsername);
            var list0 = action.Invoke();
            var list1 = action.Invoke();
            var list2 = action.Invoke();

            list0.Order.Should().Be(0);
            list1.Order.Should().Be(1);
            list2.Order.Should().Be(2);
        }

        #endregion

        #region GetUseModel

        [Test]
        public void GetUseModelShouldThrowIfUserNotFound()
        {
            const int nonExistantListId = 42;
            const string nonExistantUsername = "Opredeleno ne Pesho";

            UserS.SeedPeshoAndGosho(this.context);
            var list = ListTDS.SeedListToUser(this.context, UserS.PeshoUsername);

            Action action = () => this.listToDoService.GetUseModel(nonExistantListId, nonExistantUsername);
            action.Should().Throw<UserNotFound>();
        }

        [Test]
        public void GetUseModelShouldThrowIfListNotFound()
        {
            const int nonExistantListId = 42;

            UserS.SeedPeshoAndGosho(this.context);
            var list = ListTDS.SeedListToUser(this.context, UserS.PeshoUsername);

            Action action = () => this.listToDoService.GetUseModel(nonExistantListId, UserS.PeshoUsername);
            action.Should().Throw<ItemNotFound>().WithMessage("The ListToDo you are trying to get does not exist in the database!");
        }

        [Test]
        public void GetUseModelShouldThrowIfListDoesNotBelongToUser()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var list = ListTDS.SeedListToUser(this.context, UserS.GoshoUsername);

            Action action = () => this.listToDoService.GetUseModel(list.Id, UserS.PeshoUsername);
            action.Should().Throw<AccessDenied>().WithMessage("The ListToDo you are trying get does not belong to you");
        }

        [Test]
        public void GetUseModelShouldReturnTheCorrectModel()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var list = ListTDS.SeedListToUser(this.context, UserS.GoshoUsername);
            var items = ListTDS.SeedTwoItemsToList(this.context, list);

            var item1 = items.Single(x => x.Content == ListTDS.item1Content);
            var item2 = items.Single(x => x.Content == ListTDS.item2Content); ;

            var expectedResult = new ListToDoUse()
            {
                Categories = ListTDS.categories,
                Description = ListTDS.desctiption,
                DirectoryId = UserS.GoshoRootDirId,
                Id = list.Id,
                Name = ListTDS.name,
                Items = new List<ListToDoItemUse>
                {
                     new ListToDoItemUse
                     {
                          Changed = false,
                          Comment = ListTDS.item1Comment,
                          Content = ListTDS.item1Content,
                          Deleted = false,
                          Id = item1.Id,
                          Order = ListTDS.item1Order,
                          Status = ListTDS.item1Status,
                     },
                     new ListToDoItemUse
                     {
                          Changed = false,
                          Comment = ListTDS.item2Comment,
                          Content = ListTDS.item2Content,
                          Deleted = false,
                          Id = item2.Id,
                          Order = ListTDS.item2Order,
                          Status = ListTDS.item2Status,
                     },
                }
            };

            Func<ListToDoUse> action = () => this.listToDoService.GetUseModel(list.Id, UserS.GoshoUsername);
            var result = action.Invoke();

            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region Delete
        [Test]
        public void DeleteShouldThrowIfUserNotFound()
        {
            const string nonExistantUsername = "Opredeleno not Pesho";
            const int nonExistantListId = 42;

            UserS.SeedPeshoAndGosho(this.context);
            Action action = () => this.listToDoService.Delete(nonExistantListId, nonExistantUsername);

            action.Should().Throw<UserNotFound>();
        }

        [Test]
        public void DeleteShouldThrowIfListNotFound()
        {
            const int nonExistantListId = 42;

            UserS.SeedPeshoAndGosho(this.context);
            Action action = () => this.listToDoService.Delete(nonExistantListId, UserS.PeshoUsername);

            action.Should().Throw<ItemNotFound>().WithMessage("The list you are trying to delete does not exist!");
        }

        [Test]
        public void DeleteShouldThrowIfListDoesNotBelongToUser()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var list = ListTDS.SeedListToUser(this.context, UserS.PeshoUsername);

            Action action = () => this.listToDoService.Delete(list.Id, UserS.GoshoUsername);

            action.Should().Throw<AccessDenied>().WithMessage("The list you are trying to delete does not belong to you!");
        }

        [Test]
        public void DeleteDeleteListAdAllItsItems()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var list = ListTDS.SeedListToUser(this.context, UserS.PeshoUsername);
            ListTDS.SeedTwoItemsToList(this.context, list);

            ChangeTrackerOperations.DetachAll(this.context);
            Func<ListToDo> action = () => this.listToDoService.Delete(list.Id, UserS.PeshoUsername);

            var deletedList = action.Invoke();

            deletedList.IsDeleted.Should().Be(true);
            deletedList.Items.Count.Should().Be(2);
            deletedList.Items.Select(x => x.IsDeleted).Should().AllBeEquivalentTo(true);
        }
        #endregion
    }
}
