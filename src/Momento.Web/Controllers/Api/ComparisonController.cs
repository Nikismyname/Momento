namespace Momento.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts.Comparisons;
    using Momento.Services.Models.ComparisonModels;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ComparisonController : ControllerBase
    {
        private readonly IComparisonService comparisonService;

        public ComparisonController(IComparisonService comparisonService)
        {
            this.comparisonService = comparisonService;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<bool> Create([FromBody] ComparisonCreate data)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var result = this.comparisonService.CreateApi(
                data, this.User.Identity.Name, isAdmin);
            return result;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<ComparisonEdit> Get([FromBody] int comparisonId)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var comp = this.comparisonService.GetForEditApi(
                comparisonId, this.User.Identity.Name,isAdmin);
            return comp;
        }

        [HttpPost]
        [Route("[action]")]
        public ActionResult<bool> Save([FromBody] ComparisonSave data)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var result = this.comparisonService.SaveApi(
                data, this.User.Identity.Name,isAdmin);
            return result;
        }

        ///api/Comparison/Delete
        [HttpPost]
        [Route("[action]")]
        public ActionResult<bool> Delete([FromBody] int id)
        {
            var isAdmin = this.User.IsInRole("Admin");
            var result = this.comparisonService.DeleteApi(
                id, this.User.Identity.Name,isAdmin);
            return result;
        }
    }
}
