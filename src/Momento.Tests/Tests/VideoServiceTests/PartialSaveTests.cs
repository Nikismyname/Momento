namespace Momento.Tests.VideoServiceTests
{
    using AutoMapper;
    using FluentAssertions;
    using Momento.Models.Enums;
    using Momento.Services.Contracts.Video;
    using Momento.Services.Exceptions;
    using Momento.Services.Implementations.Shared;
    using Momento.Services.Implementations.Video;
    using Momento.Services.Models.Video;
    using Momento.Tests.Contracts;
    using Momento.Tests.Seeding;
    using Momento.Web.Profiles;
    using NUnit.Framework;
    using System;
    using System.Linq;

    public class PartialSaveTests : BaseTestsSqlLiteInMemory
    {
        private IVideoService videoService;

        public override void Setup()
        {
            base.Setup();

            var myProfile = new MomentoProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(myProfile));
            var mapper = new Mapper(configuration);

            var trackableService = new TrackableService(this.context);

            this.videoService = new VideoService(
                    this.context,
                    mapper,
                    trackableService);
        }

        #region ValidateSaveAndRegisterModification
        [Test]
        public void ShouldThrowIfVIdeoIsNotFound()
        {
            const int NonExistantVideoId = 12;
            Action action = CallSaveFunction(NonExistantVideoId, Seeder.GoshoUsername);
            action.Should().Throw<ItemNotFound>().WithMessage("The video you are working on does not exists in the database");
        }

        [Test]
        public void ShouldThrowIfVIdeoDoesNotBelogToUser()
        {
            Seeder.SeedPeshoAndGosho(context);
            var video = Seeder.SeedVideosToUser(context, Seeder.GoshoId);
            Action action = CallSaveFunction(video.Id, Seeder.PeshoUsername);
            action.Should().Throw<AccessDenied>().WithMessage("The video you are trying to medify does not belong to you!");
        }

        [Test]
        public void ShouldThrowIfIdsToChangeDoNotBelongToVideo()
        {
            Seeder.SeedPeshoAndGosho(context);
            var video = Seeder.SeedVideosToUserWithNotes(context, Seeder.GoshoId);
            var changes = new string[][]
            {
                new string[]{"1", "some prop", "some val" },
                new string[]{"2", "some prop", "some val"},
                ///The existing values are 1 and 2, 3 is not in that video; 
                new string[]{"3", "some prop", "some val"}
            };

            Action action = CallSaveFunction(video.Id, Seeder.GoshoUsername, changes);
            action.Should().Throw<AccessDenied>().WithMessage("The video notes you are trying to modify does not belong the the current video");
        }
        #endregion

        #region PartialSaveVideoFields
        [Test]
        public void ShouldNotChangeVideoFieldsIfTheyAreAllNull()
        {
            Seeder.SeedPeshoAndGosho(context);
            var video = Seeder.SeedVideosToUserWithNotes(context, Seeder.GoshoId);
            Action action = CallSaveFunction(video.Id, Seeder.GoshoUsername);
            action.Invoke();

            video.SeekTo.Should().Be(Seeder.DefaultVideoSeekTo);
            video.Name.Should().Be(Seeder.DefaultVideoName);
            video.Description.Should().Be(Seeder.DefaultVideoDesctiption);
            video.Url.Should().Be(Seeder.DefaultVideoUrl);
        }

        [Test]
        public void ShouldChangeVideoFieldsIfTheyAreProvided()
        {
            const int NewSeekTo = 20;
            const string NewName = "NewName";
            const string NewDescription = "NewDescription";
            const string NewUrl = "NewUrl";

            Seeder.SeedPeshoAndGosho(context);
            var video = Seeder.SeedVideosToUserWithNotes(context, Seeder.GoshoId);
            Action action = CallSaveFunction(video.Id, Seeder.GoshoUsername, NewSeekTo, NewName, NewDescription, NewUrl);
            action.Invoke();

            video.SeekTo.Should().Be(NewSeekTo);
            video.Name.Should().Be(NewName);
            video.Description.Should().Be(NewDescription);
            video.Url.Should().Be(NewUrl);
        }
        #endregion

        #region PartialSaveChangesToExistingNote
        [Test]
        public void AppliesContentChangesToExistingNotesCorrectly()
        {
            const string Note1NewContent = "newContentForNote1";
            const string Note2NewContent = "newContentForNote2";

            Seeder.SeedPeshoAndGosho(context);
            var video = Seeder.SeedVideosToUserWithNotes(context, Seeder.GoshoId);
            var changes = new string[][]
            {
                new string[]{"1", "Content", Note1NewContent },
                new string[]{"2", "Content", Note2NewContent }
            };
            Action action = CallSaveFunction(video.Id, Seeder.GoshoUsername, changes);
            action.Invoke();

            var note1 = video.Notes.SingleOrDefault(x => x.Id == 1);
            var note2 = video.Notes.SingleOrDefault(x => x.Id == 2);

            note1.Content.Should().Be(Note1NewContent);
            note2.Content.Should().Be(Note2NewContent);
        }

        [Test]
        public void AppliesDeletedChangeToExistingNotesShouldSoftDeleteThem()
        {
            Seeder.SeedPeshoAndGosho(context);
            var video = Seeder.SeedVideosToUserWithNotes(context, Seeder.GoshoId);
            var changes = new string[][]
            {
                new string[]{"1", "Deleted", "Not Used"},
                new string[]{"2", "Deleted", "Not Used"}
            };
            Action action = CallSaveFunction(video.Id, Seeder.GoshoUsername, changes);
            action.Invoke();

            var note1 = video.Notes.SingleOrDefault(x => x.Id == 1);
            var note2 = video.Notes.SingleOrDefault(x => x.Id == 2);

            note1.IsDeleted.Should().Be(true);
            note2.IsDeleted.Should().Be(true);
        }

        [Test]
        public void AppliesFormattingChangesToExistingNotesCorrectly()
        {
            const Formatting Note1NewFormatting = Formatting.SQL;
            const Formatting Note2NewFormatting = Formatting.None;

            Seeder.SeedPeshoAndGosho(context);
            var video = Seeder.SeedVideosToUserWithNotes(context, Seeder.GoshoId);
            var changes = new string[][]
            {
                new string[]{"1", "Formatting", ((int)Note1NewFormatting).ToString() },
                new string[]{"2", "Formatting", ((int)Note2NewFormatting).ToString() }
            };
            Action action = CallSaveFunction(video.Id, Seeder.GoshoUsername, changes);
            action.Invoke();

            var note1 = video.Notes.SingleOrDefault(x => x.Id == 1);
            var note2 = video.Notes.SingleOrDefault(x => x.Id == 2);

            note1.Formatting.Should().Be(Note1NewFormatting);
            note2.Formatting.Should().Be(Note2NewFormatting);
        }

        [Test]
        public void AppliesSeekToChangesToExistingNotesCorrectly()
        {
            const int Note1NewSeekTo = 15;
            const int Note2NewSeekTo = 16;

            Seeder.SeedPeshoAndGosho(context);
            var video = Seeder.SeedVideosToUserWithNotes(context, Seeder.GoshoId);
            var changes = new string[][]
            {
                new string[]{"1", "SeekTo", Note1NewSeekTo.ToString() },
                new string[]{"2", "SeekTo", Note2NewSeekTo.ToString() }
            };
            Action action = CallSaveFunction(video.Id, Seeder.GoshoUsername, changes);
            action.Invoke();

            var note1 = video.Notes.SingleOrDefault(x => x.Id == 1);
            var note2 = video.Notes.SingleOrDefault(x => x.Id == 2);

            note1.SeekTo.Should().Be(Note1NewSeekTo);
            note2.SeekTo.Should().Be(Note2NewSeekTo);
        }
        #endregion

        #region PartialSaveNewNotes
        [Test]
        public void ShoudCreate3nestedDivsStartingAtRootCorrectly()
        {
            Seeder.SeedPeshoAndGosho(this.context);
            var video = Seeder.SeedVideosToUserWithNotes(context, Seeder.GoshoId);
            var newNotes = Seeder.GenerateNoteCreateSimpleNested();
            Action action = CallSaveFunction(video.Id, Seeder.GoshoUsername, newNotes);
            action.Invoke();

            var rootNote = video.Notes.SingleOrDefault(x => x.Content == "RootNote");
            rootNote.Note.Should().BeNull();
            rootNote.ChildNotes.Count.Should().Be(1);

            var level2Note = rootNote.ChildNotes.SingleOrDefault();
            level2Note.Content.Should().Be("NestedLevel2");
            level2Note.ChildNotes.Count.Should().Be(1);

            var level3Note = level2Note.ChildNotes.SingleOrDefault();
            level3Note.Content.Should().Be("NestedLevel3");
            level3Note.ChildNotes.Count.Should().Be(0);

            video.Notes.Count.Should().Be(5);
        }
        #endregion

        private Action CallSaveFunction(int videoId, string username)
        {
            return () => this.videoService.PartialSave(
                videoId,
                username,
                null,
                null,
                null,
                null,
                new string[][] { },
                new VideoNoteCreate[] { },
                false);
        }

        private Action CallSaveFunction(int videoId, string username, VideoNoteCreate[] newNotes)
        {
            return () => this.videoService.PartialSave(
                videoId,
                username,
                null,
                null,
                null,
                null,
                new string[][] { },
                newNotes,
                false);
        }

        private Action CallSaveFunction(int videoId, string username, string[][] changes)
        {
            return () => this.videoService.PartialSave(
                videoId,
                username,
                null,
                null,
                null,
                null,
                changes,
                new VideoNoteCreate[] { },
                false);
        }

        private Action CallSaveFunction(int videoId, string username, int? seekTo, string name, string description, string url)
        {
            return () => this.videoService.PartialSave(
                videoId,
                username,
                seekTo,
                name,
                description,
                url,
                new string[][] { },
                new VideoNoteCreate[] { },
                false);
        }
    }
}
