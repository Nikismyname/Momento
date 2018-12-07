﻿namespace Momento.Web.Controllers.Content
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Momento.Services.Contracts.Directory;
    using Momento.Services.Models.DirectoryModels;

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
            if(id == null)
            {
                id = model.Id;
            }
            model.CurrentDirId = id;
            return View(model);
        }

        public IActionResult IndexReact()
        {
            return View();
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
            directoryService.Delete(id);
            return RedirectToAction(nameof(Index), new {id = returnDirId});
        }

        [HttpPost]
        public void Reorder(string type, int parentDir, int[] values)
        {
            reorderService.SaveItemsForOneDir(parentDir,type, values );
        }
    }
}