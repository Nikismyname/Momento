namespace Momento.Tests.Tests.NoteTests
{
    using FluentAssertions;
    using Momento.Services.Contracts.Notes;
    using Momento.Services.Exceptions;
    using Momento.Services.Implementations.Notes;
    using Momento.Services.Implementations.Shared;
    using Momento.Services.Implementations.Utilities;
    using Momento.Services.Mapping;
    using Momento.Services.Models.NoteModels;
    using Momento.Services.Models.VideoModels;
    using Momento.Tests.Contracts;
    using Momento.Tests.Seeding;
    using NUnit.Framework;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SaveTests : BaseTestsSqliteInMemory
    {
        private INoteService noteService;

        public override void Setup()
        {
            base.Setup();
            ///TODO: I guess I have to mock this
            AutoMapperConfig.RegisterMappings(typeof(VideoCreate).Assembly);
            this.noteService = new NoteService(this.context, new UtilitiesService(), new TrackableService(this.context));
        }

        [Test]
        public void ShouldThrowIfUserNotFound()
        {
            const string NonExistantUsername = "nonExistantUsername";
            UserS.SeedPeshoAndGosho(this.context);

            var note = NoteS.SeedNoteToUser(UserS.PeshoId, this.context);
            Action action = () => this.noteService.Save(new NoteEdit(), NonExistantUsername);

            action.Should().Throw<UserNotFound>();
        }

        [Test]
        public void ShouldThrowIfNoteNotFound()
        {
            const int nonExistantNoteId = 42;
            UserS.SeedPeshoAndGosho(this.context);
            var note = NoteS.SeedNoteToUser(UserS.PeshoId, this.context);
            Action action = () => this.noteService.Save(new NoteEdit { Id = nonExistantNoteId }, UserS.GoshoUsername);
            action.Should().Throw<ItemNotFound>().WithMessage("The note you are trying to edit does not exist!");
        }

        [Test]
        public void ShouldThrowIfNoteUserIdDoesNotMatchUserId()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var note = NoteS.SeedNoteToUser(UserS.PeshoId, this.context);
            var noteId = note.Id;
            Action action = () => this.noteService.Save(new NoteEdit { Id = noteId }, UserS.GoshoUsername);
            action.Should().Throw<AccessDenied>().WithMessage("The note you are trying to edit does not belong to you!");
        }

        [Test]
        ///Excluding Id, name, description for now
        public void SaveShouldChangePrimitiveProperties()
        {
            const string newSource = "best source";
            const string newMainNoteContent = "best main note content!";
            const bool newEditorMode = true;
            const bool newShowSourceEditor = true;

            UserS.SeedPeshoAndGosho(this.context);
            var note = NoteS.SeedNoteToUser(UserS.PeshoId, this.context);

            var editInput = new NoteEdit
            {
                Id = note.Id,
                EditorMode = newEditorMode,
                MainNoteContent = newMainNoteContent,
                ShowSourceEditor = newShowSourceEditor,
                Source = newSource,
            };

            Action action = () => this.noteService.Save(editInput, UserS.PeshoUsername);
            action.Invoke();

            note.EditorMode.Should().Be(newEditorMode);
            note.Source.Should().Be(newSource);
            note.MainNoteContent.Should().Be(newMainNoteContent);
            note.ShowSourceEditor.Should().Be(newShowSourceEditor);
        }

        [Test]
        public void SaveShouldThrowIdLinesToBeUpdatedsIdsDoNoteBelongTheGivenNote()
        {
            const int NonExistantCodeLineId = 42;

            UserS.SeedPeshoAndGosho(this.context);
            var note = NoteS.SeedNoteToUser(UserS.PeshoId, this.context);
            var codeLines = NoteS.SeedTwoCodeLinesToNote(note, this.context);

            var editInput = new NoteEdit
            {
                Id = note.Id,
                Lines = new HashSet<CodeLineEdit>
                {
                    new CodeLineEdit
                    {
                        Id = codeLines[0].Id,
                    },
                    new CodeLineEdit
                    {
                        Id = NonExistantCodeLineId,
                    }
                }
            };

            Action action = () => this.noteService.Save(editInput, UserS.PeshoUsername);

            action.Should().Throw<AccessDenied>().WithMessage("The Lines you are trying to edit do not belong the Note you are editing!");
        }

        [Test]
        public void SaveShouldUpdateExistionCodeLines()
        {
            const bool newEditorMode = true;
            const bool newVisible = true;
            const int line1newInpageId = 42;
            const int line2newInpageId = 43;
            const string line1NewNoteContent = "line one new note content";
            const string line2NewNoteContent = "line two new note content";
            const int line1newOrder = 44;
            const int line2newOrder = 45;
            const string line1NewSourceContent = "line one new source content";
            const string line2NewSourceContent = "line two new source content";

            UserS.SeedPeshoAndGosho(this.context);
            var note = NoteS.SeedNoteToUser(UserS.PeshoId, this.context);
            var codeLines = NoteS.SeedTwoCodeLinesToNote(note, this.context);

            var editInput = new NoteEdit
            {
                Id = note.Id,
                Lines = new HashSet<CodeLineEdit>
                {
                    new CodeLineEdit
                    {
                        Id = codeLines[0].Id,
                        EditorMode = newEditorMode,
                        InPageId =line1newInpageId ,
                        NoteContent = line1NewNoteContent,
                        Order =line1newOrder,
                        SourceContent = line1NewSourceContent,
                        Visible = newVisible,
                    },
                    new CodeLineEdit
                    {
                        Id = codeLines[1].Id,
                        EditorMode = newEditorMode,
                        InPageId =line2newInpageId ,
                        NoteContent = line2NewNoteContent,
                        Order =line2newOrder,
                        SourceContent = line2NewSourceContent,
                        Visible = newVisible,
                    },
                }
            };

            Action action = () => this.noteService.Save(editInput, UserS.PeshoUsername);
            action.Invoke();

            codeLines[0].EditorMode.Should().Be(newEditorMode);
            codeLines[0].InPageId.Should().Be(line1newInpageId);
            codeLines[0].NoteContent.Should().Be(line1NewNoteContent);
            codeLines[0].Order.Should().Be(line1newOrder);
            codeLines[0].SourceContent.Should().Be(line1NewSourceContent);
            codeLines[0].Visible.Should().Be(newVisible);

            //codeLines[1].EditorMode.Should().Be(newEditorMode);
            //codeLines[1].InPageId.Should().Be(line2newInpageId);
            //codeLines[1].NoteContent.Should().Be(line2NewNoteContent);
            //codeLines[1].Order.Should().Be(line2newOrder);
            //codeLines[1].SourceContent.Should().Be(line2NewSourceContent);
            //codeLines[1].Visible.Should().Be(newVisible);
        }

        [Test]
        public void SaveShouldCreateNewCodeLines()
        {
            const bool newEditorMode = true;
            const bool newVisible = true;
            const int line1newInpageId = 42;
            const string line1NewNoteContent = "line one new note content";
            const int line1newOrder = 44;
            const string line1NewSourceContent = "line one new source content";

            UserS.SeedPeshoAndGosho(this.context);
            var note = NoteS.SeedNoteToUser(UserS.PeshoId, this.context);

            var editInput = new NoteEdit
            {
                Id = note.Id,
                Lines = new HashSet<CodeLineEdit>
                {
                    new CodeLineEdit
                    {
                        Id = 0,
                        EditorMode = newEditorMode,
                        InPageId =line1newInpageId ,
                        NoteContent = line1NewNoteContent,
                        Order =line1newOrder,
                        SourceContent = line1NewSourceContent,
                        Visible = newVisible,
                    },
                }
            };

            Action action = () => this.noteService.Save(editInput, UserS.PeshoUsername);
            action.Invoke();

            var codeLine = note.Lines.SingleOrDefault();

            codeLine.EditorMode.Should().Be(newEditorMode);
            codeLine.InPageId.Should().Be(line1newInpageId);
            codeLine.NoteContent.Should().Be(line1NewNoteContent);
            codeLine.Order.Should().Be(line1newOrder);
            codeLine.SourceContent.Should().Be(line1NewSourceContent);
            codeLine.Visible.Should().Be(newVisible);
        }

        [Test]
        public void SaveShouldDeleteRemovedCodeLines()
        {
            const bool newEditorMode = true;
            const bool newVisible = true;
            const int line1newInpageId = 42;
            const string line1NewNoteContent = "line one new note content";
            const int line1newOrder = 44;
            const string line1NewSourceContent = "line one new source content";

            UserS.SeedPeshoAndGosho(this.context);
            var note = NoteS.SeedNoteToUser(UserS.PeshoId, this.context);
            var codeLines = NoteS.SeedTwoCodeLinesToNote(note, this.context);

            var editInput = new NoteEdit
            {
                Id = note.Id,
                Lines = new HashSet<CodeLineEdit>
                {
                    new CodeLineEdit
                    {
                        Id = codeLines[0].Id,
                        EditorMode = newEditorMode,
                        InPageId =line1newInpageId ,
                        NoteContent = line1NewNoteContent,
                        Order =line1newOrder,
                        SourceContent = line1NewSourceContent,
                        Visible = newVisible,
                    },
                }
            };

            Action action = () => this.noteService.Save(editInput, UserS.PeshoUsername);
            action.Invoke();

            ///Since it is passed back from the edit model it is not deleted
            codeLines[0].IsDeleted.Should().Be(false);
            ///Since it is not passed back it is understood it should be deleted
            codeLines[1].IsDeleted.Should().Be(true);
        }
    }
}
