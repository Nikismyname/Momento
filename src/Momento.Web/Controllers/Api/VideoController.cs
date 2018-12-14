namespace Momento.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Momento.Services.Contracts.Video;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService videoService;

        public VideoController(IVideoService videoService)
        {
            this.videoService = videoService;
        }

        ///api/Comparison/Delete
        [HttpPost]
        [Route("[action]")]
        public ActionResult<bool> Delete([FromBody] int id)
        {
            var result = this.videoService.DeleteApi(id, this.User.Identity.Name);
            return result;
        }
    }
}
