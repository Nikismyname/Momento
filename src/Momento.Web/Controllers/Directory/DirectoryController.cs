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

        public IActionResult IndexReact(int id)
        {
            var path = this.Request.Path;
            var reactPrerenderInfo = new ReactPrerenderInfo();

            var urlTokens = path.ToString().Split("/", StringSplitOptions.RemoveEmptyEntries);
            var lastPart = urlTokens.Last();

            if (urlTokens[0] != "Directory" || urlTokens[1] != "IndexReact")
                throw new Exception("Wrong Spa Fallback");

            if(int.TryParse(urlTokens[urlTokens.Length-1], out int index))
            {
                reactPrerenderInfo.WantedIndex = index;
                if (urlTokens.Length == 3)
                {
                    reactPrerenderInfo.WantedComponent = ReactComponent.index;
                }
                else
                {
                    if(urlTokens[2] == "compare" && urlTokens.Length == 4)
                    {
                        reactPrerenderInfo.WantedComponent = ReactComponent.compare;
                    }
                }
            }
            else
            {
                reactPrerenderInfo.WantedIndex = 0;

                if (urlTokens.Length == 2)
                {
                    reactPrerenderInfo.WantedComponent = ReactComponent.index;
                }
                else
                {
                    if (urlTokens[2] == "compare" && urlTokens.Length == 3)
                    {
                        reactPrerenderInfo.WantedComponent = ReactComponent.compare;
                    }
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

        //[HttpPost]
        //public void Reorder(string type, int parentDir, int[] values)
        //{
        //    reorderService.SaveItemsForOneDir(parentDir, type, values);
        //}
    }
}