namespace Momento.Tests.Tests.DirectoryServiceTests
{

    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Momento.Models.Comparisons;
    using Momento.Models.Contracts;
    using Momento.Models.Directories;
    using Momento.Services.Contracts.Directory;
    using Momento.Services.Exceptions;
    using Momento.Services.Implementations.Directory;
    using Momento.Services.Models.ComparisonModels;
    using Momento.Services.Models.DirectoryModels;
    using Momento.Services.Models.ListToDoModels;
    using Momento.Services.Models.NoteModels;
    using Momento.Services.Models.VideoModels;
    using Momento.Tests.Contracts;
    using Momento.Tests.Seeding;
    using Momento.Tests.Utilities;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DirectoryServiceTests : BaseTestsSqliteInMemory
    {
        private IDirectoryService directoryService;

        public override void Setup()
        {
            base.Setup();
            this.directoryService = new DirectoryService(this.context);
        }

        #region Create
        [Test]
        public void CreateThrowsIfParentUserIsNotFound()
        {
            var NonExistantDirId = 42;
            var NonExistantUsername = "who coud think it is pesho";

            ChangeTrackerOperations.DetachAll(this.context);
            Func<int> action = () => this.directoryService.Create(NonExistantDirId, "not important now", NonExistantUsername);
            action.Should().Throw<UserNotFound>();
        }

        [Test]
        public void CreateThrowsIfParentDirectoryIsNotFound()
        {
            var NonExistantDirId = 42;

            UserS.SeedPeshoAndGosho(this.context);

            ChangeTrackerOperations.DetachAll(this.context);
            Func<int> action = () => this.directoryService.Create(NonExistantDirId, "not important now", UserS.PeshoUsername);
            action.Should().Throw<ItemNotFound>().WithMessage("The parent directory of the directory you are trying to create does not exist!");
        }

        [Test]
        public void CreateThrowsIfParentDirectoryDoesNotBelongToUser()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var dir = this.context.Directories.Single(x => x.Id == UserS.PeshoRootDirId);

            ChangeTrackerOperations.DetachAll(this.context);
            Func<int> action = () => this.directoryService.Create(dir.Id, "not important now", UserS.GoshoUsername);
            action.Should().Throw<AccessDenied>().WithMessage("The parent directory does not belong to you!");
        }

        [Test]
        [TestCase("Root")]
        [TestCase("ROOT")]
        [TestCase("RoOt")]
        [TestCase("root")]
        [TestCase("    ")]
        [TestCase("")]
        public void CreateThrowsIfNewDirectoryNameIsNotProper(string value)
        {
            UserS.SeedPeshoAndGosho(this.context);
            var dir = this.context.Directories.Single(x => x.Id == UserS.PeshoRootDirId);

            Func<int> action = () => this.directoryService.Create(dir.Id, value, UserS.PeshoUsername);
            action.Should().Throw<BadRequestError>().WithMessage("The directory name is not valid!");
        }

        [Test]
        public void CreateShouldCreateADirectoryWithTheRightProperties()
        {
            const string DirName = "inspired dir name";

            UserS.SeedPeshoAndGosho(this.context);
            var dir = this.context.Directories.Single(x => x.Id == UserS.PeshoRootDirId);

            ChangeTrackerOperations.DetachAll(this.context);
            Func<int> action = () => this.directoryService.Create(dir.Id, DirName, UserS.PeshoUsername);
            var newDirId = action.Invoke();

            var newDir = this.context.Directories.Single(x => x.Id == newDirId);

            newDir.Name.Should().Be(DirName);
            newDir.ParentDirectoryId.Should().Be(dir.Id);
            newDir.UserId.Should().Be(UserS.PeshoId);
        }

