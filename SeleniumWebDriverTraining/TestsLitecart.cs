using System;
using System.Text;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Linq;
using System.Threading;
using System.IO;
using System.Reflection;

namespace SeleniumTests
{
    [TestFixture]
    public class TestsLitecart
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private string baseURL;
        private bool acceptNextAlert = true;
        private WebDriverWait wait;

        [SetUp]
        public void SetupTest()
        {
            driver = new ChromeDriver();
            baseURL = "http://localhost:8080/litecart/";
            verificationErrors = new StringBuilder();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(30);
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        public void Test_LoginAdminPart()
        {
            LoginAdminPart();

        }

        [Test]
        public void Test_ClickOnMenuItems()
        {
            LoginAdminPart();

            var uls = driver.FindElement(By.Id("box-apps-menu"));
            var lis = uls.FindElements(By.Id("app-"));

            for (int i = 0; i < lis.Count; i++)
            {
                var liItem = driver.FindElements(By.CssSelector("li#app-"));
                liItem[i].Click();
                Thread.Sleep(1000);

                var h1s = driver.FindElement(By.Id("content")).FindElements(By.TagName("h1"));
                var lenthH1 = h1s.Count;
                Assert.NotZero(lenthH1);

                var ulsSub = driver.FindElements(By.CssSelector("ul.docs > li"));
                for (int y = 0; y < ulsSub.Count; y++)
                {
                    var lisSub = driver.FindElements(By.CssSelector("ul.docs > li"));
                    lisSub[y].Click();
                    var h1sSub = driver.FindElements(By.CssSelector("h1"));
                    Assert.IsTrue(h1sSub.Count > 0);
                }
            }

        }

        [Test]
        public void Test_CheckSticker()
        {
            driver.Navigate().GoToUrl(baseURL);
            Thread.Sleep(1000);
            var lis = driver.FindElements(By.ClassName("product"));
            foreach (var item in lis)
            {
                var stickerCount = item.FindElements(By.CssSelector("a.link>div.image-wrapper>div.sticker")).Count;
                var isStickerPresent = IsElementPresent(By.CssSelector("a.link>div.image-wrapper>div.sticker"), item);
                Assert.IsTrue(isStickerPresent);
                Assert.AreEqual(1, stickerCount);
            }

        }

        [Test]
        public void Test_CheckSortCountries()
        {
            driver.Navigate().GoToUrl(baseURL + "admin");
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Submit();
            driver.Navigate().GoToUrl("http://localhost:8080/litecart/admin/?app=countries&doc=countries");
            var trRowElements = driver.FindElements(By.CssSelector("tr> td:nth-child(5)>a"));
            List<string> countries = new List<string>();
            List<string> textContents = new List<string>();

            foreach (var trRowElement in trRowElements)
            {
                var textContent = trRowElement.GetAttribute("text");
                countries.Add(textContent);
                textContents.Add(textContent);
            }
            countries.Sort();
            Assert.AreEqual(countries, textContents);
        }

        [Test]
        public void Test_CheckSortCountriesZoneIfZoneEqual_0()
        {
            driver.Navigate().GoToUrl(baseURL + "admin");
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Submit();
            driver.Navigate().GoToUrl("http://localhost:8080/litecart/admin/?app=countries&doc=countries");
            var trRowElements = driver.FindElements(By.CssSelector("tr> td:nth-child(6)"));
            List<int> rowNumber = new List<int>();
            int i = 1;
            foreach (var trRowElement in trRowElements)
            {
                var textContent = trRowElement.GetAttribute("textContent");
                if (Int32.Parse(textContent) != 0)
                {
                    rowNumber.Add(i);
                }
                i++;
            }
            var rowNumberCount = rowNumber.Count;
            List<string> countries = new List<string>();
            List<string> textContents = new List<string>();
            foreach (var item in rowNumber)
            {
                driver.FindElement(By.CssSelector("tbody tr:nth-child("+(item+1)+") td:nth-child(5)>a")).Click();
                var trRowElementsZone = driver.FindElements(By.CssSelector("tr>td:nth-child(3)>input[type=hidden]"));
                foreach (var trRowElementsZoneCountry in trRowElementsZone)
                {
                    var textContent = trRowElementsZoneCountry.GetAttribute("value");
                    countries.Add(textContent);
                    textContents.Add(textContent);
                }
                countries.Sort();
                Assert.AreEqual(countries, textContents);                
            }

        }

        [Test]
        public void Test_CheckSortCountriesZone()
        {
            driver.Navigate().GoToUrl(baseURL + "admin");
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Submit();
            driver.Navigate().GoToUrl("http://localhost:8080/litecart/admin/?app=geo_zones&doc=geo_zones");
            var trRowElementsA = driver.FindElements(By.CssSelector("tbody .row > td:nth-child(3) > a"));

            for (int i=0; i < trRowElementsA.Count; i++)
            {
                var trRowElementsAs = driver.FindElements(By.CssSelector("tbody .row > td:nth-child(3) > a"));
                trRowElementsAs[i].Click();
                CheckZones();      
                driver.Navigate().GoToUrl("http://localhost:8080/litecart/admin/?app=geo_zones&doc=geo_zones");
            }  
        }

        public void CheckZones() 
        {
            var trRowElementsA = driver.FindElements(By.XPath("//select[contains(@name,'zone_code')]"));
            List<string> selectCountries = new List<string>();
            List<string> selectCountriesSort = new List<string>();

            foreach (var item in trRowElementsA)
            {
                //arrSelect.Add(new SelectElement(item));
                string selectedValue = (new SelectElement(item)).SelectedOption.Text.Trim();
                selectCountries.Add(selectedValue);
                selectCountriesSort.Add(selectedValue);
            }
            selectCountriesSort.Sort();
            Assert.AreEqual(selectCountries, selectCountriesSort);
        }


        [Test]
        public void Test_CheckItem()
        {
            driver.Navigate().GoToUrl(baseURL);
            var itemName = driver.FindElement(By.CssSelector("#box-campaigns div.name"));
            var itemNameContent = itemName.GetAttribute("textContent");
            var itemnameColor = itemName.GetCssValue("color");


            var itemRegularPrice = driver.FindElement(By.CssSelector("#box-campaigns .regular-price"));
            var itemRegularPriceContent = itemRegularPrice.GetAttribute("textContent");
            var itemRegularPriceColor = itemRegularPrice.GetCssValue("color");
            var itemRegularPriceFont = itemRegularPrice.GetCssValue("font-size");
            var itemRegularPriceFontDouble = (float)Convert.ToDouble(itemRegularPriceFont);

            string[] separators = { ",", "(", ")" };
            string[] itemRegularPriceColorRGBA = itemRegularPriceColor.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var itemRegularPriceColorRGBATrim = MyTrim(itemRegularPriceColorRGBA);            


            var itemCampaignPrice = driver.FindElement(By.CssSelector("#box-campaigns .campaign-price"));
            var itemCampaignPriceContent = itemCampaignPrice.GetAttribute("textContent");
            var itemCampaignPriceColor = itemCampaignPrice.GetCssValue("color");
            var itemCampaignPriceFont = itemCampaignPrice.GetCssValue("font-size");
            var itemCampaignPriceFontDouble = (float)Convert.ToDouble(itemCampaignPriceFont);

            string[] itemCampaignPriceColorRGBA = itemCampaignPriceColor.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            var itemCampaignPriceColorRGBATrim = MyTrim(itemCampaignPriceColorRGBA);

            var elem = driver.FindElement(By.CssSelector("#box-campaigns a.link"));

            elem.Click();

            var itemNameMain = driver.FindElement(By.CssSelector("h1.title"));
            var itemNameMainContent = itemNameMain.GetAttribute("textContent");

            var itemRegularPriceMain = driver.FindElement(By.CssSelector(".price-wrapper .regular-price"));
            var itemRegularPriceMainContent = itemRegularPriceMain.GetAttribute("textContent");
            var itemRegularPriceMainFont = itemRegularPriceMain.GetCssValue("font-size");
            var itemRegularPriceMainFontDouble = (float)Convert.ToDouble(itemRegularPriceMainFont);

            var itemCampaignPriceMain = driver.FindElement(By.CssSelector(".price-wrapper .campaign-price"));
            var itemCampaignPriceMainContent = itemCampaignPriceMain.GetAttribute("textContent");
            var itemCampaignPriceMainFont = itemCampaignPriceMain.GetCssValue("font-size");
            var itemCampaignPriceMainFontDouble = (float)Convert.ToDouble(itemCampaignPriceMainFont);

            Assert.AreEqual(itemNameContent, itemNameMainContent);
            Assert.AreEqual(itemRegularPriceContent, itemRegularPriceMainContent);
            Assert.AreEqual(itemCampaignPriceContent, itemCampaignPriceMainContent);

            Assert.AreEqual(itemRegularPriceColorRGBATrim[1], itemRegularPriceColorRGBATrim[2]);
            Assert.AreEqual(itemRegularPriceColorRGBATrim[2], itemRegularPriceColorRGBATrim[3]);
            Assert.AreEqual(itemRegularPriceColorRGBATrim[1], itemRegularPriceColorRGBATrim[3]);

            Assert.AreEqual(itemCampaignPriceColorRGBATrim[2], itemCampaignPriceColorRGBATrim[3]);
            Assert.AreEqual(itemCampaignPriceColorRGBATrim[2], "0");
            Assert.AreEqual(itemCampaignPriceColorRGBATrim[3], "0");
            
            Assert.Greater(itemCampaignPriceFontDouble, itemRegularPriceFontDouble);
            Assert.Greater(itemCampaignPriceMainFontDouble, itemRegularPriceMainFontDouble);
        }

        [Test]
        public void Test_UserRegistration()
        {
            Random rnd = new Random();
            var str = rnd.Next(100);

            string myemail = "AlexOsp"+str+"@mail.ru";

            driver.Navigate().GoToUrl(baseURL);
            var newCustomer = driver.FindElement(By.CssSelector("table tbody tr td>a"));
            newCustomer.Click();
            Thread.Sleep(1000);

            var firstname = driver.FindElement(By.CssSelector("input[name=firstname]"));
            firstname.SendKeys("123");

            var lastname = driver.FindElement(By.CssSelector("input[name=lastname]"));
            lastname.SendKeys("123");

            var address1 = driver.FindElement(By.CssSelector("input[name=address1]"));
            address1.SendKeys("Voss");

            var postcode = driver.FindElement(By.CssSelector("input[name=postcode]"));
            postcode.SendKeys("12312");

            var city = driver.FindElement(By.CssSelector("input[name=city]"));
            city.SendKeys("Nov");

            var email = driver.FindElement(By.CssSelector("input[name=email]"));
            email.SendKeys(myemail);

            var phone = driver.FindElement(By.CssSelector("input[name=phone]"));
            phone.SendKeys("123123123");

            var password = driver.FindElement(By.CssSelector("input[name=password]"));
            password.SendKeys("123123");

            var confirmed_password = driver.FindElement(By.CssSelector("input[name=confirmed_password]"));
            confirmed_password.SendKeys("123123");

            SelectElement selectCountry = new SelectElement(driver.FindElement(By.CssSelector("select[name=country_code]")));
            selectCountry.SelectByText("United States");

            SelectElement selectZoneCode = new SelectElement(driver.FindElement(By.CssSelector("select[name=zone_code]")));
            selectZoneCode.SelectByValue("AK");

            driver.FindElement(By.CssSelector("button[type=submit]")).Click();

            Thread.Sleep(500);
            driver.FindElement(By.LinkText("Logout")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.CssSelector("input[name=email]")).SendKeys(myemail);
            driver.FindElement(By.CssSelector("input[name=password]")).SendKeys("123123");
            
            driver.FindElement(By.CssSelector("button[type=submit]")).Click();
        }



        [Test]
        public void Test_AddNewProduct()
        {
            LoginAdminPart();
            Thread.Sleep(500);
            string servicePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\duckOmon.jpg";
            driver.FindElement(By.XPath("(//li[@id='app-']/a/span[2])[2]")).Click();
            Click(By.LinkText("Add New Product"));
            driver.FindElement(By.Name("status")).Click();
            driver.FindElement(By.Name("name[en]")).Click();
            driver.FindElement(By.Name("name[en]")).Clear();
            
            driver.FindElement(By.Name("name[en]")).SendKeys("12356");
            Thread.Sleep(1000);
            driver.FindElement(By.Name("code")).Click();
            driver.FindElement(By.Name("code")).Clear();
            driver.FindElement(By.Name("code")).SendKeys("65465");
            driver.FindElement(By.Name("product_groups[]")).Click();
            driver.FindElement(By.XPath("(//input[@name='product_groups[]'])[2]")).Click();
            //driver.FindElement(By.Name("new_images[]")).Click();
            driver.FindElement(By.Name("new_images[]")).Clear();
            driver.FindElement(By.Name("new_images[]")).SendKeys(servicePath);
            driver.FindElement(By.Name("date_valid_from")).Click();
            driver.FindElement(By.Name("date_valid_from")).Clear();
            driver.FindElement(By.Name("date_valid_from")).SendKeys(Keys.Home + "2020-06-16");
            driver.FindElement(By.Name("date_valid_to")).Click();
            driver.FindElement(By.Name("date_valid_to")).Clear();
            driver.FindElement(By.Name("date_valid_to")).SendKeys(Keys.Home + "2020-06-30");
            driver.FindElement(By.LinkText("Information")).Click();
            driver.FindElement(By.Name("manufacturer_id")).Click();
            new SelectElement(driver.FindElement(By.Name("manufacturer_id"))).SelectByText("ACME Corp.");
            driver.FindElement(By.XPath("(//option[@value='1'])[4]")).Click();
            driver.FindElement(By.Name("supplier_id")).Click();
            driver.FindElement(By.XPath("(//option[@value=''])[5]")).Click();
            driver.FindElement(By.Name("keywords")).Click();
            driver.FindElement(By.Name("keywords")).Clear();
            driver.FindElement(By.Name("keywords")).SendKeys("test");
            driver.FindElement(By.Name("short_description[en]")).Click();
            driver.FindElement(By.Name("short_description[en]")).Clear();
            driver.FindElement(By.Name("short_description[en]")).SendKeys("test");
            driver.FindElement(By.XPath("//div[@id='tab-information']/table/tbody/tr[5]/td/span/div/div[2]")).Click();
            
            driver.FindElement(By.Name("head_title[en]")).Click();
            driver.FindElement(By.Name("head_title[en]")).Clear();
            driver.FindElement(By.Name("head_title[en]")).SendKeys("testtest");
            driver.FindElement(By.Name("meta_description[en]")).Click();
            driver.FindElement(By.Name("meta_description[en]")).Clear();
            driver.FindElement(By.Name("meta_description[en]")).SendKeys("testtest");
            driver.FindElement(By.LinkText("Prices")).Click();
            driver.FindElement(By.Name("purchase_price")).Clear();
            driver.FindElement(By.Name("purchase_price")).SendKeys("10");
            driver.FindElement(By.Name("purchase_price")).Click();
            driver.FindElement(By.Name("purchase_price_currency_code")).Click();
            new SelectElement(driver.FindElement(By.Name("purchase_price_currency_code"))).SelectByText("US Dollars");
            driver.FindElement(By.XPath("//option[@value='USD']")).Click();
            driver.FindElement(By.Name("tax_class_id")).Click();
            driver.FindElement(By.XPath("(//option[@value=''])[7]")).Click();
            driver.FindElement(By.Name("prices[USD]")).Click();
            driver.FindElement(By.Name("prices[USD]")).Clear();
            driver.FindElement(By.Name("prices[USD]")).SendKeys("20");
            driver.FindElement(By.Name("prices[EUR]")).Click();
            driver.FindElement(By.Name("prices[EUR]")).Clear();
            driver.FindElement(By.Name("prices[EUR]")).SendKeys("18");
            Click(By.Name("save"));

            
        }

        [Test]
        public void Test_AddProdBasket_DelProdBusket() 
        {
            driver.Navigate().GoToUrl(baseURL);
            //driver.FindElement(By.XPath("//img[@alt='Green Duck']")).Click();
            driver.FindElement(By.XPath("//*[@id ='box-most-popular']/div/ul/li[1]")).Click();

            //div[@id='tab-information']/
            Thread.Sleep(2000);

        }


        //            Сделайте сценарий для добавления товаров в корзину и удаления товаров из корзины.

        //1) открыть главную страницу
        //2) открыть первый товар из списка
        //2) добавить его в корзину(при этом может случайно добавиться товар, который там уже есть, ничего страшного)
        //3) подождать, пока счётчик товаров в корзине обновится
        //4) вернуться на главную страницу, повторить предыдущие шаги ещё два раза, чтобы в общей сложности в корзине было 3 единицы товара
        //5) открыть корзину(в правом верхнем углу кликнуть по ссылке Checkout)
        //6) удалить все товары из корзины один за другим, после каждого удаления подождать, пока внизу обновится таблица













        [Test]
        public void TheUntitledTestCaseTest()
        {
            LoginAdminPart();
            Thread.Sleep(500);
            driver.Navigate().GoToUrl("http://localhost:8080/litecart/admin/");
            driver.FindElement(By.LinkText("Catalog")).Click();
            Click(By.LinkText("Add New Product"));
            Thread.Sleep(500);
            driver.FindElement(By.Name("status")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.Name("name[en]")).Click();
            driver.FindElement(By.Name("name[en]")).Clear();
            driver.FindElement(By.Name("name[en]")).SendKeys("test654");
            driver.FindElement(By.Name("code")).Click();
            driver.FindElement(By.Name("code")).Clear();
            driver.FindElement(By.Name("code")).SendKeys("123");
            driver.FindElement(By.Name("product_groups[]")).Click();
            driver.FindElement(By.XPath("(//input[@name='product_groups[]'])[2]")).Click();
            driver.FindElement(By.Name("quantity")).Click();
            driver.FindElement(By.Name("quantity")).Click();
            driver.FindElement(By.Name("quantity")).Clear();
            driver.FindElement(By.Name("quantity")).SendKeys("123");
            
            
            driver.FindElement(By.Name("new_images[]")).SendKeys(@"C:\Users\AlexeyOSp\source\repos\SeleniumWebDriverTraining\SeleniumWebDriverTraining\duckOmon.jpg");

            driver.FindElement(By.Name("date_valid_from")).Click();
            driver.FindElement(By.Name("date_valid_from")).Clear();
            driver.FindElement(By.Name("date_valid_from")).SendKeys(Keys.Home + "2020-06-25");
            driver.FindElement(By.Name("date_valid_to")).Click();
            driver.FindElement(By.Name("date_valid_to")).Clear();
            driver.FindElement(By.Name("date_valid_to")).SendKeys(Keys.Home + "2020-06-25");
            driver.FindElement(By.LinkText("Information")).Click();
            driver.FindElement(By.Name("manufacturer_id")).Click();
            new SelectElement(driver.FindElement(By.Name("manufacturer_id"))).SelectByText("ACME Corp.");
            driver.FindElement(By.XPath("(//option[@value='1'])[4]")).Click();
            driver.FindElement(By.Name("supplier_id")).Click();
            driver.FindElement(By.XPath("(//option[@value=''])[5]")).Click();
            driver.FindElement(By.Name("keywords")).Click();
            driver.FindElement(By.Name("keywords")).Clear();
            driver.FindElement(By.Name("keywords")).SendKeys("dfg");
            driver.FindElement(By.XPath("//div[@id='tab-information']/table/tbody/tr[5]/td/span/div/div[2]")).Click();
            driver.FindElement(By.Name("short_description[en]")).Click();
            driver.FindElement(By.Name("short_description[en]")).Clear();
            driver.FindElement(By.Name("short_description[en]")).SendKeys("dfg");
            driver.FindElement(By.XPath("//div[@id='tab-information']/table/tbody/tr[5]/td/span/div/div[2]")).Click();
            // ERROR: Caught exception [unknown command [editContent]]
            driver.FindElement(By.Name("head_title[en]")).Click();
            driver.FindElement(By.Name("head_title[en]")).Clear();
            driver.FindElement(By.Name("head_title[en]")).SendKeys("dfg");
            driver.FindElement(By.Name("meta_description[en]")).Click();
            driver.FindElement(By.Name("meta_description[en]")).Clear();
            driver.FindElement(By.Name("meta_description[en]")).SendKeys("dfg");
            driver.FindElement(By.LinkText("Prices")).Click();
            driver.FindElement(By.Name("purchase_price")).Click();
            driver.FindElement(By.Name("purchase_price")).Clear();
            driver.FindElement(By.Name("purchase_price")).SendKeys("123");
            driver.FindElement(By.Name("purchase_price_currency_code")).Click();
            new SelectElement(driver.FindElement(By.Name("purchase_price_currency_code"))).SelectByText("US Dollars");
            driver.FindElement(By.XPath("//option[@value='USD']")).Click();
            driver.FindElement(By.Name("tax_class_id")).Click();
            driver.FindElement(By.XPath("(//option[@value=''])[7]")).Click();
            driver.FindElement(By.Name("prices[USD]")).Click();
            driver.FindElement(By.Name("prices[USD]")).Clear();
            driver.FindElement(By.Name("prices[USD]")).SendKeys("12312");
            driver.FindElement(By.Name("prices[EUR]")).Click();
            driver.FindElement(By.Name("prices[EUR]")).Clear();
            driver.FindElement(By.Name("prices[EUR]")).SendKeys("33");
            driver.FindElement(By.LinkText("General")).Click();
            driver.FindElement(By.Name("save")).Click();
            Thread.Sleep(3000);
        }

        //Сделайте сценарий для добавления нового товара(продукта) в учебном приложении litecart(в админке).
        //Для добавления товара нужно открыть меню Catalog, в правом верхнем углу нажать кнопку "Add New Product", 
        //    заполнить поля с информацией о товаре и сохранить.
        //Достаточно заполнить только информацию на вкладках General, Information и Prices.Скидки(Campains) на вкладке Prices можно не добавлять.
        //Переключение между вкладками происходит не мгновенно, поэтому после переключения можно сделать 
        //    небольшую паузу(о том, как делать более правильные ожидания, будет рассказано в следующих занятиях).
        //Картинку с изображением товара нужно уложить в репозиторий вместе с кодом.
        //    При этом указывать в коде полный абсолютный путь к файлу плохо, на другой машине работать не будет.Надо средствами языка 
        //        программирования преобразовать относительный путь в абсолютный.
        //После сохранения товара нужно убедиться, что он появился в каталоге (в админке). Клиентскую часть магазина можно не проверять.


        //___________________
        public void SelectFromDropDown(By locator, string text)
        {
            SelectElement select = new SelectElement(driver.FindElement(locator));
            string selectedValue = select.SelectedOption.Text.Trim(); //нужно, если есть пробелы в начале и конце элемента drop-down листа
            if (selectedValue != text) //эта проверка нужна, т.к. мб случай когда уже корректное значение выбрано
            {
                driver.FindElement(locator).Click();
                Assert.True(driver.FindElement(By.XPath("//option[contains(text(), '" + text + "')]")).Displayed, string.Format("Could not find '{0}' in the list", text));
                var list = driver.FindElement(locator);
                var selectElement = new SelectElement(list);
                selectElement.SelectByText(text);
            }
        }


        public void LoginAdminPart()
        {
            driver.Navigate().GoToUrl(baseURL + "admin");
            driver.FindElement(By.Name("username")).SendKeys("admin");
            driver.FindElement(By.Name("password")).SendKeys("admin");
            driver.FindElement(By.Name("login")).Submit();
        }

        public static Func<IWebDriver, IWebElement> Condition(By locator)
        {
            return (driver) =>
            {
                var element = driver.FindElements(locator).FirstOrDefault();
                return element != null && element.Displayed && element.Enabled ? element : null;
            };
        }
        protected void Click(By locator)
        {
            WaitUntilClickable(locator).Click();
        }


        public void WaitUntilVisible(By locator)
        {
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
            wait.Message = "Element with locator '" + locator + "' was not visible in 10 seconds";
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(locator));
        }

        public IWebElement WaitUntilClickable(By locator)
        {
            wait = new WebDriverWait(driver, new TimeSpan(0, 0, 10));
            wait.Message = "Element with locator '" + locator + "' was not clickable in 10 seconds";
            return wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(locator));
        }

        public string[] MyTrim(string[] c)
        {            
            for (int i = 0; i < c.Length; i++)
                c[i] = c[i].Trim();
            return c;
        }

        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsElementPresent(By by, IWebElement element)
        {
            try
            {
                element.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
    }
}
