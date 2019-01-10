namespace Momento.SeleniumTests.Tests
{
    using System.Linq;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using OpenQA.Selenium.Interactions;
    using System;
    using OpenQA.Selenium.Support.UI;
    using Momento.Tests.Contracts;
    using Momento.Tests.Seeding;
    using FunApp.Web.Tests;
    using Web;
    using Xunit;
    using Momento.Tests.Utilities;
    using Seeding;
    using OpenQA.Selenium;

    public class ListToDoTest : SeleniumInMemoryDbBaseTest
    {
        public ListToDoTest(SeleniumServerFactoryInMemory<Startup> server) 
            : base(server, true)
        {
        }

        [Fact]
        public void ListToDoFullTest()
        {
            const string Item1Content = "Selenium First Item Content";
            const string Item1Comment = "Selenium First Item Content";
            const string Item2Content = "Selenium Second Item Content";
            const string Item2Comment = "Selenium Second Item Content";

            GeneralS.SeedRoles(this.Context);

            SeleniumActions.RegisterUser(UserS.PeshoUsername, UserS.PeshoEmail,
                UserS.PeshoPassword, this.Browser, this.RootUri);

            ///Clicking the create new ListToDo link
            this.Browser.FindElement(By.LinkText("Create List ToDo")).Click();
            
            ///Filling The listToDo Create forum
            this.Browser.FindElement(By.Name("Name")).SendKeys("SeleniumNote");
            this.Browser.FindElement(By.Name("Description")).SendKeys("SeleniumNoteDesc");
            this.Browser.FindElement(By.CssSelector(".btn-success")).Click();
            this.Browser.Navigate().Refresh();
            ///...

            ///Getting into the actual app
            this.Wait(1000, 1000, this.Browser);
            this.Browser.FindElement(By.LinkText("Use")).Click();
            ///...

            var addItemButtonId = "btn-create";
            var itemId = "box"; ///+Id
            var textAreaId = "text-area"; /// + Id
            var commentId = "comment"; /// + Id

            ///Creating two items
            var addItemButton = this.Browser.FindElement(By.Id(addItemButtonId));
            addItemButton.Click();
            addItemButton.Click();

            var box1 = this.Browser.FindElement(By.Id(itemId + "0"));
            var content1 = this.Browser.FindElement(By.Id(textAreaId + "0"));
            var comment1 = this.Browser.FindElement(By.Id(commentId + "0"));
            box1.Click();
            content1.SendKeys(Item1Content);
            Actions actions = new Actions(this.Browser);
            actions.MoveToElement(comment1);
            actions.Click();
            actions.SendKeys(Item1Comment);
            actions.Build().Perform();

            var box2 = this.Browser.FindElement(By.Id(itemId + "1"));
            var content2 = this.Browser.FindElement(By.Id(textAreaId + "1"));
            var comment2 = this.Browser.FindElement(By.Id(commentId + "1"));
            box2.Click();
            content2.SendKeys(Item2Content);
            Actions actions2 = new Actions(this.Browser);
            actions2.MoveToElement(comment2);
            actions2.Click();
            actions2.SendKeys(Item2Comment);
            actions2.Build().Perform();
            ///...
            
            ///Moving Item two to the Active Category

            //var newCategoryElement = this.Browser
            //    .FindElement(By.Id("heading-Active"));

            //Actions dragAndDrop = new Actions(this.Browser);
            //dragAndDrop.ClickAndHold(box1);
            //dragAndDrop.MoveToElement(newCategoryElement);
            //dragAndDrop.Release(box1);
            //dragAndDrop.Build().Perform();
            ///...
            
            ///Saving the result:
            this.Browser.FindElement(By.Id("btn-save-list")).Click();

            this.Context.ListsTodo.Count().Should().Be(1);
            var list = this.Context.ListsTodo
                .Include(x=>x.Items).
                Single();
            list.Items.Count.Should().Be(2);
            var item1 = list.Items.SingleOrDefault(x => x.Content == Item1Content);
            var item2 = list.Items.SingleOrDefault(x => x.Content == Item2Content);
            item1.Comment.Should().Be(Item1Comment);
            item2.Comment.Should().Be(Item2Comment);
        }

        public void Wait(double delay, double interval, IWebDriver myWebDriver)
        {
            // Causes the WebDriver to wait for at least a fixed delay
            var now = DateTime.Now;
            var wait = new WebDriverWait(myWebDriver, TimeSpan.FromMilliseconds(delay));
            wait.PollingInterval = TimeSpan.FromMilliseconds(interval);
            wait.Until(wd => (DateTime.Now - now) - TimeSpan.FromMilliseconds(delay) > TimeSpan.Zero);
        }
    }
}
