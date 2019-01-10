namespace Momento.SeleniumTests.Tests
{
    using FluentAssertions;
    using FunApp.Web.Tests;
    using Momento.Web;
    using OpenQA.Selenium;
    using Xunit;
    using Momento.Services.Utilities;
    using Momento.Tests.Contracts;

    public class SeleniumRealDbTests: SeleniumRealDbBaseTest
    {
        public SeleniumRealDbTests(SeleniumServerFactory<Startup> server) : base(server) { }

        [Fact]
        public void XUnitShouldWorkWithFluentAssertions()
        {
            this.LogInWithMainAccount(this.Browser, this.Server);
            12.Should().Be(12);
        }

        private void LogInWithMainAccount(IWebDriver browser, SeleniumServerFactory<Startup> server)
        {
            browser.Navigate().GoToUrl(this.Server.RootUri +"/Identity/Account/Login");
            var username = browser.FindElement(By.Name("Input.Username"));
            var password = browser.FindElement(By.Name("Input.Password"));
            var form = browser.FindElement(By.TagName("form"));
            username.SendKeys("test@test.test");
            password.SendKeys("testtest");
            form.Submit();
            browser.Navigate().GoToUrl(server.RootUri + Constants.ReactAppPath);
        }
    }
}
