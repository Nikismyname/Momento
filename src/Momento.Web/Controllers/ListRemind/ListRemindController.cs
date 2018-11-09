namespace Momento.Web.Controllers.ListRemind
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Momento.Services.Contracts.ListRemind;
    using Momento.Services.Models.ListRemind;
    using Momento.Web.Models.ListRemind;

    [Authorize]
    public class ListRemindController : Controller
    {
        private readonly IListRemindService listService;

        public ListRemindController(IListRemindService listService)
        {
            this.listService = listService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var model = listService.GetIndex(User.Identity.Name);
            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new ListRemindCreate();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(ListRemindCreate model)
        {
            if (ModelState.IsValid)
            {
                listService.Create(User.Identity.Name,model.Name,model.ListItems);
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = listService.GetEdit(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(ListRemindCreate model)
        {
            if (ModelState.IsValid)
            {
                listService.Edit(model.Id,model.Name,model.ListItems);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var model = new ListRemindDelete
            {
                 Id = id,
                 Name = listService.GetName(id),
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(ListRemindDelete model)
        {
            listService.Delete(model.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}