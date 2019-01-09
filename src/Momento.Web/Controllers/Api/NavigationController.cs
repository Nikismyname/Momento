using Microsoft.AspNetCore.Routing.Constraints;

namespace Momento.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts.Directory;
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
            var isAdmin = this.User.IsInRole("Admin");
            var dir = directoryService.GetIndexSingleApi(id,this.User.Identity.Name, isAdmin);
            return dir;
        }

        // POST: api/Navigation
        [HttpPost]
        [Route("[action]")]
        public JsonResult CreateDirectory([FromBody]DirectoryCreate data)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var result = directoryService.CreateApi(data.ParentDirId, data.DirectoryName , User.Identity.Name, isAdmin);
            return new JsonResult(result);
        }

        [HttpPost]
        [Route("[action]")]
        public JsonResult Delete([FromBody] int id)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var result = this.directoryService.DeleteApi(id, User.Identity.Name, isAdmin);
            return new JsonResult(result);
        }
    }
}
