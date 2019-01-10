using System.Web.Http;

namespace Momento.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Mvc;
    using Momento.Services.Contracts.Notes;
    using Momento.Services.Models.NoteModels;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NoteController : ControllerBase
    {
        private readonly INoteService noteService;

        public NoteController(INoteService noteService)
        {
            this.noteService = noteService;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<bool> Create([FromBody] NoteCreate note)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var result = noteService.CreateApi(
                note, this.User.Identity.Name, isAdmin);
            return result;
        }

        // GET: api/Note/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<NoteEdit> Get(int id)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var result = noteService.GetForEditApi(
                id, this.User.Identity.Name, isAdmin);
            return result;
        }

        // POST: api/Note
        [HttpPost]
        public ActionResult<bool> Post([FromBody] NoteEdit model)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var result = noteService.SaveApi(
                model, this.User.Identity.Name, isAdmin);
            return result;
        }

        // DELETE: api/Note/5
        [HttpDelete("{id}")]
        public ActionResult<bool> Delete(int id)
        {
            var isAdmin = this.User.IsInRole("Admin");
            return this.noteService.DeleteApi(id, this.User.Identity.Name,isAdmin);
        }
    }
}
