namespace Momento.Tests.SeleniumTests
{
    using FluentAssertions;
    using FunApp.Web.Tests;
    using Momento.Web;
    using OpenQA.Selenium;
    using Xunit;
    using Momento.Services.Utilities;
    using Momento.Tests.Contracts;
    using Momento.Tests.Seeding;
    using System.Linq;

    public class SeleniumInMemoryDbTests : SeleniumInMemoryDbBaseTest
    {
        public SeleniumInMemoryDbTests(SeleniumServerFactoryInMemory<Startup> server) : base(server) { }

        [Fact]
        public void InMemoryTest()
        {
            RegisterUser(UserS.GoshoUsername,UserS.GoshoEmail,UserS.GoshoPassword,this.Browser);
            var user = Context.Users.SingleOrDefault();
            this.Server.Context.Users.Count().Should().Be(1);
        }

        private void LoginUser(string username, string password, IWebDriver browser)
        {
            browser.Navigate().GoToUrl(this.Server.RootUri +"/Identity/Account/Login");

            browser.FindElement(By.Name("Input.Username")).SendKeys(username);

            browser.FindElement(By.Name("Input.Password")).SendKeys(password);

            browser.FindElement(By.TagName("form")).Submit();

            browser.Navigate().GoToUrl(this.Server.RootUri + Constants.ReactAppPath);
        }


        private void RegisterUser(string username, string emain, string password, IWebDriver browser)
        {
            browser.Navigate().GoToUrl(this.Server.RootUri + "/Identity/Account/Register");

            browser.FindElement(By.Name("Input.Username")).SendKeys(username);

            browser.FindElement(By.Name("Input.Email")).SendKeys(emain);

            browser.FindElement(By.Name("Input.Password")).SendKeys(password);

            browser.FindElement(By.Name("Input.ConfirmPassword")).SendKeys(password);

            browser.FindElement(By.TagName("form")).Submit();
        }
    }
}
