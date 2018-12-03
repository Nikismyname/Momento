namespace Momento.Tests.VideoServiceTests
{
    #region Initialization
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
    using System.Collections.Generic;
    using System.Linq;

    public class PartialSaveTests : BaseTestsSqliteInMemory
    {
        private IVideoService videoService;

        public override void Setup()
        {
            base.Setup();

            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(new MomentoProfile()));
            var mapper = new Mapper(configuration);

            var trackableService = new TrackableService(this.context);

            this.videoService = new VideoService(
                    this.context,
                    mapper,
                    trackableService);
        }
        #endregion

        #region ValidateSaveAndRegisterModification
        [Test]
        public void ShouldThrowIfVIdeoIsNotFound()
        {
            const int NonExistantVideoId = 12;
            Action action = GetPartialSaveAction(NonExistantVideoId, Seeder.GoshoUsername);
            action.Should().Throw<ItemNotFound>().WithMessage("The video you are working on does not exists in the database");
        }

        [Test]
        public void ShouldThrowIfVIdeoDoesNotBelogToUser()
        {
            Seeder.SeedPeshoAndGosho(context);
            var video = Seeder.SeedVideosToUser(context, Seeder.GoshoId);
            Action action = GetPartialSaveAction(video.Id, Seeder.PeshoUsername);
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

            Action action = GetPartialSaveAction(video.Id, Seeder.GoshoUsername, changes);
            action.Should().Throw<AccessDenied>().WithMessage("The video notes you are trying to modify does not belong the the current video");
        }
        #endregion

        #region PartialSaveVideoFields
        [Test]
        public void ShouldNotChangeVideoFieldsIfTheyAreAllNull()
        {
            Seeder.SeedPeshoAndGosho(context);
            var video = Seeder.SeedVideosToUserWithNotes(context, Seeder.GoshoId);
            Action action = GetPartialSaveAction(video.Id, Seeder.GoshoUsername);
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
            Action action = GetPartialSaveAction(video.Id, Seeder.GoshoUsername, NewSeekTo, NewName, NewDescription, NewUrl);
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
            Action action = GetPartialSaveAction(video.Id, Seeder.GoshoUsername, changes);
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
            Action action = GetPartialSaveAction(video.Id, Seeder.GoshoUsername, changes);
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
            Action action = GetPartialSaveAction(video.Id, Seeder.GoshoUsername, changes);
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
            Action action = GetPartialSaveAction(video.Id, Seeder.GoshoUsername, changes);
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
            var newNotes = Seeder.GenerateNoteCreateSimpleNested(null);
            Action action = GetPartialSaveAction(video.Id, Seeder.GoshoUsername, newNotes);
            action.Invoke();

            var level1Note = video.Notes.SingleOrDefault(x => x.Content == "NestedLevel1");
            level1Note.Note.Should().BeNull();
            level1Note.ChildNotes.Count.Should().Be(1);

            var level2Note = level1Note.ChildNotes.SingleOrDefault();
            level2Note.Content.Should().Be("NestedLevel2");
            level2Note.ChildNotes.Count.Should().Be(1);

            var level3Note = level2Note.ChildNotes.SingleOrDefault();
            level3Note.Content.Should().Be("NestedLevel3");
            level3Note.ChildNotes.Count.Should().Be(0);

            video.Notes.Count.Should().Be(5);
        }

        [Test]
        public void ShoudCreate3nestedDivsStartingAtExistingNote()
        {
            Seeder.SeedPeshoAndGosho(this.context);
            var video = Seeder.SeedVideosToUserWithNotes(context, Seeder.GoshoId);
            var newNotes = Seeder.GenerateNoteCreateSimpleNested(Seeder.preExistingNote1Id);
            Action action = this.GetPartialSaveAction(video.Id, Seeder.GoshoUsername, newNotes);
            action.Invoke();

            var preExistingRootNote = context.VideoNotes.SingleOrDefault(x => x.Id == Seeder.preExistingNote1Id);
            preExistingRootNote.ChildNotes.Count.Should().Be(1);

            var level1Note = preExistingRootNote.ChildNotes.SingleOrDefault();
            level1Note.Content.Should().Be("NestedLevel1");
            level1Note.ChildNotes.Count.Should().Be(1);

            var level2Note = level1Note.ChildNotes.SingleOrDefault();
            level2Note.Content.Should().Be("NestedLevel2");
            level2Note.ChildNotes.Count.Should().Be(1);

            var level3Note = level2Note.ChildNotes.SingleOrDefault();
            level3Note.Content.Should().Be("NestedLevel3");
            level3Note.ChildNotes.Count.Should().Be(0);

            video.Notes.Count.Should().Be(5);
        }

        [Test]
        public void IfTheNestingLevelIsGreaterThan4Throw()
        {
            Seeder.SeedPeshoAndGosho(this.context);
            var video = Seeder.SeedVideosToUserWithNotes(context, Seeder.GoshoId);
            var newNotes = Seeder.GenerateNoteCreateSimpleNested(Seeder.preExistingNote1Id, 4);
            Action action = this.GetPartialSaveAction(video.Id, Seeder.GoshoUsername, newNotes);
            action.Should().Throw<BadRequestError>().WithMessage("The notes you are trying to save are nested deeper the four levels!");
        }

        [Test]
        public void CreateNewItemsReturnsTheRightIdsToUpdateThePageEntries()
        {
            Seeder.SeedPeshoAndGosho(this.context);
            var video = Seeder.SeedVideosToUserWithNotes(context, Seeder.GoshoId);
            var newNotes = Seeder.GenerateNoteCreateSimpleNested(Seeder.preExistingNote1Id);
            Func<int[][]> function = this.GetPartialSaveFunction(video.Id, Seeder.GoshoUsername, newNotes);
            var result = function.Invoke();
            ///This means everything is ok
            result[0][0].Should().Be(0);

            var separatedResult = result.Skip(1).ToList();

            var newNote1DbId = context.VideoNotes.Where(x => x.Content == Seeder.Note1Content).SingleOrDefault().Id;
            var newNote2DbId = context.VideoNotes.Where(x => x.Content == Seeder.Note2Content).SingleOrDefault().Id;
            var newNote3DbId = context.VideoNotes.Where(x => x.Content == Seeder.Note3Content).SingleOrDefault().Id;

            var expectedOutcome = new List<int[]>
            {
                new int[]{ Seeder.Note1InPageId, newNote1DbId },
                new int[]{ Seeder.Note2InPageId, newNote2DbId },
                new int[]{ Seeder.Note3InPageId, newNote3DbId },
            };

            for (int i = 0; i < expectedOutcome.Count; i++)
            {
                var pair = expectedOutcome[i];
                separatedResult.Should().Contain(x=>x[0] == pair[0] && x[1] == pair[1]);
                var ind = separatedResult.FindIndex(x => x[0] == pair[0] && x[1] == pair[1]);
                separatedResult.RemoveAt(ind);
            }
            separatedResult.Should().HaveCount(0);
        }
        #endregion

        #region ModifactionUpdates
        ///A bit of an integration test, bacause the ITrackableService is used here.
        [Test]
        public void PartialSaveRegisterModificationOfVideoIfItIsFinalSave()
        {
            Seeder.SeedPeshoAndGosho(this.context);
            var video = Seeder.SeedVideosToUserWithNotes(context, Seeder.GoshoId);

            var videoLastModifiedOn = video.LastModifiedOn;
            var videoMidificationCount = video.TimesModified;

            Action action = this.GetPartialSaveAction(video.Id, Seeder.GoshoUsername, true);
            action.Invoke();

            var newVideoLastModifiedOn = video.LastModifiedOn;
            var newVideoMidificationCount = video.TimesModified;

            newVideoLastModifiedOn.Value.Should().NotBe(videoLastModifiedOn.Value);
            newVideoMidificationCount.Should().Be(videoMidificationCount + 1);
        }

        [Test]
        public void PartialSaveDoesNotRegisterModificationOfVideoIfItIsNotFinalSave()
        {
            Seeder.SeedPeshoAndGosho(this.context);
            var video = Seeder.SeedVideosToUserWithNotes(context, Seeder.GoshoId);

            var videoLastModifiedOn = video.LastModifiedOn;
            var videoMidificationCount = video.TimesModified;

            Action action = this.GetPartialSaveAction(video.Id, Seeder.GoshoUsername, false);
            action.Invoke();

            var newVideoLastModifiedOn = video.LastModifiedOn;
            var newVideoMidificationCount = video.TimesModified;

            newVideoLastModifiedOn.Value.Should().Be(videoLastModifiedOn.Value);
            newVideoMidificationCount.Should().Be(videoMidificationCount);
        }

        [Test]
        public void RegisterModificationWhenExistingItemsAreChanged()
        {
            const Formatting Note1NewFormatting = Formatting.SQL;
            const Formatting Note2NewFormatting = Formatting.None;
            Seeder.SeedPeshoAndGosho(context);
            var video = Seeder.SeedVideosToUserWithNotes(context, Seeder.GoshoId);

            var note1 = video.Notes.SingleOrDefault(x => x.Id == Seeder.preExistingNote1Id);
            var note2 = video.Notes.SingleOrDefault(x => x.Id == Seeder.preExistingNote2Id);
            var intialModifedOnNote1 = note1.LastModifiedOn;
            var intialModifedOnNote2 = note2.LastModifiedOn;
            var initialTimesModifiedNote1 = note1.TimesModified;
            var initialTimesModifiedNote2 = note2.TimesModified;

            var changes = new string[][]
            {
                new string[]{Seeder.preExistingNote1Id.ToString(), "Formatting", ((int)Note1NewFormatting).ToString() },
                new string[]{Seeder.preExistingNote2Id.ToString(), "Formatting", ((int)Note2NewFormatting).ToString() }
            };
            Action action = GetPartialSaveAction(video.Id, Seeder.GoshoUsername, changes);
            action.Invoke();

            intialModifedOnNote1.Value.Should().BeBefore(note1.LastModifiedOn.Value);
            intialModifedOnNote2.Value.Should().BeBefore(note2.LastModifiedOn.Value);
            initialTimesModifiedNote1.Should().Be(note1.TimesModified - 1);
            initialTimesModifiedNote2.Should().Be(note2.TimesModified - 1);
        }

        #endregion

        #region Helpers 
        private Action GetPartialSaveAction(int videoId, string username)
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

        private Action GetPartialSaveAction(int videoId, string username, VideoNoteCreate[] newNotes)
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

        private Func<int[][]> GetPartialSaveFunction(int videoId, string username, VideoNoteCreate[] newNotes)
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

        private Action GetPartialSaveAction(int videoId, string username, string[][] changes)
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

        private Action GetPartialSaveAction(int videoId, string username, int? seekTo, string name, string description, string url)
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

        private Action GetPartialSaveAction(int videoId, string username, bool finalSave)
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
                finalSave);
        }
        #endregion
    }
}
