namespace Momento.Web.Controllers.CheatSheet
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Momento.Services.Contracts.CheatSheet;
    using Momento.Services.Contracts.Other;
    using Momento.Services.Models.CheatSheets;
    using Momento.Web.Models.CheatSheet;

    [Authorize]
    public class CheatSheetController : Controller
    {
        private readonly ICheatSheetService cheatSheetService;
        private readonly IUserService userService;

        public CheatSheetController(ICheatSheetService cheatSheetService, IUserService userService)
        {
            this.cheatSheetService = cheatSheetService;
            this.userService = userService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var cheatSheets = this.cheatSheetService.GetAllCheatSheetsForUser(User.Identity.Name);
            return View(cheatSheets);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new CheatSheetCreate());
        }

        [HttpPost]
        public IActionResult Create(CheatSheetCreate model)
        {
            if (ModelState.IsValid)
            {
                var userId = userService.ByUsername(User.Identity.Name).Id;
                cheatSheetService.CreateCheetSheet(model.Title,model.Description, userId);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var topics = cheatSheetService.GetAllTopicsWithPoints(id);
            var model = new CheatSheetEdit
            {
                 Id = id,
                 Topics = topics,
            };
            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var name = cheatSheetService.ById(id).Name;

            var model = new CheatSheetDelete
            {
                Id = id,
                Name = name,
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(CheatSheetDelete model)
        {
            cheatSheetService.Delete(model.Id);

            return RedirectToAction(nameof(Index));
        }
    }
}
