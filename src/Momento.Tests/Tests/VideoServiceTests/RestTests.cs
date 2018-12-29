namespace Momento.Tests.Tests.VideoServiceTests
{
    #region Initialization
    using AutoMapper;
    using FluentAssertions;
    using Momento.Models.Videos;
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

    public class RestTests : BaseTestsSqliteInMemory
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

        #region GetView
        [Test]///Checked
        public void GetViewThrowsIfVideoNotFound()
        {
            const int NonExistantVideoId = 42;

            Action action = () => this.videoService.GetView(NonExistantVideoId);
            action.Should().Throw<ItemNotFound>().WithMessage("Video you are trying to view does not exist in the database!");
        }

        [Test]///Checked
        public void GetViewShouldReturnCorrectView()
        {
            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideosToUserWithNotes(context, UserS.PeshoId);
            var resultView = this.videoService.GetView(video.Id);
            var expectedResultView = new VideoView
            {
                Description = video.Description,
                Name = video.Name,
                Url = video.Url,
                Notes = video.Notes.Select(x => MapVideoNoteToView(x)).ToList()
            };

            resultView.Should().BeEquivalentTo(expectedResultView);
        }

        [Test]///Checked
        public void GetViewShouldReturnCorrectViewWithNestedNotes()
        {
            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideosToUserWithNotes(context, UserS.PeshoId, true);
            var resultView = this.videoService.GetView(video.Id);
            var expectedResultView = new VideoView
            {
                Description = video.Description,
                Name = video.Name,
                Url = video.Url,
                ///We ignore the notes that are also nested in parent notes
                Notes = video.Notes.Where(x => x.Note == null).Select(x => MapVideoNoteToView(x)).ToList()
            };

            resultView.Should().BeEquivalentTo(expectedResultView);
        }
        #endregion

        #region Create
        ///A little iffy how to test that it returns the right index 
        [Test]///Checked
        public void CreateThrowsIfDirectoryDoesNotExist()
        {
            const int NonExistantDirectoryId = 13;
            const string NonExistantUsername = "NonExistantUsername";

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.videoService.Create(NonExistantDirectoryId, NonExistantUsername);

            action.Should().Throw<ItemNotFound>().WithMessage("The Directory you selected for creating the new video notes in, does not exist!");
        }

        [Test]///Checked
        public void CreateThrowsIfUserDoesNotExist()
        {
            UserS.SeedPeshoAndGosho(context);

            int ExistingDirecotryId = UserS.GoshoRootDirId;
            const string NonExistantUsername = "NonExistantUsername";

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.videoService.Create(ExistingDirecotryId, NonExistantUsername);
            action.Should().Throw<UserNotFound>();
        }

        [Test]///Checked
        public void CreateThrowsIfDirectoryDoesNotBelongToUser()
        {
            UserS.SeedPeshoAndGosho(context);

            int ExistingDirecotryId = UserS.GoshoRootDirId;
            string ExistingUsername = UserS.PeshoUsername;

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.videoService.Create(ExistingDirecotryId, ExistingUsername);
            action.Should().Throw<AccessDenied>().WithMessage("The directory you are trying to create a video on does note belong to you!");
        }

        [Test]///Checked
        public void CreateCreatsVideo()
        {
            ///Also seeds their root directories
            UserS.SeedPeshoAndGosho(context);

            int ExistingDirecotryId = UserS.GoshoRootDirId;
            string ExistingUsername = UserS.GoshoUsername;

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.videoService.Create(ExistingDirecotryId, ExistingUsername);
            action.Invoke();

            var video = context.Directories
                .SingleOrDefault(x => x.Id == ExistingDirecotryId)
                .Videos
                .SingleOrDefault();

            video.DirectoryId.Should().Be(ExistingDirecotryId);
            video.UserId.Should().Be(UserS.GoshoId);
        }

        [Test]///Checked
        public void CreateReturnsTheVideosId()
        {
            ///Also seeds their root directories
            UserS.SeedPeshoAndGosho(context);

            int ExistingDirecotryId = UserS.GoshoRootDirId;
            string ExistingUsername = UserS.GoshoUsername;

            Func<int> funk = () => this.videoService.Create(ExistingDirecotryId, ExistingUsername);
            var videoId = funk.Invoke();

            var video = context.Directories
                .SingleOrDefault(x => x.Id == ExistingDirecotryId)
                .Videos
                .SingleOrDefault();

            videoId.Should().Be(video.Id);
        }

        [Test]///Checked
        public void CreateSetNewVidosOrderTo0WhenItIsTheFirstVideoInAdirectory()
        {
            ///Also seeds their root directories
            UserS.SeedPeshoAndGosho(context);
            int ExistingDirecotryId = UserS.GoshoRootDirId;
            string ExistingUsername = UserS.GoshoUsername;

            Func<int> funk = () => this.videoService.Create(ExistingDirecotryId, ExistingUsername);
            var videoId = funk.Invoke();

            var video = context.Videos.SingleOrDefault(x => x.Id == videoId);
            video.Order.Should().Be(0);
        }

        [Test]///Checked
        public void CreateSetNewVidosOrderToTheCountOfVideosMinusOne()
        {
            UserS.SeedPeshoAndGosho(context);
            int ExistingDirecotryId = UserS.GoshoRootDirId;
            string ExistingUsername = UserS.GoshoUsername;

            Func<int> funk = () => this.videoService.Create(ExistingDirecotryId, ExistingUsername);
            funk.Invoke();
            funk.Invoke();
            funk.Invoke();
            funk.Invoke();
            var videoId = funk.Invoke();

            var video = context.Videos.SingleOrDefault(x => x.Id == videoId);
            video.Order.Should().Be(4);
        }
        #endregion

        #region GetVideoForEdit 
        [Test]///Checked
        public void GetVideoForEditShouldThrowIfVideoIsNotFound()
        {
            var nonExistantVideoId = 42;
            UserS.SeedPeshoAndGosho(context);
            Func<VideoCreate> function = () => this.videoService.GetVideoForEdit(nonExistantVideoId, UserS.GoshoUsername);
            function.Should().Throw<ItemNotFound>().WithMessage("The video you are trying to edit does not exist!");
        }

        [Test]///Checked
        public void GetVideoForEditShouldThrowIfUserIsNotFound()
        {
            UserS.SeedPeshoAndGosho(context);
            var nonExistantUsername = "Gosho420";

            var video = VideoS.SeedVideosToUserWithNotes(context, UserS.GoshoId);
            Func<VideoCreate> function = () => this.videoService.GetVideoForEdit(video.Id, nonExistantUsername);
            function.Should().Throw<UserNotFound>();
        }

        [Test]///Checked
        public void GetVideoForEditShouldThrowIfVideoDoesNoteBelongToUser()
        {
            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideosToUserWithNotes(context, UserS.GoshoId);
            Func<VideoCreate> function = () => this.videoService.GetVideoForEdit(video.Id, UserS.PeshoUsername);
            function.Should().Throw<AccessDenied>().WithMessage("You can note edit video that does not belong to you!");
        }

        [Test]///Checked
        public void GetVideoForEditShouldReturnCorrectResult()
        {
            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideosToUserWithNotes(context, UserS.GoshoId, true);
            Func<VideoCreate> function = () => this.videoService.GetVideoForEdit(video.Id, UserS.GoshoUsername);
            var result = function.Invoke();

            var dbNote0 = video.Notes.SingleOrDefault(x=>x.Order == 0);
            var dbNote1 = video.Notes.SingleOrDefault(x => x.Order == 1);
            var dbNote2 = video.Notes.SingleOrDefault(x => x.Order == 2);

            var pageNote0 = MapVideoNoteToNoteCreate(dbNote0);
            var pageNote1 = MapVideoNoteToNoteCreate(dbNote1);
            var pageNote2 = MapVideoNoteToNoteCreate(dbNote2);

            pageNote0.Level = 1;
            pageNote1.Level = 2;
            pageNote2.Level = 1;

            pageNote1.InPageParentId = pageNote0.InPageId;

            var expectedResult = new VideoCreate
            {
                Description = video.Description,
                DirectoryId = video.DirectoryId,
                Id = video.Id,
                Name = video.Name,
                Order = video.Order,
                SeekTo = video.SeekTo,
                Url = video.Url,
                Notes = new List<VideoNoteCreate> { pageNote0, pageNote1, pageNote2 }
            };

            result.Should().BeEquivalentTo(expectedResult);
        }
        #endregion

        #region Delete
        [Test]///Checked
        public void DeleteShouldThrowIfVideIsNotFound()
        {
            var nonExistantVideoId = 42;
            UserS.SeedPeshoAndGosho(context);
            Action action = () => this.videoService.Delete(nonExistantVideoId, UserS.GoshoUsername, DateTime.UtcNow);
            action.Should().Throw<ItemNotFound>().WithMessage("The video you are trying to delete does not exist!");
        }

        [Test]///Checked
        public void DeleteShouldThrowIfUserIsNotFound()
        {
            UserS.SeedPeshoAndGosho(context);
            var nonExistantUsername = "Gosho420";

            var video = VideoS.SeedVideosToUserWithNotes(context, UserS.GoshoId);
            Action action = () => this.videoService.Delete(video.Id, nonExistantUsername, DateTime.UtcNow);
            action.Should().Throw<UserNotFound>();

        }

        [Test]///Checked
        public void DeleteShouldThrowIfVideoDoesNoteBelongToUser()
        {
            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideosToUserWithNotes(context, UserS.GoshoId);
            Action action = () => this.videoService.Delete(video.Id, UserS.PeshoUsername, DateTime.UtcNow);
            action.Should().Throw<AccessDenied>().WithMessage("The video you are trying to delete does not belong to you!");
        }

        [Test]///Checked
        public void DeleteShouldSoftDeleteTheVideoAndAllItsNotes()
        {
            var now = DateTime.Now;
            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideosToUserWithNotes(context, UserS.GoshoId, true);
            Action action = () => this.videoService.Delete(video.Id, UserS.GoshoUsername, now);
            action.Invoke();
            video.IsDeleted.Should().Be(true);
            video.DeletedOn.Should().Be(now);
            video.Notes.Select(x => x.IsDeleted).Should().AllBeEquivalentTo(true);
            video.Notes.Select(x => x.DeletedOn).Should().AllBeEquivalentTo(now);
        }
        #endregion

        #region Helpers 
        private VideoNoteView MapVideoNoteToView(VideoNote note)
        {
            return new VideoNoteView()
            {
                ChildNotes = note.ChildNotes.Select(x => MapVideoNoteToView(x)).ToHashSet(),
                Content = note.Content,
                Formatting = note.Formatting,
                Id = note.Id,
                Level = note.Level,
                SeekTo = note.SeekTo,
            };
        }

        private VideoNoteCreate MapVideoNoteToNoteCreate (VideoNote note)
        {
            return new VideoNoteCreate
            {
                 Content = note.Content,
                 Formatting = note.Formatting,
                 Id = note.Id,
                 SeekTo = note.SeekTo,
                 Type = note.Type,
                 InPageId = note.Order,
            };
        }
        #endregion
    }
}
