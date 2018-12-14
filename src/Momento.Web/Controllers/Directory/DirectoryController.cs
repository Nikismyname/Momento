namespace Momento.Web.Controllers.Content
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Momento.Services.Contracts.Directory;
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

        public IActionResult IndexReact()
        {
            var path = this.Request.Path;
            var reactPrerenderInfo = new ReactPrerenderInfo();

            var urlTokens = path.ToString().Split("/");
            var lastPart = urlTokens.Last();

            if (int.TryParse(lastPart, out int index))
            {
                reactPrerenderInfo.WantedIndex = index;
                var component = urlTokens[urlTokens.Length - 2];
                if (Enum.TryParse(component, out ReactComponent comp))
                {
                    reactPrerenderInfo.WantedComponent = comp;
                }
            }
            else
            {
                if (Enum.TryParse(lastPart, out ReactComponent comp))
                {
                    reactPrerenderInfo.WantedComponent = comp;
                }
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

        [HttpPost]
        public void Reorder(string type, int parentDir, int[] values)
        {
            reorderService.SaveItemsForOneDir(parentDir, type, values);
        }
    }
}