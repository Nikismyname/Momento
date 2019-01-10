using System.Collections.Generic;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Momento.Services.Contracts.ListToDo;

namespace Momento.Web.Controllers.Api
{
    [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ListToDoController : ControllerBase
    {
        private readonly IListToDoService listToDoService;

        public ListToDoController(IListToDoService listToDoService)
        {
            this.listToDoService = listToDoService;
        }

        // DELETE: api/ListToDo/5
        [Microsoft.AspNetCore.Mvc.HttpDelete(/*"{id}"*/)]
        public ActionResult<bool> Delete([Microsoft.AspNetCore.Mvc.FromBody]int id)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var result = this.listToDoService.DeleteApi(id, this.User.Identity.Name, isAdmin);
            return result;
        }
    }
}
