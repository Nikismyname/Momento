namespace Momento.Web.Middleware
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using Momento.Services.Contracts.View;

    public class AddDataToLayoutServicePageFilter : IPageFilter
    {
        private readonly ILayoutViewService layoutService;

        public AddDataToLayoutServicePageFilter(ILayoutViewService layoutService)
        {
            this.layoutService = layoutService;
        }

        ///this is necessary for the layout service since the actionFilter does not capture razorpages 
        ///not sure why it breaks if I disable post request but it does, so I don't 
        ///you can not even check for model state valid, because it will still render if the login info is not valid
        public void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            var req = context.HttpContext.Request;
            var resp = context.HttpContext.Response;
            var user = context.HttpContext.User;
            layoutService.SetData(req, resp, user);
        }

        public void OnPageHandlerExecuted(PageHandlerExecutedContext context)
        {
            return;
        }

        public void OnPageHandlerSelected(PageHandlerSelectedContext context)
        {
            return;
        }
    }
}
