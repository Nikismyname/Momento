namespace Momento.Web.Controllers.Other
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Momento.Models.Users;
    using Momento.Services.Contracts.Other;
    using System;

    [Authorize]
    public class SettingsController : Controller
    {
        private readonly ISettingsService settingsService;

        public SettingsController(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        [HttpGet]
        public ActionResult Edit()
        {
            var username = User.Identity.Name;
            var model = settingsService.GetSettings(username);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(UserSettings model)
        {
            if (ModelState.IsValid)
            {
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(10);
                Response.Cookies.Append("Theme", ((int)model.LACSSTheme).ToString(), option);
                Response.Cookies.Append("DarkInputs", model.LADarkInputs.ToString().ToLower(), option);

                settingsService.Edit(model);
                return RedirectToAction("Index", "Home");
            }

            return View();
        }
    }
}