namespace Momento.Services.Contracts.View
{
    using Microsoft.AspNetCore.Http;
    using Momento.Services.Models.View;
    using System.Security.Claims;

    public interface ILayoutViewService
    {
        void SetData(HttpRequest request, HttpResponse response, ClaimsPrincipal user);
        LayoutData GetData();
    }
}
