using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Momento.Services.Contracts.Directory;

namespace Momento.Web.Controllers.Api
{
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

        // GET: api/Navigation
        [HttpGet]
        public JsonResult Get()
        {
            var model = directoryService.GetIndex(this.User.Identity.Name);
            return new JsonResult(model);
        }

        // GET: api/Navigation/5
        [HttpGet("{id}", Name = "Get")]
        public JsonResult Get(int id)
        {
            var dir = directoryService.GetIndexSingle(id,this.User.Identity.Name);
            return new JsonResult(dir);
        }

        // POST: api/Navigation
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Navigation/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
