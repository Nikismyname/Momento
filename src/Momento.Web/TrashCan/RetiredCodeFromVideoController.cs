//[HttpPost]
//public IActionResult Create(VideoCreateWithSettings modelIn)
//{
//    var model = modelIn.ContentCreate;
//    if (ModelState.IsValid)
//    {
//        model.Notes = RemoveDeleted(model.Notes);
//        ProcessPageNotes(model);
//        videoService.Create(model);
//        return RedirectToAction("Index","Directory", new {id = model.DirectoryId});
//    }

//    var settings = settingsService.GetVideoNoteSettings(User.Identity.Name);
//    modelIn.Mode = "create";
//    modelIn.Settings = settings;
//    return View("CreateEdit", modelIn);
//}

//[HttpPost]
//public IActionResult Edit(VideoCreateWithSettings model)
//{
//    var contentCreate = model.ContentCreate;
//    if (ModelState.IsValid)
//    {
//        contentCreate.Notes = RemoveDeleted(contentCreate.Notes);
//        ProcessPageNotes(contentCreate);
//        videoService.Edit(contentCreate);
//        return RedirectToAction("Index", "Directory", new { id = contentCreate.DirectoryId });
//    }

//    var settings = settingsService.GetVideoNoteSettings(User.Identity.Name);
//    model.Mode = "edit";
//    model.Settings = settings;
//    return View("CreateEdit", model);
//}
