namespace Momento.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Mvc;
    using Momento.Services.Contracts.Notes;
    using Momento.Services.Models.NoteModels;

    [Route("api/[controller]")]
    [ApiController]
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
            var result = noteService.CreateApi(note, this.User.Identity.Name);
            return result;
        }

        // GET: api/Note/5
        [HttpGet("{id}", Name = "Get")]
        public ActionResult<NoteEdit> Get(int id)
        {
            var result = noteService.GetForEditApi(id, this.User.Identity.Name);
            return result;
        }

        // POST: api/Note
        [HttpPost]
        public ActionResult<bool> Post([FromBody] NoteEdit model)
        {
            var result = noteService.SaveApi(model, this.User.Identity.Name);
            return result;
        }

        // DELETE: api/Note/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
