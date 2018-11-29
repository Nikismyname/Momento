namespace Momento.Tests.Tests.VideoServiceTests
{
    using AutoMapper;
    using FluentAssertions;
    using Momento.Models.Videos;
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

    public class RestTests : BaseTestsSqliteInMemory
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
        #region ById
        [Test]
        public void ByIdReturnsTheCorrectVideo()
        {
            Seeder.SeedPeshoAndGosho(context);
            var video = Seeder.SeedVideosToUser(context, Seeder.GoshoId);
            var resultVideo = this.videoService.ById(video.Id);
            resultVideo.Should().Be(video);
        }

        [Test]
        public void ByIdReturnsNullIfVideoWithGivenIdDoesNotExist()
        {
            const int NonExistantId = 10;

            Seeder.SeedPeshoAndGosho(context);
            var video = Seeder.SeedVideosToUser(context, Seeder.GoshoId);
            var resultVideo = this.videoService.ById(NonExistantId);
            resultVideo.Should().Be(null);
        }
        #endregion

        #region GetView
        [Test]
        public void GetViewThrowsIfVideoNotFound()
        {
            Action action = () => this.videoService.GetView(42);
            action.Should().Throw<ItemNotFound>().WithMessage("Video you are trying to view does not exist in the database!");
        }

        [Test]
        ///TODO: Add more complex cases if an issue arises, Add test with nested notes
        public void GetViewShouldReturnCorrectView()
        {
            Seeder.SeedPeshoAndGosho(context);
            var video = Seeder.SeedVideosToUserWithNotes(context, Seeder.PeshoId);
            var resultView = this.videoService.GetView(video.Id);
            var expectedResultView = new VideoView
            {
                Description = video.Description,
                Name = video.Name,
                Url = video.Url,
                Notes = video.Notes.Select(x=> MapVideoNoteToView(x)).ToList()
            };

            resultView.Should().BeEquivalentTo(expectedResultView);
        }
        #endregion

        #region Create

        [Test]
        public void CreateThrowsIfDirectoryDoesNotExist()
        {
            const int NonExistantDirectoryId = 13;
            const string NonExistantUsername = "NonExistantUsername";

            Action action = () => this.videoService.Create(NonExistantDirectoryId, NonExistantUsername);
            action.Should().Throw<ItemNotFound>().WithMessage("The Directory you selected for creating the new video notes in, does not exist!");
        }

        [Test]
        public void CreateThrowsIfUserDoesNotExist()
        {
            ///Also seeds their root directories
            Seeder.SeedPeshoAndGosho(context);

            int ExistingDirecotryId = Seeder.GoshoRootDirId;
            const string NonExistantUsername = "NonExistantUsername";

            Action action = () => this.videoService.Create(ExistingDirecotryId, NonExistantUsername);
            action.Should().Throw<UserNotFound>();
        }

        [Test]
        public void CreateThrowsIfDirectoryDoesNotBelongToUser()
        {
            ///Also seeds their root directories
            Seeder.SeedPeshoAndGosho(context);

            int ExistingDirecotryId = Seeder.GoshoRootDirId;
            string ExistingUsername = Seeder.PeshoUsername;

            Action action = () => this.videoService.Create(ExistingDirecotryId, ExistingUsername);
            action.Should().Throw<AccessDenied>().WithMessage("The directory you are trying to create a video on does note belong to you!");
        }

        [Test]
        ///TODO: Order
        public void CreateCreatsVideo()
        {
            ///Also seeds their root directories
            Seeder.SeedPeshoAndGosho(context);

            int ExistingDirecotryId = Seeder.GoshoRootDirId;
            string ExistingUsername = Seeder.GoshoUsername;

            Action action = () => this.videoService.Create(ExistingDirecotryId, ExistingUsername);
            action.Invoke();

            var video = context.Directories
                .SingleOrDefault(x => x.Id == ExistingDirecotryId)
                .Videos
                .SingleOrDefault();

            video.DirectoryId.Should().Be(ExistingDirecotryId);
            video.UserId.Should().Be(Seeder.GoshoId);
        }

        #endregion

        #region Helpers 
        private VideoNoteView MapVideoNoteToView(VideoNote note)
        {
            return new VideoNoteView()
            {
                Content = note.Content,
                Formatting = note.Formatting,
                Id = note.Id,
                Level = note.Level,
                SeekTo = note.SeekTo,
            };
        }
        #endregion
    }
}
