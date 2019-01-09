namespace Momento.Tests.Tests.VideoServiceTests
{
    #region Initialization
    using AutoMapper;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Momento.Models.Enums;
    using Momento.Services.Contracts.Video;
    using Momento.Services.Exceptions;
    using Momento.Services.Implementations.Shared;
    using Momento.Services.Implementations.Video;
    using Momento.Services.Models.VideoModels;
    using Momento.Tests.Contracts;
    using Momento.Tests.Seeding;
    using Momento.Tests.Utilities;
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

            var trackableService = new TrackableService(this.context);

            this.videoService = new VideoService(
                    this.context,
                    trackableService);
        }
        #endregion

        #region ValidateSaveAndRegisterModification
        [Test]
        public void ShouldThrowIfVIdeoIsNotFound()
        {
            const int NonExistantVideoId = 12;

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = GetPartialSaveAction(NonExistantVideoId, UserS.GoshoUsername);

            action.Should().Throw<ItemNotFound>().WithMessage("The video you are working on does not exists in the database");
        }

        [Test]
        public void ShouldThrowIfVIdeoDoesNotBelogToUser()
        {
            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideosToUser(context, UserS.GoshoId);

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = GetPartialSaveAction(video.Id, UserS.PeshoUsername);

            action.Should().Throw<AccessDenied>().WithMessage("The video you are trying to modify does not belong to you!");
        }

        [Test]
        public void ShouldThrowIfIdsToChangeDoNotBelongToVideo()
        {
            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);
            var changes = new string[][]
            {
                new string[]{"1", "some prop", "some val" },
                new string[]{"2", "some prop", "some val"},
                ///The existing values are 1 and 2, 3 is not in that video; 
                new string[]{"3", "some prop", "some val"}
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = GetPartialSaveAction(video.Id, UserS.GoshoUsername, changes);

            action.Should().Throw<AccessDenied>().WithMessage("The video notes you are trying to modify does not belong the the current video");
        }
        #endregion

        #region PartialSaveVideoFields
        [Test]
        public void ShouldNotChangeVideoFieldsIfTheyAreAllNull()
        {
            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = GetPartialSaveAction(video.Id, UserS.GoshoUsername);
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

            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = GetPartialSaveAction(video.Id, UserS.GoshoUsername, NewSeekTo, NewName, NewDescription);
            action.Invoke();

            video = context.Videos
                .SingleOrDefault(x => x.Id == video.Id);

            video.SeekTo.Should().Be(NewSeekTo);
            video.Name.Should().Be(NewName);
            video.Description.Should().Be(NewDescription);
        }
        #endregion

        #region PartialSaveChangesToExistingNote
        ///Properties to track:
        ///Deleted X; Content X; Formatting X; seekTo X; Type; BorderColor; BorderThickness
        [Test]///Checked
        public void AppliesContentChangesToExistingNotesCorrectly()
        {
            const string Note1NewContent = "newContentForNote1";
            const string Note2NewContent = "newContentForNote2";

            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);
            var changes = new string[][]
            {
                new string[]{"1", "Content", Note1NewContent },
                new string[]{"2", "Content", Note2NewContent }
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = GetPartialSaveAction(video.Id, UserS.GoshoUsername, changes);
            action.Invoke();

            video = context.Videos
                .Include(x => x.Notes)
                .SingleOrDefault(x => x.Id == video.Id);

            var note1 = video.Notes.SingleOrDefault(x => x.Id == 1);
            var note2 = video.Notes.SingleOrDefault(x => x.Id == 2);

            note1.Content.Should().Be(Note1NewContent);
            note2.Content.Should().Be(Note2NewContent);
        }

        [Test]///Checked
        public void AppliesDeletedChangeToExistingNotesShouldSoftDeleteThem()
        {
            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);
            var changes = new string[][]
            {
                new string[]{"1", "Deleted", "Not Used"},
                new string[]{"2", "Deleted", "Not Used"}
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = GetPartialSaveAction(video.Id, UserS.GoshoUsername, changes);
            action.Invoke();

            video = context.Videos
                .Include(x => x.Notes)
                .SingleOrDefault(x => x.Id == video.Id);

            var note1 = video.Notes.SingleOrDefault(x => x.Id == 1);
            var note2 = video.Notes.SingleOrDefault(x => x.Id == 2);

            note1.IsDeleted.Should().Be(true);
            note2.IsDeleted.Should().Be(true);
        }

        [Test]///Checked
        public void AppliesFormattingChangesToExistingNotesCorrectly()
        {
            const Formatting Note1NewFormatting = Formatting.SQL;
            const Formatting Note2NewFormatting = Formatting.None;

            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);
            var changes = new string[][]
            {
                new string[]{"1", "Formatting", ((int)Note1NewFormatting).ToString() },
                new string[]{"2", "Formatting", ((int)Note2NewFormatting).ToString() }
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = GetPartialSaveAction(video.Id, UserS.GoshoUsername, changes);
            action.Invoke();

            video = context.Videos
                .Include(x => x.Notes)
                .SingleOrDefault(x => x.Id == video.Id);

            var note1 = video.Notes.SingleOrDefault(x => x.Id == 1);
            var note2 = video.Notes.SingleOrDefault(x => x.Id == 2);

            note1.Formatting.Should().Be(Note1NewFormatting);
            note2.Formatting.Should().Be(Note2NewFormatting);
        }

        [Test]///Checked
        public void AppliesSeekToChangesToExistingNotesCorrectly()
        {
            const int Note1NewSeekTo = 15;
            const int Note2NewSeekTo = 16;

            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);
            var changes = new string[][]
            {
                new string[]{"1", "SeekTo", Note1NewSeekTo.ToString() },
                new string[]{"2", "SeekTo", Note2NewSeekTo.ToString() }
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = GetPartialSaveAction(video.Id, UserS.GoshoUsername, changes);
            action.Invoke();

            video = context.Videos
                .Include(x => x.Notes)
                .SingleOrDefault(x => x.Id == video.Id);

            var note1 = video.Notes.SingleOrDefault(x => x.Id == 1);
            var note2 = video.Notes.SingleOrDefault(x => x.Id == 2);

            note1.SeekTo.Should().Be(Note1NewSeekTo);
            note2.SeekTo.Should().Be(Note2NewSeekTo);
        }

        [Test]///Checked
        public void Applies_Type_BorderThickness_BorderColor_ChangesToExistingNotesCorrectly()
        {
            const VideoNoteType note1NewType = VideoNoteType.TimeStamp;
            const VideoNoteType note2NewType = VideoNoteType.Topic;
            const string note1NewBorderColor = "red";
            const string note2NewBorderColor = "purple";
            const int note1NewBorderThickness = 15;
            const int note2NewBorderThickness = 1;

            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);
            var changes = new string[][]
            {
                new string[]{"1", "type", ((int)note1NewType).ToString() },
                new string[]{"2", "type", ((int)note2NewType).ToString() },
                new string[]{"1", "borderThickness", note1NewBorderThickness.ToString() },
                new string[]{"2", "borderThickness", note2NewBorderThickness.ToString() },
                new string[]{"1", "borderColor", note1NewBorderColor },
                new string[]{"2", "borderColor", note2NewBorderColor },
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = GetPartialSaveAction(video.Id, UserS.GoshoUsername, changes);
            action.Invoke();

            video = context.Videos
                .Include(x => x.Notes)
                .SingleOrDefault(x => x.Id == video.Id);

            var note1 = video.Notes.SingleOrDefault(x => x.Id == 1);
            var note2 = video.Notes.SingleOrDefault(x => x.Id == 2);

            note1.Type.Should().Be(note1NewType);
            note2.Type.Should().Be(note2NewType);
            note1.BorderColor.Should().Be(note1NewBorderColor);
            note2.BorderColor.Should().Be(note2NewBorderColor);
            note1.BorderThickness.Should().Be(note1NewBorderThickness);
            note2.BorderThickness.Should().Be(note2NewBorderThickness);
        }
        #endregion

        #region PartialSaveNewNotes

        [Test]///Checked
        public void ShouldSaveAllFieldsOfANewNoteCorectly()
        {
            const string borderColor = "Some great color!";
            const int borderThickness = 12;
            const string content = "Some great content";
            const Formatting formatting = Formatting.CSharp;
            const int seekTo = 42;
            const VideoNoteType type = VideoNoteType.TimeStamp;
            const int level = 1;  

            UserS.SeedPeshoAndGosho(this.context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);

            var newNote = new VideoNoteCreate
            {
                Id = 0,
                BorderColor = borderColor,
                BorderThickness = borderThickness,
                Content = content,
                Formatting = formatting,
                SeekTo = seekTo,
                Type = type,
                Level = level,

                ParentDbId = -1,
                ///Not in use currently
                TextColor = "",
                Deleted = false,
                BackgroundColor = "",
                InPageId = 0,
                InPageParentId = 0,
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = GetPartialSaveAction(video.Id, UserS.GoshoUsername,new VideoNoteCreate[] { newNote });
            action.Invoke();

            video = context.Videos
                .Include(x => x.Notes)
                .SingleOrDefault(x => x.Id == video.Id);

            var allNotes = video.Notes;

            var note = video.Notes.SingleOrDefault(x=>x.Id != VideoS.preExistingNote1Id && x.Id != VideoS.preExistingNote2Id);

            note.BorderColor.Should().Be(borderColor);
            note.BorderThickness.Should().Be(borderThickness);
            note.Content.Should().Be(content);
            note.Formatting.Should().Be(formatting);
            note.SeekTo.Should().Be(seekTo);
            note.Type.Should().Be(type);
            note.Level.Should().Be(level);
        }

        [Test]///Checked
        public void ShoudCreate3nestedNotesStartingAtRootCorrectly()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);
            var newNotes = VideoS.GenerateNoteCreateSimpleNested(null);

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = GetPartialSaveAction(video.Id, UserS.GoshoUsername, newNotes);
            action.Invoke();

            video = context.Videos
                .Include(x => x.Notes)
                .SingleOrDefault(x => x.Id == video.Id);

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

        [Test]///Checked
        public void ShoudCreate3nestedNotesStartingAtExistingNote()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);
            var newNotes = VideoS.GenerateNoteCreateSimpleNested(VideoS.preExistingNote1Id);

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = this.GetPartialSaveAction(video.Id, UserS.GoshoUsername, newNotes);
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

            video = context.Videos
                .Include(x => x.Notes)
                .SingleOrDefault(x => x.Id == video.Id);

            video.Notes.Count.Should().Be(5);
        }

        [Test]
        public void IfTheNestingLevelIsGreaterThan4Throw()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);
            var newNotes = VideoS.GenerateNoteCreateSimpleNested(VideoS.preExistingNote1Id, 4);

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = this.GetPartialSaveAction(video.Id, UserS.GoshoUsername, newNotes);

            action.Should().Throw<BadRequestError>().WithMessage("The notes you are trying to save are nested deeper the four levels!");
        }

        [Test]
        public void CreateNewItemsReturnsTheRightIdsToUpdateThePageEntries()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);
            var newNotes = VideoS.GenerateNoteCreateSimpleNested(VideoS.preExistingNote1Id);

            ChangeTrackerOperations.DetachAll(this.context);
            Func<int[][]> function = this.GetPartialSaveFunction(video.Id, UserS.GoshoUsername, newNotes);
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
                separatedResult.Should().Contain(x => x[0] == pair[0] && x[1] == pair[1]);
                var ind = separatedResult.FindIndex(x => x[0] == pair[0] && x[1] == pair[1]);
                separatedResult.RemoveAt(ind);
            }
            separatedResult.Should().HaveCount(0);
        }
        #endregion

        #region ModifactionUpdates
        [Test]///Cheked
        public void PartialSaveRegisterModificationOfVideoIfItIsFinalSave()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);

            var videoLastModifiedOn = video.LastModifiedOn;
            var videoMidificationCount = video.TimesModified;


            ChangeTrackerOperations.DetachAll(this.context);
            Action action = this.GetPartialSaveAction(video.Id, UserS.GoshoUsername, true);
            action.Invoke();

            video = context.Videos
                .Include(x => x.Notes)
                .SingleOrDefault(x => x.Id == video.Id);

            var newVideoLastModifiedOn = video.LastModifiedOn;
            var newVideoMidificationCount = video.TimesModified;

            newVideoLastModifiedOn.Value.Should().NotBe(videoLastModifiedOn.Value);
            newVideoMidificationCount.Should().Be(videoMidificationCount + 1);
        }

        [Test]
        public void PartialSaveDoesNotRegisterModificationOfVideoIfItIsNotFinalSave()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);

            var videoLastModifiedOn = video.LastModifiedOn;
            var videoMidificationCount = video.TimesModified;


            ChangeTrackerOperations.DetachAll(this.context);
            Action action = this.GetPartialSaveAction(video.Id, UserS.GoshoUsername, false);
            action.Invoke();

            var newVideoLastModifiedOn = video.LastModifiedOn;
            var newVideoMidificationCount = video.TimesModified;

            newVideoLastModifiedOn.Value.Should().Be(videoLastModifiedOn.Value);
            newVideoMidificationCount.Should().Be(videoMidificationCount);
        }

        [Test]///Checked
        public void RegisterModificationWhenExistingItemsAreChanged()
        {
            const Formatting Note1NewFormatting = Formatting.SQL;
            const Formatting Note2NewFormatting = Formatting.None;
            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);

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

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = GetPartialSaveAction(video.Id, UserS.GoshoUsername, changes);
            action.Invoke();

            video = context.Videos
                .Include(x => x.Notes)
                .SingleOrDefault(x => x.Id == video.Id);

            note1 = video.Notes.SingleOrDefault(x => x.Id == VideoS.preExistingNote1Id);
            note2 = video.Notes.SingleOrDefault(x => x.Id == VideoS.preExistingNote2Id);

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
                changes,
                new VideoNoteCreate[] { },
                false);
        }

        private Action GetPartialSaveAction(int videoId, string username, int? seekTo, string name, string description)
        {
            return () => this.videoService.PartialSave(
                videoId,
                username,
                seekTo,
                name,
                description,
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
                new string[][] { },
                new VideoNoteCreate[] { },
                finalSave);
        }
        #endregion
    }
}
