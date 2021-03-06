﻿namespace Momento.Tests.SeleniumTests
{
    using FluentAssertions;
    using FunApp.Web.Tests;
    using Momento.Tests.Contracts;
    using Momento.Tests.Seeding;
    using Momento.Tests.Utilities;
    using Momento.Web;
    using System.Linq;
    using Xunit;

    //SeleniumServerFactoryInMemory<Startup> Server { get; }
    //IWebDriver Browser { get; }
    //MomentoDbContext Context { get; }
    //string RootUri { get; set; }

    public class SeleniumRegisterTests : SeleniumInMemoryDbBaseTest
    {
        public SeleniumRegisterTests(SeleniumServerFactoryInMemory<Startup> server) : base(server, false) { }

        [Fact]
        public void RegisterRegistesNewUser()
        {
            SeleniumActions.RegisterUser(UserS.PeshoUsername, UserS.PeshoEmail,
                UserS.PeshoPassword, this.Browser, this.RootUri);

            var user = this.Context.Users.Single();

            user.UserName.Should().Be(UserS.PeshoUsername);
            user.Email.Should().Be(UserS.PeshoEmail);
        }

        [Fact]
        public void RegisterCreatesSettingsAndRootDirectoryeForRegisteredUser()
        {
            SeleniumActions.RegisterUser(UserS.PeshoUsername, UserS.PeshoEmail,
                UserS.PeshoPassword, this.Browser, this.RootUri);

            var user = this.Context.Users.Single();

            var settings = this.Context.UsersSettings.Single();
            settings.UserId.Should().Be(user.Id);

            var rootDirectory = this.Context.Directories.Single();
            rootDirectory.UserId.Should().Be(user.Id);
        }
    }
}


//var action = new OpenQA.Selenium.Interactions.Actions(this.Browser);
//action.ContextClick(element).Perform();
//this.Browser.Manage().Timeouts().();
//WebDriverWait wait = new WebDriverWait(this.Browser, TimeSpan.FromSeconds(0));
///Click the logout button if there are no other forms on the page,
//var element = this.Browser.FindElement(By.TagName("form"));
//element.Submit();
