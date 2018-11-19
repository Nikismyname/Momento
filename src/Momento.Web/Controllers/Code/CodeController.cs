namespace Momento.Web.Controllers.CodeSnipet
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Momento.Services.Contracts.Code;
    using Momento.Services.Models.Code;

    public class CodeController : Controller
    {
        private readonly ICodeService codeService;

        public CodeController(ICodeService codeService)
        {
            this.codeService = codeService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create(int id)
        {
            var model = new CodeCreate
            {
                DirectoryId = id,
            };
            return View(model);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Create(CodeCreate model)
        {
            if (ModelState.IsValid)
            {
                codeService.Create(model);
                return RedirectToAction("Index","Directory", new { id = model.DirectoryId });
            }

            return View(model);
        }
    }
}