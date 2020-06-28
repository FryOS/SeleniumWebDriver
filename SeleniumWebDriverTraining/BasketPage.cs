using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeleniumWebDriverTraining
{
    public class BasketPage
    {
        static private By checkOut;
        protected IWebDriver driver;

        static public By CheckOut
        {
            get
            {
                if (checkOut == null)
                {
                    checkOut = By.LinkText("Checkout »");
                }
                return checkOut;
            }
        }

        public ReadOnlyCollection<IWebElement> GetTrs()
        {
            return driver.FindElements(By.CssSelector(".dataTable.rounded-corners tr > td.item"));
        }
    }
}
