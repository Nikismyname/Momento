namespace Momento.Web.Middleware
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using Momento.Services.Contracts.View;

    public class AddDataToLayoutServiceActionFilter : IActionFilter
    {
        private readonly ILayoutViewService layoutService;

        public AddDataToLayoutServiceActionFilter(ILayoutViewService layoutService)
        {
            this.layoutService = layoutService;
        }

        ///sets the layout atomatically for every get request
        ///we don't render on POST when the model state is valids
        ///
        ///is there is extra logic in the future when we render when
        ///even on valid model, this code needs to change
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var valid = context.ModelState.IsValid;
            var method = context.HttpContext.Request.Method;

            if (method == "POST" && valid == true)
            {
                return;
            }

            var req = context.HttpContext.Request;
            var resp = context.HttpContext.Response;
            var user = context.HttpContext.User;
            layoutService.SetData(req, resp, user);
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            return;
        }
    }
}
