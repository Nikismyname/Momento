namespace Momento.Web.Controllers.CheatSheet
{
    using Microsoft.AspNetCore.Mvc;
    using Momento.Services.Contracts.CheatSheet;
    using Momento.Services.Models.CheatSheets;

    public class PointController : Controller
    {
        private readonly IPointService pointService;

        public PointController(IPointService pointService)
        {
            this.pointService = pointService;
        }

        [HttpGet]
        public IActionResult Create(int sheetId, int topicId)
        {
            var model = new PointCreate
            {
                TopicId = topicId,
                SheetId = sheetId,
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(PointCreate model)
        {
            if (ModelState.IsValid)
            {
                var content = model.Content;
                var contentFormatted = string.Empty;
                var preview = string.Empty;
                var previewFormatted = string.Empty;

                pointService.Create(model.TopicId,model.Name, content, model.Formatting);
                return RedirectToAction("Edit", "CheatSheet", new { id = model.SheetId });
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = pointService.GetEditModel(id);
            return null;
        }

        [HttpPost]
        public IActionResult Edit()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Delete(int pointId, int sheetiId)
        {
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            return View();
        }
    }
}