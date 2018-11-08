namespace Momento.Web.Controllers.Other
{
    using Microsoft.AspNetCore.Mvc;

    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Test()
        {
            return View();
        }
    }
}