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
    using Momento.Services.Models.VideoModels;
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
            Action action = GetPartialSaveAction(NonExistantVideoId, VideoS.GoshoUsername);
            action.Should().Throw<ItemNotFound>().WithMessage("The video you are working on does not exists in the database");
        }

        [Test]
        public void ShouldThrowIfVIdeoDoesNotBelogToUser()
        {
            VideoS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideosToUser(context, VideoS.GoshoId);
            Action action = GetPartialSaveAction(video.Id, VideoS.PeshoUsername);
            action.Should().Throw<AccessDenied>().WithMessage("The video you are trying to medify does not belong to you!");
        }

        [Test]
        public void ShouldThrowIfIdsToChangeDoNotBelongToVideo()
        {
            VideoS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideosToUserWithNotes(context, VideoS.GoshoId);
            var changes = new string[][]
            {
                new string[]{"1", "some prop", "some val" },
                new string[]{"2", "some prop", "some val"},
                ///The existing values are 1 and 2, 3 is not in that video; 
                new string[]{"3", "some prop", "some val"}
            };

            Action action = GetPartialSaveAction(video.Id, VideoS.GoshoUsername, changes);
            action.Should().Throw<AccessDenied>().WithMessage("The video notes you are trying to modify does not belong the the current video");
        }
        #endregion

        #region PartialSaveVideoFields
        [Test]
        public void ShouldNotChangeVideoFieldsIfTheyAreAllNull()
        {
            VideoS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideosToUserWithNotes(context, VideoS.GoshoId);
            Action action = GetPartialSaveAction(video.Id, VideoS.GoshoUsername);
            action.Invoke();

            video.SeekTo.Should().Be(VideoS.DefaultVideoSeekTo);
            video.Name.Should().Be(VideoS.DefaultVideoName);
            video.Description.Should().Be(VideoS.DefaultVideoDesctiption);
            video.Url.Should().Be(VideoS.DefaultVideoUrl);
        }

        [Test]
        public void ShouldChangeVideoFieldsIfTheyAreProvided()
        {
            const int NewSeekTo = 20;
            const string NewName = "NewName";
            const string NewDescription = "NewDescription";
            const string NewUrl = "NewUrl";

            VideoS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideosToUserWithNotes(context, VideoS.GoshoId);
            Action action = GetPartialSaveAction(video.Id, VideoS.GoshoUsername, NewSeekTo, NewName, NewDescription, NewUrl);
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

            VideoS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideosToUserWithNotes(context, VideoS.GoshoId);
            var changes = new string[][]
            {
                new string[]{"1", "Content", Note1NewContent },
                new string[]{"2", "Content", Note2NewContent }
            };
            Action action = GetPartialSaveAction(video.Id, VideoS.GoshoUsername, changes);
            action.Invoke();

            var note1 = video.Notes.SingleOrDefault(x => x.Id == 1);
            var note2 = video.Notes.SingleOrDefault(x => x.Id == 2);

            note1.Content.Should().Be(Note1NewContent);
            note2.Content.Should().Be(Note2NewContent);
        }

        [Test]
        public void AppliesDeletedChangeToExistingNotesShouldSoftDeleteThem()
        {
            VideoS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideosToUserWithNotes(context, VideoS.GoshoId);
            var changes = new string[][]
            {
                new string[]{"1", "Deleted", "Not Used"},
                new string[]{"2", "Deleted", "Not Used"}
            };
            Action action = GetPartialSaveAction(video.Id, VideoS.GoshoUsername, changes);
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

            VideoS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideosToUserWithNotes(context, VideoS.GoshoId);
            var changes = new string[][]
            {
                new string[]{"1", "Formatting", ((int)Note1NewFormatting).ToString() },
                new string[]{"2", "Formatting", ((int)Note2NewFormatting).ToString() }
            };
            Action action = GetPartialSaveAction(video.Id, VideoS.GoshoUsername, changes);
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

            VideoS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideosToUserWithNotes(context, VideoS.GoshoId);
            var changes = new string[][]
            {
                new string[]{"1", "SeekTo", Note1NewSeekTo.ToString() },
                new string[]{"2", "SeekTo", Note2NewSeekTo.ToString() }
            };
            Action action = GetPartialSaveAction(video.Id, VideoS.GoshoUsername, changes);
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
            VideoS.SeedPeshoAndGosho(this.context);
            var video = VideoS.SeedVideosToUserWithNotes(context, VideoS.GoshoId);
            var newNotes = VideoS.GenerateNoteCreateSimpleNested(null);
            Action action = GetPartialSaveAction(video.Id, VideoS.GoshoUsername, newNotes);
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
            VideoS.SeedPeshoAndGosho(this.context);
            var video = VideoS.SeedVideosToUserWithNotes(context, VideoS.GoshoId);
            var newNotes = VideoS.GenerateNoteCreateSimpleNested(VideoS.preExistingNote1Id);
            Action action = this.GetPartialSaveAction(video.Id, VideoS.GoshoUsername, newNotes);
            action.Invoke();

            var preExistingRootNote = context.VideoNotes.SingleOrDefault(x => x.Id == VideoS.preExistingNote1Id);
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
            VideoS.SeedPeshoAndGosho(this.context);
            var video = VideoS.SeedVideosToUserWithNotes(context, VideoS.GoshoId);
            var newNotes = VideoS.GenerateNoteCreateSimpleNested(VideoS.preExistingNote1Id, 4);
            Action action = this.GetPartialSaveAction(video.Id, VideoS.GoshoUsername, newNotes);
            action.Should().Throw<BadRequestError>().WithMessage("The notes you are trying to save are nested deeper the four levels!");
        }

        [Test]
        public void CreateNewItemsReturnsTheRightIdsToUpdateThePageEntries()
        {
            VideoS.SeedPeshoAndGosho(this.context);
            var video = VideoS.SeedVideosToUserWithNotes(context, VideoS.GoshoId);
            var newNotes = VideoS.GenerateNoteCreateSimpleNested(VideoS.preExistingNote1Id);
            Func<int[][]> function = this.GetPartialSaveFunction(video.Id, VideoS.GoshoUsername, newNotes);
            var result = function.Invoke();
            ///This means everything is ok
            result[0][0].Should().Be(0);

            var separatedResult = result.Skip(1).ToList();

            var newNote1DbId = context.VideoNotes.Where(x => x.Content == VideoS.Note1Content).SingleOrDefault().Id;
            var newNote2DbId = context.VideoNotes.Where(x => x.Content == VideoS.Note2Content).SingleOrDefault().Id;
            var newNote3DbId = context.VideoNotes.Where(x => x.Content == VideoS.Note3Content).SingleOrDefault().Id;

            var expectedOutcome = new List<int[]>
            {
                new int[]{ VideoS.Note1InPageId, newNote1DbId },
                new int[]{ VideoS.Note2InPageId, newNote2DbId },
                new int[]{ VideoS.Note3InPageId, newNote3DbId },
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
            VideoS.SeedPeshoAndGosho(this.context);
            var video = VideoS.SeedVideosToUserWithNotes(context, VideoS.GoshoId);

            var videoLastModifiedOn = video.LastModifiedOn;
            var videoMidificationCount = video.TimesModified;

            Action action = this.GetPartialSaveAction(video.Id, VideoS.GoshoUsername, true);
            action.Invoke();

            var newVideoLastModifiedOn = video.LastModifiedOn;
            var newVideoMidificationCount = video.TimesModified;

            newVideoLastModifiedOn.Value.Should().NotBe(videoLastModifiedOn.Value);
            newVideoMidificationCount.Should().Be(videoMidificationCount + 1);
        }

        [Test]
        public void PartialSaveDoesNotRegisterModificationOfVideoIfItIsNotFinalSave()
        {
            VideoS.SeedPeshoAndGosho(this.context);
            var video = VideoS.SeedVideosToUserWithNotes(context, VideoS.GoshoId);

            var videoLastModifiedOn = video.LastModifiedOn;
            var videoMidificationCount = video.TimesModified;

            Action action = this.GetPartialSaveAction(video.Id, VideoS.GoshoUsername, false);
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
            VideoS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideosToUserWithNotes(context, VideoS.GoshoId);

            var note1 = video.Notes.SingleOrDefault(x => x.Id == VideoS.preExistingNote1Id);
            var note2 = video.Notes.SingleOrDefault(x => x.Id == VideoS.preExistingNote2Id);
            var intialModifedOnNote1 = note1.LastModifiedOn;
            var intialModifedOnNote2 = note2.LastModifiedOn;
            var initialTimesModifiedNote1 = note1.TimesModified;
            var initialTimesModifiedNote2 = note2.TimesModified;

            var changes = new string[][]
            {
                new string[]{VideoS.preExistingNote1Id.ToString(), "Formatting", ((int)Note1NewFormatting).ToString() },
                new string[]{VideoS.preExistingNote2Id.ToString(), "Formatting", ((int)Note2NewFormatting).ToString() }
            };
            Action action = GetPartialSaveAction(video.Id, VideoS.GoshoUsername, changes);
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
