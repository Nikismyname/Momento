using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Momento.Services.Contracts.Admin;
using Momento.Services.Models.Admin;

namespace Momento.Web.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService adminService;

        public AdminController(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        [HttpGet]
        [Route("[action]")]
        public ActionResult<AdminViewUser[]> Get ()
        {
            var result = adminService.GetAllUsers(this.User.Identity.Name);
            return result;
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        [Route("[action]")]
        public ActionResult<bool> Promote([FromBody] string userId)
        {
            var result = adminService.PromoteUser(userId);
            return result;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<bool> Demote([FromBody] string userId)
        {
            var result = adminService.DemoteUser(userId);
            return result;
        }
    }
}
