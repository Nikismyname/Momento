namespace Momento.Web.Controllers.ListToDo
{
    using Microsoft.AspNetCore.Mvc;
    using Momento.Services.Contracts.ListToDo;
    using Momento.Services.Models.ListToDoModels;
    using System.Linq;

    public class ListToDoController : Controller
    {
        private readonly IListToDoService toDoService;

        public ListToDoController(IListToDoService toDoService)
        {
            this.toDoService = toDoService;
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            var model = new ListToDoCreate();
            model.DirectoryId = id;
            model.UserName = User.Identity.Name;
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(ListToDoCreate model)
        {
            if (ModelState.IsValid)
            {
                toDoService.Create(model);
                return RedirectToAction("Index","Directory",new {id = model.DirectoryId});
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Use(int id)
        {
            var model = toDoService.GetUseModel(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(int id, int dirId)
        {
            toDoService.Delete(id);
            return RedirectToAction("Index","Directory", new {id = dirId});
        }

        [HttpPost]
        public IActionResult Save(ListToDoUse model)
        {
            model.Items = model.Items.Where(x => x.Deleted == false).ToList();
            if (ModelState.IsValid)
            {
                toDoService.Save(model, User.Identity.Name);
                return Redirect($"/Directory/Index/{model.DirectoryId}");
            }

            return RedirectToAction(nameof(Use), new {id = model.Id});
        }
    }
}