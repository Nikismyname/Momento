namespace Momento.Web.Controllers.ListToDo
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Momento.Services.Contracts.ListToDo;
    using Momento.Services.Models.ListToDoModels;
    using Services.Utilities;
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
                var isAdmin = this.User.IsInRole("Admin");
                toDoService.Create(model, User.Identity.Name, isAdmin);
                return Redirect(Constants.ReactAppPath + "/" + model.DirectoryId);
            }

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Use(int id)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var model = toDoService.GetUseModel(id, User.Identity.Name, isAdmin);
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Use(ListToDoUse model)
        {
            model.Items = model.Items.Where(x => x.Deleted == false).ToList();
            if (ModelState.IsValid)
            {
                var isAdmin = this.User.IsInRole("Admin");
                toDoService.Save(model, User.Identity.Name, isAdmin);
                return Redirect($"{Constants.ReactAppPath}/{model.DirectoryId}");
            }

            return View(model);
        }
    }
}