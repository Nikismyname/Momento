namespace Momento.Web.Controllers.ListToDo
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Momento.Services.Contracts.ListToDo;
    using Momento.Services.Models.ListToDoModels;
    using Momento.Services.Utilities;
    using System.Linq;

    public class ListToDoController : Controller
    {
        private readonly IListToDoService toDoService;

        public ListToDoController(IListToDoService toDoService)
        {
            this.toDoService = toDoService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create(int id)
        {
            var model = new ListToDoCreate();
            model.DirectoryId = id;
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(ListToDoCreate model)
        {
            if (ModelState.IsValid)
            {
                toDoService.Create(model, User.Identity.Name);
                return Redirect(Constants.ReactAppPath + "/" + model.DirectoryId);
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Use(int id)
        {
            var model = toDoService.GetUseModel(id, User.Identity.Name);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Use(ListToDoUse model)
        {
            model.Items = model.Items.Where(x => x.Deleted == false).ToList();
            if (ModelState.IsValid)
            {
                toDoService.Save(model, User.Identity.Name);
                return Redirect($"{Constants.ReactAppPath}/{model.DirectoryId}");
            }

            return View(model);
        }
    }
}


























//[HttpPost]
//[Authorize]
//public IActionResult Delete(int id, int dirId)
//{
//    toDoService.Delete(id, this.User.Identity.Name);
//    return RedirectToAction("Index","Directory", new {id = dirId});
//}