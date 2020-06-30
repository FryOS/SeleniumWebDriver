using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
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
        static private By removeCartItem;
        protected IWebDriver driver;
        private WebDriverWait wait;

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

        static public By RemoveCartItem
        {
            get
            {
                if (removeCartItem == null)
                {
                    removeCartItem = By.Name("remove_cart_item »");
                }
                return removeCartItem;
            }
        }

        public ReadOnlyCollection<IWebElement> GetTrs()
        {
            return driver.FindElements(By.CssSelector(".dataTable.rounded-corners tr > td.item"));
        }        
    }
}
