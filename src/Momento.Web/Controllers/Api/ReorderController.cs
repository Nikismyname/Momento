namespace Momento.Web.Controllers.Api
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Momento.Services.Contracts.Directory;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReorderController : ControllerBase
    {
        private readonly IReorderingService reorderingService;

        public ReorderController(IReorderingService reorderingService)
        {
            this.reorderingService = reorderingService;
        }

        public class ReorderData
        {
            public string  Type { get; set; }
            public int  Directory { get; set; }
            public int[][] NewOrders { get; set; }
        }

        [HttpPost]
        [Route("[action]")]
        public void Reorder([FromBody] ReorderData data)
        {
            this.reorderingService.Reorder(data.Type,data.Directory, data.NewOrders, this.User.Identity.Name);
        }
    }
}
