namespace Momento.Web.Controllers.Content
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Momento.Services.Contracts.Other;
    using Momento.Services.Contracts.Video;
    using Momento.Services.Models.VideoModels;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Authorize]
    public class VideoController : Controller
    {
        #region Initializing
        private readonly IVideoService videoService;
        private readonly IUserService userService;
        private readonly ISettingsService settingsService;

        public VideoController(
            IVideoService videoService,
            IUserService userService,
            ISettingsService settingsService)
        {
            this.videoService = videoService;
            this.userService = userService;
            this.settingsService = settingsService;
        }
        #endregion

        #region View
        [HttpGet]
        ///This is where you can press buttons to move around the video.
        public IActionResult View(int id)
        {
            var model = videoService.GetView(id, this.User.Identity.Name);
            return View(model);
        }
        #endregion

        #region Create 
        //[HttpGet]
        //public IActionResult Create(int id)///Id is parent dir Id
        //{
        //    var settings = settingsService.GetVideoNoteSettings(User.Identity.Name);
        //    var videoId = videoService.Create(id, this.User.Identity.Name);

        //    var model = new VideoCreateWithSettings
        //    {
        //        ContentCreate = new VideoCreate()
        //        {
        //            Id = videoId,
        //            DirectoryId = id,
        //        },
        //        Settings = settings,
        //        Mode = "create",
        //    };

        //    return View("CreateEdit",model);
        //}
        #endregion

        #region Edit 
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var videoCreate = videoService.GetVideoForEdit(id, this.User.Identity.Name);

            //videoCreate.Id = id;
            var settings = settingsService.GetVideoNoteSettings(User.Identity.Name);
            var model = new VideoCreateWithSettings
            {
                ContentCreate = videoCreate,
                Settings = settings,
                Mode = "edit",
            };

            return View("CreateEdit", model);
        }
        #endregion

        [HttpPost]
        public IActionResult Delete(int contentId, int directoryId)
        {
            videoService.Delete(contentId, User.Identity.Name, DateTime.UtcNow);
            return RedirectToAction("Index", "Directory",new { id = directoryId});
        }

        [HttpPost]
        public IActionResult PartialSave(int videoId, int? seekTo, string name, string description,
                                         string url, string[][] changes, VideoNoteCreate[] newItems, bool finalSave)
        {
            var username = this.User.Identity.Name;
            var result = this.videoService.PartialSave(videoId, username, seekTo, name, description, url, changes, newItems, finalSave);            
            return Json(result);
        }

        #region Helpers
        private void ProcessPageNotes(VideoCreate model)
        {
            for (int i = 0; i < model.Notes.Count; i++)
            {
                if (model.Notes[i].Content == null)
                {
                    model.Notes[i].Content = string.Empty;
                }
            }
        }

        private List<VideoNoteCreate> RemoveDeleted(List<VideoNoteCreate> notes)
            => notes.Where(x => x.Deleted == false).ToList();
        #endregion
    }
}