namespace Momento.Tests.Tests.DirectoryServiceTests
{

    using FluentAssertions;
    using Momento.Models.Contracts;
    using Momento.Services.Contracts.Directory;
    using Momento.Services.Implementations.Directory;
    using Momento.Tests.Contracts;
    using Momento.Tests.Seeding;
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

        //[Test]
        //public void CreateThrowsIfParentDirectoryDoesNotExist()
        //{
        //}

        #endregion

        #region Delete
        [Test]///Give it a once over
        public void DeleteDirectoryDeletesFolderAndEachItemInsideAndEachItemsChildAndAllSubdirectoriesTheSameWay()
        {
            const int childDirId = 3;

            ///Using pesho for this one
            UserS.SeedPeshoAndGosho(this.context);
            var rootDir = this.context.Directories.Single(x => x.Id == UserS.PeshoRootDirId);
            ///Adding another directry child of the root;
            var childDir = DirS.SeedDirectoryToDirectory(this.context,UserS.PeshoRootDirId, childDirId);

            var rootVideo = VideoS.SeedVideoToUserWithNotes(this.context,UserS.PeshoId,false);
            var rootVideoNotes = rootVideo.Notes;
            var childVideo = VideoS.SeedVideoToUserWithNotes(this.context, UserS.PeshoId, false, childDirId);
            var childVideoNotes = childVideo.Notes;

            var rootListToDo = ListTDS.SeedListToUser(this.context,UserS.PeshoUsername);
            var rootListToDoItems = ListTDS.SeedTwoItemsToList(this.context, rootListToDo);
            var childListToDo = ListTDS.SeedListToUser(this.context, UserS.PeshoUsername, childDirId);
            var childListToDoItems = ListTDS.SeedTwoItemsToList(this.context, childListToDo);

            var rootNote = NoteS.SeedNoteToUser(UserS.PeshoId,this.context);
            var rootCodeLines = NoteS.SeedTwoCodeLinesToNote(rootNote,this.context,true);
            var childNote = NoteS.SeedNoteToUser(UserS.PeshoId, this.context, childDirId);
            var childCodeLines = NoteS.SeedTwoCodeLinesToNote(childNote, this.context,true);

            var rootComparisons = CompS.SeedTwoCompsToUser(this.context, UserS.PeshoId);
            var rootComparison1 = CompS.SeedThreeItemsToComp(this.context, rootComparisons[0],true);
            var rootComparison2 = CompS.SeedThreeItemsToComp(this.context, rootComparisons[1],true);
            var childComparisons = CompS.SeedTwoCompsToUser(this.context, UserS.PeshoId, childDirId);
            var childComparison1 = CompS.SeedThreeItemsToComp(this.context, childComparisons[0],true);
            var childComparison2 = CompS.SeedThreeItemsToComp(this.context, childComparisons[1],true);

            var allItems = new List<ISoftDeletable>();

            allItems.Add(rootDir);
            allItems.Add(childDir);

            allItems.Add(rootVideo); allItems.AddRange(rootVideoNotes);
            allItems.Add(childVideo); allItems.AddRange(childVideoNotes);

            allItems.Add(rootListToDo); allItems.AddRange(rootListToDoItems);
            allItems.Add(childListToDo); allItems.AddRange(childListToDoItems);

            allItems.Add(rootNote); allItems.AddRange(rootCodeLines);
            allItems.Add(childNote); allItems.AddRange(childCodeLines);

            allItems.AddRange(rootComparisons); allItems.AddRange(rootComparison1);
            allItems.AddRange(rootComparison2);
            allItems.AddRange(childComparisons); allItems.AddRange(childComparison1);
            allItems.AddRange(childComparison2);

            Action action = () => this.directoryService.Delete(rootDir.Id,UserS.PeshoUsername);
            action.Invoke();

            allItems.Select(x => x.IsDeleted).Should().AllBeEquivalentTo(true);
        }
        #endregion
    }
}
