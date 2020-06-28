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

    }
}
