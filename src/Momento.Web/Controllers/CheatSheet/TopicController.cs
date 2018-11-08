namespace Momento.Web.Controllers.CheatSheet
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Momento.Services.Contracts.CheatSheet;
    using Momento.Services.Models.CheatSheets;
    using Momento.Web.Models.CheatSheet;

    [Authorize]
    public class TopicController : Controller
    {
        private readonly ITopicService topicService;

        public TopicController(ITopicService topicService)
        {
            this.topicService = topicService;
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            var model = new TopicCreate
            {
                SheetId = id,
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Create(TopicCreate model)
        {
            if (ModelState.IsValid)
            {
                topicService.CreateTopic(model.SheetId, model.Name);
                return RedirectToAction("Edit", "CheatSheet", new { id = model.SheetId });
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = topicService.GetTopicEditViewModel(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(TopicEdit model)
        {
            if (ModelState.IsValid)
            {
                topicService.Edit(model.TopicId, model.Name);

                return RedirectToAction("Edit", "CheatSheet", new { id = model.SheetId });
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var topic = topicService.ById(id);
            var name = topic.Name;
            var sheetId = topic.CheatSheetId;

            var model = new TopicDelete
            {
                TopicId = id,
                SheetId = sheetId,
                Name = name,
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Delete(TopicDelete model)
        {
            topicService.Delete(model.TopicId);
            return RedirectToAction("Edit","CheatSheet", new {id = model.SheetId});
        }
    }
}