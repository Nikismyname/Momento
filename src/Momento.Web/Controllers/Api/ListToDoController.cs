using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Momento.Services.Contracts.ListToDo;

namespace Momento.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListToDoController : ControllerBase
    {
        private readonly IListToDoService listToDoService;

        public ListToDoController(IListToDoService listToDoService)
        {
            this.listToDoService = listToDoService;
        }

        // DELETE: api/ListToDo/5
        [HttpDelete(/*"{id}"*/)]
        public ActionResult<bool> Delete([FromBody]int id)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var result = this.listToDoService.DeleteApi(id, this.User.Identity.Name, isAdmin);
            return result;
        }
    }
}
