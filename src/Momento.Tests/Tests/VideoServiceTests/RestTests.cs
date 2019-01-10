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
        public void GetViewThrowsIfUserNotFound()
        {
            UserS.SeedPeshoAndGosho(this.context);
            const string NonExistantUserName = "Not Pesho";
            const int NonExistantVideoId = 42;

            Action action = () => this.videoService.GetView(NonExistantVideoId, NonExistantUserName);
            action.Should().Throw<UserNotFound>();
        }

        [Test]///Checked
        public void GetViewThrowsIfVideoNotFound()
        {
            UserS.SeedPeshoAndGosho(this.context);
            const int NonExistantVideoId = 42;

            Action action = () => this.videoService.GetView(NonExistantVideoId,UserS.GoshoUsername);
            action.Should().Throw<ItemNotFound>().WithMessage("Video you are trying to view does not exist in the database!");
        }

        [Test]
        public void GetViewThrowsIfVideoNotesUserIsNotTheCurrentUser()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.PeshoId);

            Action action = () => this.videoService.GetView(video.Id, UserS.GoshoUsername);
            action.Should().Throw<AccessDenied>().WithMessage("The video you are trying to view does not belong to you");
        }

        [Test]///Checked
        public void GetViewShouldReturnCorrectView()
        {
            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.PeshoId);
            var expectedResultView = new VideoView
            {
                Description = video.Description,
                Name = video.Name,
                Url = video.Url,
                Notes = video.Notes.Select(x => MapVideoNoteToView(x)).ToList()
            };

            var resultView = this.videoService.GetView(video.Id, UserS.PeshoUsername);

            resultView.Should().BeEquivalentTo(expectedResultView);
        }

        [Test]
        public void GetViewShouldReturnCorrectViewWithNestedNotes()
        {
            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.PeshoId, true);
            var resultView = this.videoService.GetView(video.Id, UserS.PeshoUsername);
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

        [Test]
        public void GetViewApiShouldReturnCorrectViewWithNestedNotes()
        {
            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.PeshoId, true);
            var resultView = this.videoService.GetViewApi(video.Id, UserS.PeshoUsername);
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

        [Test]
        public void GetViewApiReturnsNullIfVideoNotesUserIsNotTheCurrentUser()
        {
            UserS.SeedPeshoAndGosho(this.context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.PeshoId);

            Func<VideoView> action = () => this.videoService.GetViewApi(video.Id, UserS.GoshoUsername);
            var result = action.Invoke();
            result.Should().Be(null);
        }
        #endregion

        #region Create
        [Test]///Checked
        public void CreateThrowsIfUserDoesNotExist()
        {
            UserS.SeedPeshoAndGosho(context);

            int ExistingDirecotryId = UserS.GoshoRootDirId;
            const string NonExistantUsername = "NonExistantUsername";

            ///This data does not matter for this test
            var initCreate = new VideoInitialCreate
            {
                Description = "",
                DirectoryId = 42,
                Name = "",
                Url = "",
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.videoService.Create(initCreate, NonExistantUsername);
            action.Should().Throw<UserNotFound>();
        }

        [Test]///Checked
        public void CreateThrowsIfDirectoryDoesNotExist()
        {
            const int nonExistantDirectoryId = 42;
            const string NonExistantUsername = "NonExistantUsername";

            UserS.SeedPeshoAndGosho(this.context);

            var initCreate = new VideoInitialCreate
            {
                 Description = "",
                 DirectoryId = nonExistantDirectoryId,
                 Name = "",
                 Url = "",
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.videoService.Create(initCreate, UserS.PeshoUsername);

            action.Should().Throw<ItemNotFound>().WithMessage("The Directory you selected for creating the new video notes in, does not exist!");
        }

        [Test]///Checked
        public void CreateThrowsIfDirectoryDoesNotBelongToUser()
        {
            UserS.SeedPeshoAndGosho(context);

            var initCreate = new VideoInitialCreate
            {
                Description = "",
                DirectoryId = UserS.GoshoRootDirId,
                Name = "",
                Url = "",
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Action action = () => this.videoService.Create(initCreate, UserS.PeshoUsername);
            action.Should().Throw<AccessDenied>().WithMessage("The directory you are trying to create a video on does note belong to you!");
        }

        [Test]///Checked
        public void CreateCreatsVideo()
        {
            const string initialDescription = "best description ever!";
            const string initialName = "best name ever";
            const string initialUrl = "definitely a valid url, who is asking?";

            ///Also seeds their root directories
            UserS.SeedPeshoAndGosho(context);

            var initCreate = new VideoInitialCreate
            {
                DirectoryId = UserS.GoshoRootDirId,

                Description = initialDescription,
                Name = initialName,
                Url = initialUrl,
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Func<Video> action = () => this.videoService.Create(initCreate, UserS.GoshoUsername);
            var video = action.Invoke();

            video.DirectoryId.Should().Be(UserS.GoshoRootDirId);
            video.UserId.Should().Be(UserS.GoshoId);

            video.Description.Should().Be(initialDescription);
            video.Name.Should().Be(initialName);
            video.Url.Should().Be(initialUrl);
        }

        [Test]///Checked
        public void CreateSetNewVidosOrderTo0WhenItIsTheFirstVideoInAdirectory()
        {
            ///Also seeds their root directories
            UserS.SeedPeshoAndGosho(context);

            var initCreate = new VideoInitialCreate
            {
                DirectoryId = UserS.GoshoRootDirId,

                Description = "",
                Name = "",
                Url = "",
            };

            Func<Video> funk = () => this.videoService.Create(initCreate, UserS.GoshoUsername);
            var video = funk.Invoke();

            video.Order.Should().Be(0);
        }

        [Test]///Checked
        public void CreateSetNewVideosOrderToTheCountOfVideosMinusOne()
        {
            ///Also seeds their root directories
            UserS.SeedPeshoAndGosho(context);

            var initCreate = new VideoInitialCreate
            {
                DirectoryId = UserS.GoshoRootDirId,

                Description = "",
                Name = "",
                Url = "",
            };

            Func<Video> funk = () => this.videoService.Create(initCreate, UserS.GoshoUsername);
            funk.Invoke();
            funk.Invoke();
            funk.Invoke();
            funk.Invoke();
            var video = funk.Invoke();

            video.Order.Should().Be(4);
        }

        [Test]///Checked
        public void CreateApiTeturnsFlaseIfDirectoryDoesNotExist()
        {
            const int nonExistantDirectoryId = 42;
            const string NonExistantUsername = "NonExistantUsername";

            UserS.SeedPeshoAndGosho(this.context);

            var initCreate = new VideoInitialCreate
            {
                Description = "",
                DirectoryId = nonExistantDirectoryId,
                Name = "",
                Url = "",
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Func<bool> action = () => this.videoService.CreateApi(initCreate, UserS.PeshoUsername);
            var result = action.Invoke();
            result.Should().Be(false);
        }
        [Test]///TODO fix this up
        public void CreateApiCreatsVideo()
        {
            const string initialDescription = "best description ever!";
            const string initialName = "best name ever";
            const string initialUrl = "definitely a valid url, who is asking?";

            ///Also seeds their root directories
            UserS.SeedPeshoAndGosho(context);

            var initCreate = new VideoInitialCreate
            {
                DirectoryId = UserS.GoshoRootDirId,

                Description = initialDescription,
                Name = initialName,
                Url = initialUrl,
            };

            ChangeTrackerOperations.DetachAll(this.context);
            Func<bool> action = () => this.videoService.CreateApi(initCreate, UserS.GoshoUsername);
            var result = action.Invoke();
            result.Should().Be(true);
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

            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);
            Func<VideoCreate> function = () => this.videoService.GetVideoForEdit(video.Id, nonExistantUsername);
            function.Should().Throw<UserNotFound>();
        }

        [Test]///Checked
        public void GetVideoForEditShouldThrowIfVideoDoesNoteBelongToUser()
        {
            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);
            Func<VideoCreate> function = () => this.videoService.GetVideoForEdit(video.Id, UserS.PeshoUsername);
            function.Should().Throw<AccessDenied>().WithMessage("You can note edit video that does not belong to you!");
        }

        [Test]
        public void GetVideoForEditShouldReturnCorrectResult()
        {
            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId, true);
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

        [Test]
        public void GetVideoForEditApiShouldReturnNullIfUserIsNotFound()
        {
            UserS.SeedPeshoAndGosho(context);
            var nonExistantUsername = "Gosho420";

            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);
            Func<VideoCreate> action = () => this.videoService.GetVideoForEditApi(video.Id, nonExistantUsername);
            var result = action.Invoke();
            result.Should().Be(null);
        }

        [Test]
        public void GetVideoForEditApiShouldReturnCorrectResult()
        {
            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId, true);
            Func<VideoCreate> function = () => this.videoService.GetVideoForEditApi(video.Id, UserS.GoshoUsername);
            var result = function.Invoke();

            var dbNote0 = video.Notes.SingleOrDefault(x => x.Order == 0);
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

            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);
            Action action = () => this.videoService.Delete(video.Id, nonExistantUsername, DateTime.UtcNow);
            action.Should().Throw<UserNotFound>();

        }

        [Test]///Checked
        public void DeleteShouldThrowIfVideoDoesNoteBelongToUser()
        {
            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);
            Action action = () => this.videoService.Delete(video.Id, UserS.PeshoUsername, DateTime.UtcNow);
            action.Should().Throw<AccessDenied>().WithMessage("The video you are trying to delete does not belong to you!");
        }

        [Test]///Checked
        public void DeleteShouldSoftDeleteTheVideoAndAllItsNotes()
        {
            var now = DateTime.Now;
            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId, true);
            Action action = () => this.videoService.Delete(video.Id, UserS.GoshoUsername, now);
            action.Invoke();
            video.IsDeleted.Should().Be(true);
            video.DeletedOn.Should().Be(now);
            video.Notes.Select(x => x.IsDeleted).Should().AllBeEquivalentTo(true);
            video.Notes.Select(x => x.DeletedOn).Should().AllBeEquivalentTo(now);
        }
        ///API 
        [Test]///Checked
        public void DeleteApiShouldReturnFlaseIfVideoDoesNoteBelongToUser()
        {
            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId);
            Func<bool> action = () => this.videoService.DeleteApi(video.Id, UserS.PeshoUsername);
            var result = action.Invoke();
            result.Should().Be(false);
        }

        [Test]///Checked
        public void DeleteAPIShouldSoftDeleteTheVideoAndAllItsNotesAndReturTrue()
        {
            UserS.SeedPeshoAndGosho(context);
            var video = VideoS.SeedVideoToUserWithTwoOrThreeNotes(context, UserS.GoshoId, true);
            Func<bool> action = () => this.videoService.DeleteApi(video.Id, UserS.GoshoUsername);
            var result = action.Invoke();
            result.Should().Be(true);
            video.IsDeleted.Should().Be(true);
            video.Notes.Select(x => x.IsDeleted).Should().AllBeEquivalentTo(true);
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
                Order = note.Order,
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
