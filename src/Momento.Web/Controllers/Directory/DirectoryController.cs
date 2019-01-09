namespace Momento.Web.Controllers.Content
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts.Directory;
    using Momento.Services.Models.DirectoryModels;
    using Momento.Web.Models.React;
    using Momento.Web.Models.React.Enums;
    using System;
    using System.Linq;

    [Authorize]
    public class DirectoryController : Controller
    {
        private readonly IDirectoryService directoryService;
        private readonly IReorderingService reorderService;

        public DirectoryController(IDirectoryService directoryService, IReorderingService reorderService)
        {
            this.directoryService = directoryService;
            this.reorderService = reorderService;
        }

        public IActionResult Index(int? id)
        {
            var model = directoryService.GetIndex(User.Identity.Name);
            if (id == null)
            {
                id = model.Id;
            }
            model.CurrentDirId = id;
            return View(model);
        }

        public IActionResult IndexReact(int id)
        {
            var path = this.Request.Path;
            // /Directory/IndexReact            -> index with root         |prerender
            // /Directory/IndexReact/:id        -> index with specific Dir |prerender
            // /Directory/IndexReact/adminView  ->                         |do note prerederd

            var reactPrerenderInfo = new ReactPrerenderInfo();

            var urlTokens = path.ToString().Split("/", StringSplitOptions.RemoveEmptyEntries);
            var lastPart = urlTokens.Last();
            if (int.TryParse(lastPart, out int index))
            {
                if (urlTokens.Length == 3)
                {
                    reactPrerenderInfo.ShouldPrerender = true;
                    reactPrerenderInfo.WantedComponent = ReactComponent.index;
                    reactPrerenderInfo.WantedIndex = index;
                }
            }
            else if (urlTokens.Length == 2)
            {
                reactPrerenderInfo.ShouldPrerender = true;
                reactPrerenderInfo.WantedComponent = ReactComponent.index;
                reactPrerenderInfo.WantedIndex = 0;
            }
            else
            {
                reactPrerenderInfo.ShouldPrerender = false;
            }

            return View(reactPrerenderInfo);
        }

        public ActionResult<DirectoryIndex> IndexApi(int? id)
        {
            var model = directoryService.GetIndex(User.Identity.Name);
            if (id == null)
            {
                id = model.Id;
            }
            return model;
        }

        [HttpPost]
        public IActionResult Create(int id, string name, int returnDirId)
        {
            directoryService.Create(id, name, User.Identity.Name);
            return RedirectToAction(nameof(Index), new { id = returnDirId });
        }

        public IActionResult Delete(int id, int returnDirId)
        {
            directoryService.Delete(id, this.User.Identity.Name);
            return RedirectToAction(nameof(Index), new { id = returnDirId });
        }
    }
}