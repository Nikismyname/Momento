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

        // GET: api/ListToDo
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // POST: api/ListToDo
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/ListToDo/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ListToDo/5
        [HttpDelete(/*"{id}"*/)]
        public ActionResult<bool> Delete([FromBody]int id)
        {
            var result = this.listToDoService.DeleteApi(id, this.User.Identity.Name);
            return result;
        }
    }
}