        [Test]
        public void CreateShouldSetTheRightOrderToNewlyCreatedDirectories()
        {
            const string DirName = "inspired dir name";

            UserS.SeedPeshoAndGosho(this.context);
            var dir = this.context.Directories.Single(x => x.Id == UserS.PeshoRootDirId);

            ChangeTrackerOperations.DetachAll(this.context);
            Func<int> action = () => this.directoryService.Create(dir.Id, DirName, UserS.PeshoUsername);

            var order0Id = action.Invoke();
            action.Invoke();
            action.Invoke();
            action.Invoke();
            var order4Id = action.Invoke();

            this.context.Directories.Single(x => x.Id == order0Id).Order.Should().Be(0);
            this.context.Directories.Single(x => x.Id == order4Id).Order.Should().Be(4);
        }

        #endregion

        #region GetIndexSingle

        [Test]
        public void GetIndexDHouldThrowIfUserNotFound()
        {
            const int NonExistantDirId = 42;
            const string NonExistantUsername = "who is pesho?";

            Func<DirectoryIndexSingle> action = () => this.directoryService.GetIndexSingle(NonExistantDirId, NonExistantUsername);
            action.Should().Throw<UserNotFound>();
        }

        #region WhenRequestingTheRoot
        [Test]
        [TestCase(null)]
        [TestCase(0)]
        public void GetIndexDHouldThrowIfRequestIsForRootDirAndSuchDoesNotExist(int? dirId)
        {
            UserS.SeedPeshoAndGosho(this.context);

            Func<DirectoryIndexSingle> action = () => this.directoryService.GetIndexSingle(dirId, UserS.PeshoUsername);
            action.Should().Throw<InternalServerError>().WithMessage("Could not find root dir for user " + UserS.PeshoUsername);
        }

        [Test]
        [TestCase(null)]
        [TestCase(0)]
        ///TODO: add things to the directory for better test 
        public void GetIndexDHouldReturnTheRootDirOfTheUserInNullOrZeroIsPassed(int? dirId)
        {
            UserS.SeedPeshoAndGosho(this.context,true);
            var peshoRealRoot = this.context.Directories.Single(x => x.Name == "Root" && x.UserId == UserS.PeshoId);

            var expectedResult = new DirectoryIndexSingle
            {
                Id = peshoRealRoot.Id,
                Name = peshoRealRoot.Name,
                ParentDirectoryId = null,
                Comparisons = new HashSet<ComparisonIndex>(),
                ListsToDo = new HashSet<ListToDoIndex>(),
                Notes = new HashSet<NoteIndex>(),
                Subdirectories = new HashSet<DirectoryNavigation>(),
                Videos = new HashSet<VideoIndex>(),
            };

            Func<DirectoryIndexSingle> action = () => this.directoryService.GetIndexSingle(dirId, UserS.PeshoUsername);
            var result = action.Invoke();
            result.Should().BeEquivalentTo(expectedResult);
        }

        #endregion

        #region When Requesting a particular Dir
        [Test]
        public void GetIndexDHouldThrowIfDirectoryIdLookedForDoesNotExist()
        {
            const int NonExistantDirId = 42;

            UserS.SeedPeshoAndGosho(this.context);

            Func<DirectoryIndexSingle> action = () => this.directoryService.GetIndexSingle(NonExistantDirId, UserS.PeshoUsername);
            action.Should().Throw<BadRequestError>().WithMessage("The directory you are trying to access eather does not exist or is not yours!");
        }

        [Test]
        public void GetIndexDHouldThrowIfDirectoryIdDoesNotBelingToUser()
        {
            const int NonExistantDirId = 42;

            UserS.SeedPeshoAndGosho(this.context);

            Func<DirectoryIndexSingle> action = () => this.directoryService.GetIndexSingle(UserS.GoshoRootDirId, UserS.PeshoUsername);
            action.Should().Throw<BadRequestError>().WithMessage("The directory you are trying to access eather does not exist or is not yours!");
        }

        [Test]///TODO: Put some data in to see the mapping
        public void GetIndexDHouldReturnTheCorrectIndexMapedFromTheWantedDir()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var peshoRoot = this.context.Directories.Single(x => x.Id == UserS.PeshoRootDirId);

