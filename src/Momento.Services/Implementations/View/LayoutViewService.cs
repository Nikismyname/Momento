namespace Momento.Services.Implementations.View
{
    using Microsoft.AspNetCore.Http;
    using Momento.Data;
    using Momento.Models.Enums;
    using Momento.Services.Contracts.View;
    using Momento.Services.Models.View;
    using System;
    using System.Linq;
    using System.Security.Claims;

    /// <summary>
    /// SetData is called by two seperate filters PageFilter for razor pages
    /// ActionFilter for all other actions
    /// 
    /// SetData accepts resoponce, request and user and checks for the
    /// layout cookies, if non are presents, through the user, it gets 
    /// the layout preferences from database and sets the cookies, 
    /// if no user is logged in, it sends a defoult layout 
    /// 
    /// GetData() is used from the main _Layout (service is injected) to
    /// render the appropriate CSS 
    /// 
    /// The reason it is not all done in the service at one place is that 
    /// at the point at witch the service is called by the _Layout, you can not 
    /// set cookies any more
    /// 
    /// not sure if the whole sharade is necessary, there must be a better way 
    /// to select style
    /// </summary>
    
    public class LayoutViewService : ILayoutViewService
    {
        private readonly MomentoDbContext context;

        private LayoutData data = null;

        public LayoutViewService(IHttpContextAccessor httpContext, MomentoDbContext context)
        {
            this.context = context;
        }

        public LayoutData GetData()
        {
            if (this.data == null)
            {
                this.data = new LayoutData
                {
                    DarckInputs = true,
                    Theme = CSSTheme.Dark,
                };
            }

            return this.data;
        }

        public void SetData(HttpRequest request, HttpResponse response, ClaimsPrincipal user)
        {
            var options = new LayoutData();

            var theme = request.Cookies["Theme"];
            var darkInputs = request.Cookies["DarkInputs"];

            if (theme == null || darkInputs == null)
            {

                var userName = user?.Identity?.Name;
                if (userName == null)
                {
                    this.data = new LayoutData
                    {
                        DarckInputs = true,
                        Theme = CSSTheme.Dark,
                    };

                    return;
                }

                var dbObject = context.Users
                    .Select(x => new
                    {
                        username = x.UserName,
                        opt = new LayoutData
                        {
                            DarckInputs = x.UserSettings.LADarkInputs,
                            Theme = x.UserSettings.LACSSTheme
                        }
                    })
                    .SingleOrDefault(x => x.username == userName);

                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(10);
                response.Cookies.Append("Theme", ((int)options.Theme).ToString(), option);
                response.Cookies.Append("DarkInputs", options.DarckInputs.ToString().ToLower(), option);

                this.data = dbObject.opt;
                return;
            }

            options.Theme = (CSSTheme)(int.Parse(theme));
            options.DarckInputs = darkInputs == "true" ? true : false;

            this.data = options;
        }
    }
}
