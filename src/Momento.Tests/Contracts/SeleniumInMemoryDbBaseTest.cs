namespace Momento.Tests.Contracts
{
    using FunApp.Web.Tests;
    using Momento.Data;
    using Momento.Web;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Remote;
    using System;
    using Xunit;

    public abstract class SeleniumInMemoryDbBaseTest : IClassFixture<SeleniumServerFactoryInMemory<Startup>>, IDisposable
    {
        protected SeleniumServerFactoryInMemory<Startup> Server { get; }
        protected IWebDriver Browser { get; }
        protected MomentoDbContext Context { get; }
        
        protected SeleniumInMemoryDbBaseTest(SeleniumServerFactoryInMemory<Startup> server)
        {
            this.Server = server;
            server.CreateClient();
            var opts = new ChromeOptions();
            /*opts.AddArgument("--headless");*/ //Optional, comment this out if you want to SEE the browser window
            opts.AddArgument("no-sandbox");
            this.Browser = new RemoteWebDriver(opts);
            this.Context = server.Context;
        }

        public void Dispose()
        {
            Server.Dispose();
            Browser.Dispose();
        }
    }
}
