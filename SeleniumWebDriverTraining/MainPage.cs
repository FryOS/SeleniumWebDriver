using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace SeleniumWebDriverTraining
{
    public class MainPage
    {
        protected IWebDriver driver;
        private WebDriverWait wait; 
        

        public MainPage()
        {
            driver  = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
        }
        

        public void OpenBasePage(string baseURL) { driver.Navigate().GoToUrl(baseURL); }

        public IWebElement FirstPopular() { return driver.FindElement(By.XPath("//*[@id ='box-most-popular']/div/ul/li[1]")); }

        public void AddProductToCartMainPage(string baseURL)
        {
            OpenBasePage(baseURL);
            FirstPopular().Click(); 
            IWebElement addToCartButton = wait.Until(ExpectedConditions.ElementExists(ProductPage.AddToCart));
            addToCartButton.Click();
        }

        public void WaitUntil(string i)
        {
            wait.Until(d => d.FindElement(ProductPage.Quantity).Text.Contains(i.ToString()));
        }

        public IWebElement WaitUntil(By locator)
        {
            return wait.Until(d => d.FindElement(locator));
        }

        public void Refresh()
        {
            driver.Navigate().Refresh();
        }

        public void WaitStalenessOf(IWebElement item)
        {
            wait.Until(ExpectedConditions.StalenessOf(item));
        }

        public IWebElement WaitUntilClickable(By locator)
        {
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
            wait.Message = "Element with locator '" + locator + "' was not clickable in 10 seconds";
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
        }

        public void WaitUntilVisible(By locator)
        {
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
            wait.Message = "Element with locator '" + locator + "' was not visible in 10 seconds";
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
        }

    }
}
