namespace Momento.Web.Controllers.Other
{
    using System;
    using System.Diagnostics;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Momento.Web.Models;

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            Response.Cookies.Append("test", "test", new CookieOptions { Expires = DateTime.Now.AddDays(10) });

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
