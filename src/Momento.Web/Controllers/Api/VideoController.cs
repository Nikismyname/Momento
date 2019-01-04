namespace Momento.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Momento.Services.Contracts.Other;
    using Momento.Services.Contracts.Video;
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
            var result = this.videoService.CreateApi(videoCreate, this.User.Identity.Name);
            return result;
        }

        /// /api/Video/Delete/[FromBody]id
        [HttpPost]
        [Route("[action]")]
        public ActionResult<bool> Delete([FromBody] int id)
        {
            var result = this.videoService.DeleteApi(id, this.User.Identity.Name);
            return result;
        }

        /// /api/Video/GetView fromBody: videoId
        [HttpPost]
        [Route("[action]")]
        public ActionResult<VideoView> GetView([FromBody] int videoId)
        {
            var result = this.videoService.GetViewApi(videoId, this.User.Identity.Name);
            return result;
        }

        public class SaveData
        {
            public int VideoId { get; set; }
            public int? SeekTo { get; set; }
            public string  Name { get; set; }
            public string  Description { get; set; }
            public string  Url { get; set; }
            public string[][] Changes { get; set; }
            public VideoNoteCreate[] NewNotes { get; set; }
            public bool FinalSave { get; set; }
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<bool> Save([FromBody]SaveData data)
        {
            var username = this.User.Identity.Name;
            var result = this.videoService.PartialSaveApi(data.VideoId, username, data.SeekTo, data.Name, data.Description, data.Url, data.Changes, data.NewNotes, data.FinalSave);
            return result;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<VideoCreateWithSettings> GetForEdit([FromBody] int videoId)
        {
            var videoCreate = videoService.GetVideoForEditApi(videoId, this.User.Identity.Name);
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
