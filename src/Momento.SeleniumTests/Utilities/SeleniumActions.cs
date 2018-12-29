namespace Momento.Tests.Utilities
{
    using OpenQA.Selenium;
    using Momento.Services.Utilities; 

    public static class SeleniumActions
    {
        public static void LoginUser(string username, string password, IWebDriver browser, string rootUri)
        {
            browser.Navigate().GoToUrl(rootUri + "/Identity/Account/Login");

            browser.FindElement(By.Name("Input.Username")).SendKeys(username);

            browser.FindElement(By.Name("Input.Password")).SendKeys(password);

            browser.FindElement(By.TagName("form")).Submit();

            browser.Navigate().GoToUrl(rootUri + Constants.ReactAppPath);
        }


        public static void RegisterUser(string username, string emain, string password, IWebDriver browser, string rootUri)
        {
            browser.Navigate().GoToUrl(rootUri + "/Identity/Account/Register");

            browser.FindElement(By.Name("Input.Username")).SendKeys(username);

            browser.FindElement(By.Name("Input.Email")).SendKeys(emain);

            browser.FindElement(By.Name("Input.Password")).SendKeys(password);

            browser.FindElement(By.Name("Input.ConfirmPassword")).SendKeys(password);

            browser.FindElement(By.TagName("form")).Submit();
        }

        public static void RegisterRightClick(IWebDriver browser, IWebElement element)
        {
            var action = new OpenQA.Selenium.Interactions.Actions(browser);
            action.ContextClick(element).Perform();
        }

        /// <summary>
        /// Can be used as logout if there are no other firms on the page
        /// </summary>
        /// <param name="browser"></param>
        public static void SubmitFormIfOnlyOnePresent(IWebDriver browser)
        {
            browser.FindElement(By.TagName("form")).Submit();
        }
    }
}
