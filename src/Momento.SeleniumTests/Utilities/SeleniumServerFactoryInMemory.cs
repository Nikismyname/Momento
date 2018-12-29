namespace FunApp.Web.Tests
{
    using System;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Hosting.Server.Features;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.Data.Sqlite;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Momento.Data;

    public class SeleniumServerFactoryInMemory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        public string RootUri { get; set; } //Save this use by tests
        public MomentoDbContext Context { get; set; }
        public SqliteConnection Connection { get; set; }

        protected Process _process;
        protected IWebHost _host;
        

        public SeleniumServerFactoryInMemory()
        {
            ClientOptions.BaseAddress = new Uri("https://localhost"); //will follow redirects by default

            _process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "selenium-standalone",
                    Arguments = "start",
                    UseShellExecute = true
                }
            };
            _process.Start();
        }

        protected override TestServer CreateServer(IWebHostBuilder builder)
        {
            this.Connection = new SqliteConnection("Data Source=:memory:");
            this.Connection.Open();

            builder.ConfigureServices(services => {
                services.AddDbContext<MomentoDbContext>(options =>
                    options.UseSqlite(this.Connection));
            });
            _host = builder.UseEnvironment("Testing").Build();
            _host.Start();
            this.Context = _host.Services.GetService(typeof(MomentoDbContext)) as MomentoDbContext;
            this.Context.Database.EnsureCreated();
            RootUri = _host.ServerFeatures.Get<IServerAddressesFeature>().Addresses.LastOrDefault(); //Last is https://localhost:5001!
            //Fake Server we won't use...this is lame. Should be cleaner, or a utility class
            return new TestServer(new WebHostBuilder().UseStartup<FakeStartup>());
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                _host.Dispose();
                _process.CloseMainWindow(); //Be sure to stop Selenium Standalone
            }
        }

        public void ResetDatabase()
        {
            this.Connection.Close();
            this.Connection.Open();
            this.Context.Database.EnsureCreated();
        }

        public class FakeStartup
        {
            public void ConfigureServices(IServiceCollection services)
            {
            }

            public void Configure()
            {
            }
        }
    }
}