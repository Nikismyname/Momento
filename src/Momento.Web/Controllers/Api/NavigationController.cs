namespace Momento.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Momento.Services.Contracts.Directory;
    using Momento.Services.Models.DirectoryModels;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NavigationController : ControllerBase
    {
        private readonly IDirectoryService directoryService;

        public NavigationController(IDirectoryService directoryService)
        {
            this.directoryService = directoryService;
        }

        [HttpGet("GetDirSingle/{id:int}")]
        public ActionResult<DirectoryIndexSingle> GetDirSingle(int id)
        {
            var dir = directoryService.GetIndexSingle(id,this.User.Identity.Name);
            return dir;
        }

        public class DirCreateData
        {
            public string directoryName { get; set; }
            public int parentDirId { get; set; }
        }

        // POST: api/Navigation
        [HttpPost]
        [Route("[action]")]
        public JsonResult CreateDirectory([FromBody]DirCreateData data)
        {
            var result = directoryService.CreateApi(data.parentDirId, data.directoryName , User.Identity.Name);
            return new JsonResult(result);
        }

        [HttpPost]
        [Route("[action]")]
        public JsonResult Delete([FromBody] int id)
        {
            var result = this.directoryService.DeleteApi(id, User.Identity.Name);
            return new JsonResult(result);
        }
    }
}
