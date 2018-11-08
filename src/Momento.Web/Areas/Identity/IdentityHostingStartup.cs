using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Momento.Web.Areas.Identity.IdentityHostingStartup))]
namespace Momento.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}