            var expectedResult = new DirectoryIndexSingle
            {
                Id = peshoRoot.Id,
                Name = peshoRoot.Name,
                ParentDirectoryId = null,
                Comparisons = new HashSet<ComparisonIndex>(),
                ListsToDo = new HashSet<ListToDoIndex>(),
                Notes = new HashSet<NoteIndex>(),
                Subdirectories = new HashSet<DirectoryNavigation>(),
                Videos = new HashSet<VideoIndex>(),
            };

            Func<DirectoryIndexSingle> action = () => this.directoryService.GetIndexSingle(peshoRoot.Id, UserS.PeshoUsername);
            var result = action.Invoke();
            result.Should().BeEquivalentTo(expectedResult);
        }
        #endregion

        #endregion

        #region CreateRoot
        [Test]
        public void CreateRootCreatesRootForGivenUser()
        {
            UserS.SeedPeshoAndGosho(this.context);

            Action action = () => this.directoryService.CreateRoot(UserS.PeshoUsername);
            action.Invoke();

            var pesho = this.context.Users
                .Include(x => x.Directories)
                .Single(x => x.UserName == UserS.PeshoUsername);

            var newRootDir = pesho.Directories.SingleOrDefault(x => x.Name == "Root");
            newRootDir.Should().NotBeNull();
        }

        #endregion

        #region Delete

        [Test]
        public void DeleteShouldThrowIfUserNotFound()
        {
            const string NonExistantUsername = "What about pesho?";
            const int NonExistantDirId = 42;

            Action action = () => this.directoryService.Delete(NonExistantDirId, NonExistantUsername);
            action.Should().Throw<UserNotFound>();
        }

        [Test]
        public void DeleteShouldThrowIfUserItemNotFound()
        {
            const int NonExistantDirId = 42;

            UserS.SeedPeshoAndGosho(this.context);

            Action action = () => this.directoryService.Delete(NonExistantDirId, UserS.PeshoUsername);
            action.Should().Throw<ItemNotFound>().WithMessage("The directory you are trying to delete does not exist!");
        }

        [Test]
        public void DeleteShouldThrowIfUserItemDoesNotBelongToUser()
        {
            UserS.SeedPeshoAndGosho(this.context);

            Action action = () => this.directoryService.Delete(UserS.GoshoRootDirId, UserS.PeshoUsername);
            action.Should().Throw<AccessDenied>().WithMessage("The directory you are trying to delete does not belong to you!");
        }

        [Test]
        public void DeleteShouldThrowIfUserTrysToDeleteTheRootDirectory()
        {
            UserS.SeedPeshoAndGosho(this.context, true);

            Action action = () => this.directoryService.Delete(UserS.PeshoRealRootDirId, UserS.PeshoUsername);
            action.Should().Throw<AccessDenied>().WithMessage("Can not delete Root directory!");
        }

        /// <summary>
        /// First test does not detach all db entities before deletion, so it makes shure
        /// that the provided entities are deleted but it does not check if the delete
        /// method includes all the necessary entites for deletion
        /// 
        /// Second test is the oposite, no tracked entities at the begining of the delete
        /// but the verification that the entities are deleted comes from what the method 
        /// returns which might not be accurate for some strage reason
        /// </summary>
        [Test]
        public void DeleteDirectoryDeletesFolderAndEachItemInsideAndEachItemsChildAndAllSubdirectoriesTheSameWay()
        {
            const int childDirId = 3;

            ///Using pesho for this one
            UserS.SeedPeshoAndGosho(this.context);
            /*1*/
            var rootDir = this.context.Directories.Single(x => x.Id == UserS.PeshoRootDirId);
            ///Adding another directry child of the root;
            /*1*/
            var childDir = DirS.SeedDirectoryToDirectory(this.context, UserS.PeshoRootDirId, childDirId);

            /*1*/
            var rootVideo = VideoS.SeedVideoToUserWithTwoOrThreeNotes(this.context, UserS.PeshoId, false);
            /*2*/
            var rootVideoNotes = rootVideo.Notes;
            /*1*/
            var childVideo = VideoS.SeedVideoToUserWithTwoOrThreeNotes(this.context, UserS.PeshoId, false, childDirId);
            /*2*/
            var childVideoNotes = childVideo.Notes;

            /*1*/
            var rootListToDo = ListTDS.SeedListToUser(this.context, UserS.PeshoUsername);
            /*2*/
            var rootListToDoItems = ListTDS.SeedTwoItemsToList(this.context, rootListToDo);
            /*1*/
            var childListToDo = ListTDS.SeedListToUser(this.context, UserS.PeshoUsername, childDirId);
            /*2*/
            var childListToDoItems = ListTDS.SeedTwoItemsToList(this.context, childListToDo);

            /*1*/
            var rootNote = NoteS.SeedNoteToUser(UserS.PeshoId, this.context);
            /*2*/
            var rootCodeLines = NoteS.SeedTwoCodeLinesToNote(rootNote, this.context, true);
            /*1*/
            var childNote = NoteS.SeedNoteToUser(UserS.PeshoId, this.context, childDirId);
            /*2*/
            var childCodeLines = NoteS.SeedTwoCodeLinesToNote(childNote, this.context, true);
            /*14*/
            /*2*/
            var rootComparisons = CompS.SeedTwoCompsToUser(this.context, UserS.PeshoId);
            /*3*/
            var rootComparisonItems1 = CompS.SeedThreeItemsToComp(this.context, rootComparisons[0], true);
            /*3*/
            var rootComparisonItems2 = CompS.SeedThreeItemsToComp(this.context, rootComparisons[1], true);
            /*2*/
            var childComparisons = CompS.SeedTwoCompsToUser(this.context, UserS.PeshoId, childDirId);
            /*3*/
            var childComparisonItems1 = CompS.SeedThreeItemsToComp(this.context, childComparisons[0], true);
            /*3*/
            var childComparisonItems2 = CompS.SeedThreeItemsToComp(this.context, childComparisons[1], true);
            /*22*/
            /*TOTAL 36*/
            var allItems = new List<ISoftDeletable>();

            allItems.Add(rootDir);
            allItems.Add(childDir);

            allItems.Add(rootVideo); allItems.AddRange(rootVideoNotes);
            allItems.Add(childVideo); allItems.AddRange(childVideoNotes);

            allItems.Add(rootListToDo); allItems.AddRange(rootListToDoItems);
            allItems.Add(childListToDo); allItems.AddRange(childListToDoItems);

            allItems.Add(rootNote); allItems.AddRange(rootCodeLines);
            allItems.Add(childNote); allItems.AddRange(childCodeLines);

            allItems.AddRange(rootComparisons); allItems.AddRange(rootComparisonItems1);
            allItems.AddRange(rootComparisonItems2);
            allItems.AddRange(childComparisons); allItems.AddRange(childComparisonItems1);
            allItems.AddRange(childComparisonItems2);

            Action action = () => this.directoryService.Delete(rootDir.Id, UserS.PeshoUsername);
            action.Invoke();

            allItems.Count.Should().Be(36);
            allItems.Select(x => x.IsDeleted).Should().AllBeEquivalentTo(true);
        }

        [Test]
        public void DeleteDirectoryDeletesFolderAndEachItemInsideAndEachItemsChildAndAllSubdirectoriesTheSameWayDetached()
        {
            const int childDirId = 3;

            ///Using pesho for this one
            UserS.SeedPeshoAndGosho(this.context);
            /*1*/
            var rootDir = this.context.Directories.Single(x => x.Id == UserS.PeshoRootDirId);
            ///Adding another directry child of the root;
            /*1*/
            var childDir = DirS.SeedDirectoryToDirectory(this.context, UserS.PeshoRootDirId, childDirId);

            /*1*/
            var rootVideo = VideoS.SeedVideoToUserWithTwoOrThreeNotes(this.context, UserS.PeshoId, false);
            /*2*/
            var rootVideoNotes = rootVideo.Notes;
            /*1*/
            var childVideo = VideoS.SeedVideoToUserWithTwoOrThreeNotes(this.context, UserS.PeshoId, false, childDirId);
            /*2*/
            var childVideoNotes = childVideo.Notes;

            /*1*/
            var rootListToDo = ListTDS.SeedListToUser(this.context, UserS.PeshoUsername);
            /*2*/
            var rootListToDoItems = ListTDS.SeedTwoItemsToList(this.context, rootListToDo);
            /*1*/
            var childListToDo = ListTDS.SeedListToUser(this.context, UserS.PeshoUsername, childDirId);
            /*2*/
            var childListToDoItems = ListTDS.SeedTwoItemsToList(this.context, childListToDo);

            /*1*/
            var rootNote = NoteS.SeedNoteToUser(UserS.PeshoId, this.context);
            /*2*/
            var rootCodeLines = NoteS.SeedTwoCodeLinesToNote(rootNote, this.context, true);
            /*1*/
            var childNote = NoteS.SeedNoteToUser(UserS.PeshoId, this.context, childDirId);
            /*2*/
            var childCodeLines = NoteS.SeedTwoCodeLinesToNote(childNote, this.context, true);
            /*14*/
            /*2*/
            var rootComparisons = CompS.SeedTwoCompsToUser(this.context, UserS.PeshoId);
            /*3*/
            var rootComparisonItems1 = CompS.SeedThreeItemsToComp(this.context, rootComparisons[0], true);
            /*3*/
            var rootComparisonItems2 = CompS.SeedThreeItemsToComp(this.context, rootComparisons[1], true);
            /*2*/
            var childComparisons = CompS.SeedTwoCompsToUser(this.context, UserS.PeshoId, childDirId);
            /*3*/
            var childComparisonItems1 = CompS.SeedThreeItemsToComp(this.context, childComparisons[0], true);
            /*3*/
            var childComparisonItems2 = CompS.SeedThreeItemsToComp(this.context, childComparisons[1], true);
            /*22*/
            /*TOTAL 36*/

            ChangeTrackerOperations.DetachAll(this.context);
            Func<Directory> action = () => this.directoryService.Delete(rootDir.Id, UserS.PeshoUsername);
            var deletedfDir = action.Invoke();

            var allItems = new List<ISoftDeletable>();
            rootDir = deletedfDir;
            childDir = rootDir.Subdirectories.Single();

            allItems.Add(rootDir);
            allItems.Add(childDir);

            allItems.AddRange(rootDir.Videos); allItems.AddRange(rootDir.Videos.Single().Notes);
            allItems.AddRange(childDir.Videos); allItems.AddRange(childDir.Videos.Single().Notes);

            allItems.AddRange(rootDir.ListsToDo); allItems.AddRange(rootDir.ListsToDo.Single().Items);
            allItems.AddRange(childDir.ListsToDo); allItems.AddRange(childDir.ListsToDo.Single().Items);

            allItems.AddRange(rootDir.Notes); allItems.AddRange(rootDir.Notes.Single().Lines);
            allItems.AddRange(childDir.Notes); allItems.AddRange(childDir.Notes.Single().Lines);

            allItems.AddRange(rootDir.Comparisons); allItems.AddRange(rootDir.Comparisons.First().Items);
            allItems.AddRange(rootDir.Comparisons.Last().Items);
            allItems.AddRange(childDir.Comparisons); allItems.AddRange(childDir.Comparisons.First().Items);
            allItems.AddRange(childDir.Comparisons.Last().Items);

            allItems.Count.Should().Be(36);
            allItems.Select(x => x.IsDeleted).Should().AllBeEquivalentTo(true);
        }
        #endregion
    }
}
