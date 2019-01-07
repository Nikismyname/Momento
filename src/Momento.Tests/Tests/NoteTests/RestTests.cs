namespace Momento.Tests.Tests.NoteTests
{
    #region Initialization
    using FluentAssertions;
    using Momento.Models.Notes;
    using Momento.Services.Contracts.Notes;
    using Momento.Services.Exceptions;
    using Momento.Services.Implementations.Notes;
    using Momento.Services.Implementations.Shared;
    using Momento.Services.Implementations.Utilities;
    using Momento.Services.Models.NoteModels;
    using Momento.Tests.Contracts;
    using Momento.Tests.Seeding;
    using Momento.Tests.Utilities;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class RestTests : BaseTestsSqliteInMemory
    {
        private INoteService noteService;

        public override void Setup()
        {
            base.Setup();
            this.noteService = new NoteService(
                this.context,
                new UtilitiesService()/*Not Used In Tose Tests*/,
                new TrackableService(this.context)/*very small and simple and not in full use yet*/);
        }
        #endregion

        #region Create
        [Test]
        public void CreateShouldThrowIfUserNotFound()
        {
            const string NonExistantUsername = "Of course note Pesho";

            ///Not important for this test
            var createData = new NoteCreate
            {
                Description = "",
                DirectoryId = 1,
                Name = "",
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Func<Note> action = () => this.noteService.Create(createData, NonExistantUsername);

            action.Should().Throw<UserNotFound>();
        }

        [Test]
        public void CreateShouldThrowIfDirectoryNotFound()
        {
            const int NonExistantDirectoryId = 42;

            UserS.SeedPeshoAndGosho(this.context);

            ///Not important for this test
            var createData = new NoteCreate
            {
                Description = "",
                DirectoryId = NonExistantDirectoryId,
                Name = "",
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Func<Note> action = () => this.noteService.Create(createData, UserS.PeshoUsername);

            action.Should().Throw<ItemNotFound>().WithMessage("The directory you are trying to add a note to does not exist!");
        }

        [Test]
        public void CreateShouldThrowIfDirectoryDoesNotBelingToUser()
        {
            UserS.SeedPeshoAndGosho(this.context);

            ///Not important for this test
            var createData = new NoteCreate
            {
                Description = "",
                DirectoryId = UserS.GoshoRootDirId,
                Name = "",
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Func<Note> action = () => this.noteService.Create(createData, UserS.PeshoUsername);

            action.Should().Throw<AccessDenied>().WithMessage("The directory you are trying to add a note to does not beling to you!");
        }

        [Test]
        public void CreateShouldCreateObjectWithTheRightPropertiesValues()
        {
            const string InitialDescription = "devine description";
            const string InitialName = "devine name";

            UserS.SeedPeshoAndGosho(this.context);

            ///Not important for this test
            var createData = new NoteCreate
            {
                Description = InitialDescription,
                DirectoryId = UserS.PeshoRootDirId,
                Name = InitialName,
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Func<Note> action = () => this.noteService.Create(createData, UserS.PeshoUsername);
            var result = action.Invoke();

            result.UserId.Should().Be(UserS.PeshoId);
            result.DirectoryId.Should().Be(UserS.PeshoRootDirId);
            result.Description.Should().Be(InitialDescription);
            result.Name.Should().Be(InitialName);
        }

        [Test]
        public void CreateShouldSetTheOrderCorrectly()
        {
            const string InitialDescription = "devine description";
            const string InitialName = "devine name";

            UserS.SeedPeshoAndGosho(this.context);

            ///Not important for this test
            var createData = new NoteCreate
            {
                Description = InitialDescription,
                DirectoryId = UserS.PeshoRootDirId,
                Name = InitialName,
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Func<Note> action = () => this.noteService.Create(createData, UserS.PeshoUsername);
            var order0 = action.Invoke();
            action.Invoke();
            action.Invoke();
            action.Invoke();
            var order4 = action.Invoke();

            order0.Order.Should().Be(0);
            order4.Order.Should().Be(4);
        }
        #endregion

        #region GetForEdit

        [Test]
        public void GetForEditShouldThrowIfUserNotFound()
        {
            const string NonExistantUsername = "Can't belive its not Pesho";
            const int NonExistantNoteId = 42;

            ChangeTrackerOperations.DetachAll(this.context);
            Func<NoteEdit> action = () => this.noteService.GetForEdit(NonExistantNoteId, NonExistantUsername);
            action.Should().Throw<UserNotFound>();
        }

        [Test]
        public void GetForEditShouldThrowIfItemNotFound()
        {
            const int NonExistantNoteId = 42;

            UserS.SeedPeshoAndGosho(this.context);

            ChangeTrackerOperations.DetachAll(this.context);
            Func<NoteEdit> action = () => this.noteService.GetForEdit(NonExistantNoteId, UserS.PeshoUsername);
            action.Should().Throw<ItemNotFound>().WithMessage("The note you are trying to get does not exist!");
        }

        [Test]
        public void GetForEditShouldThrowIfItemDoesNotBelongToUser()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var note = NoteS.SeedNoteToUser(UserS.GoshoId, this.context);

            ChangeTrackerOperations.DetachAll(this.context);
            Func<NoteEdit> action = () => this.noteService.GetForEdit(note.Id, UserS.PeshoUsername);
            action.Should().Throw<AccessDenied>().WithMessage("The note you are trying to get does not belong to you!");
        }

        [Test]
        public void GetForEditShouldReturnTheCorrectResult()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var note = NoteS.SeedNoteToUser(UserS.PeshoId, this.context);
            var lines = NoteS.SeedTwoCodeLinesToNote(note, context);

            var expectedResult = new NoteEdit
            {
                Id = NoteS.NoteId,
                Description = NoteS.NoteDesctiption,
                EditorMode = NoteS.NoteEditorModel,
                MainNoteContent = NoteS.NoteMainNoteContent,
                Name = NoteS.NoteName,
                ShowSourceEditor = NoteS.NoteShowSourceEditor,
                Source = NoteS.NoteSource,

                Lines = new HashSet<CodeLineEdit>
                {
                    new CodeLineEdit
                    {
                        Id = NoteS.Line1Id,
                        EditorMode = NoteS.Line1EditorMode,
                        InPageId = NoteS.Line1InPageId,
                        NoteContent = NoteS.Line1NoteContent,
                        Order = NoteS.Line1Order,
                        SourceContent = NoteS.Line1SourceContent,
                        Visible = NoteS.Line1Visible,
                    },
                    new CodeLineEdit
                    {
                        Id = NoteS.Line2Id,
                        EditorMode = NoteS.Line2EditorMode,
                        InPageId = NoteS.Line2InPageId,
                        NoteContent = NoteS.Line2NoteContent,
                        Order = NoteS.Line2Order,
                        SourceContent = NoteS.Line2SourceContent,
                        Visible = NoteS.Line2Visible,
                    },
                }
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Func<NoteEdit> action = () => this.noteService.GetForEdit(note.Id, UserS.PeshoUsername);
            var result = action.Invoke();

            result.Should().BeEquivalentTo(expectedResult);
        }
        #endregion

        #region Delete
        [Test]
        public void DeleteShouldThrowIfUserNotFound()
        {
            const string NonExistantUsername = "Pesho under cover";
            const int NonExistantNoteId = 42;

            ChangeTrackerOperations.DetachAll(this.context);
            Func<Note> action = () => this.noteService.Delete(NonExistantNoteId, NonExistantUsername);
            action.Should().Throw<UserNotFound>();
        }

        [Test]
        public void DeleteShouldThrowIfItemNotFound()
        {
            UserS.SeedPeshoAndGosho(this.context);
            const int NonExistantNoteId = 42;

            ChangeTrackerOperations.DetachAll(this.context);
            Func<Note> action = () => this.noteService.Delete(NonExistantNoteId, UserS.PeshoUsername);
            action.Should().Throw<ItemNotFound>().WithMessage("The note you are trying to delete does not exist!");
        }

        [Test]
        public void DeleteShouldThrowIfItemDoesNotBelongToUser()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var note = NoteS.SeedNoteToUser(UserS.GoshoId, this.context);

            ChangeTrackerOperations.DetachAll(this.context);
            Func<Note> action = () => this.noteService.Delete(note.Id, UserS.PeshoUsername);
            action.Should().Throw<AccessDenied>().WithMessage("The note you are trying to delete does not belong to you!");
        }

        [Test]
        public void DeleteShouldDeleteTheGivenNoteAndAllItsChildren()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var note = NoteS.SeedNoteToUser(UserS.GoshoId, this.context);
            var lines = NoteS.SeedTwoCodeLinesToNote(note, this.context);

            ChangeTrackerOperations.DetachAll(this.context);
            Func<Note> action = () => this.noteService.Delete(note.Id, UserS.GoshoUsername);
            var result = action.Invoke();

            result.Lines.Count.Should().Be(2);
            result.IsDeleted.Should().Be(true);
            result.Lines.Select(x => x.IsDeleted).Should().AllBeEquivalentTo(true);
        }
        #endregion
    }
}
