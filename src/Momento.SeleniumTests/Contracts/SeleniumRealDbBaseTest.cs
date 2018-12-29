namespace Momento.Tests.Contracts
{
    using FunApp.Web.Tests;
    using Momento.Web;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Remote;
    using System;
    using Xunit;

    public abstract class SeleniumRealDbBaseTest : IClassFixture<SeleniumServerFactory<Startup>>,  IDisposable
    {
        protected SeleniumServerFactory<Startup> Server { get; }
        protected IWebDriver Browser { get; }

        protected SeleniumRealDbBaseTest(SeleniumServerFactory<Startup> server)
        {
            this.Server = server;
            server.CreateClient();
            var opts = new ChromeOptions();
            /*opts.AddArgument("--headless");*/ //Optional, comment this out if you want to SEE the browser window
            opts.AddArgument("no-sandbox");
            this.Browser = new RemoteWebDriver(opts);
        }

        public void Dispose()
        {
            //Server.Dispose();
            Browser.Dispose();
        }
    }
}
