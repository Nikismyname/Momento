namespace Momento.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Momento.Services.Contracts.Other;
    using Services.Contracts.Video;
    using Momento.Services.Models.VideoModels;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService videoService;
        private readonly ISettingsService settingsService;

        public VideoController(IVideoService videoService, ISettingsService settingsService)
        {
            this.videoService = videoService;
            this.settingsService = settingsService;
        }

        /// /api/Video/Create
        [HttpPost]
        [Route("[action]")]
        public ActionResult<bool> Create(VideoInitialCreate videoCreate)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var result = this.videoService.CreateApi(videoCreate, this.User.Identity.Name, isAdmin);
            return result;
        }

        /// /api/Video/Delete/[FromBody]id
        [HttpPost]
        [Route("[action]")]
        public ActionResult<bool> Delete([FromBody] int id)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var result = this.videoService.DeleteApi(id, this.User.Identity.Name, isAdmin);
            return result;
        }

        /// /api/Video/GetView fromBody: videoId
        [HttpPost]
        [Route("[action]")]
        public ActionResult<VideoView> GetView([FromBody] int videoId)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var result = this.videoService.GetViewApi(videoId, this.User.Identity.Name, isAdmin);
            return result;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<bool> Save([FromBody]VideoSave data)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var username = this.User.Identity.Name;
            var result = this.videoService.PartialSaveApi(
                data.VideoId, username, data.SeekTo, data.Name, data.Description,
                data.Changes, data.NewNotes, data.FinalSave, isAdmin);
            return result;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<VideoCreateWithSettings> GetForEdit([FromBody] int videoId)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var videoCreate = videoService.GetVideoForEditApi(videoId, this.User.Identity.Name, isAdmin);
            if (videoCreate == null)
            {
                return null;
            }

            var settings = this.settingsService.GetVideoNoteSettings(User.Identity.Name);

            var model = new VideoCreateWithSettings
            {
                ContentCreate = videoCreate,
                Settings = settings,
            };

            return model;
        }
    }
}
