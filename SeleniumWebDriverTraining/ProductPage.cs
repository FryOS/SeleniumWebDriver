using OpenQA.Selenium;

namespace SeleniumWebDriverTraining
{
    static public class ProductPage
    {

        static private By addToCart;
        static private By quantity;

        static public By AddToCart
        {
            get
            {
                if (addToCart == null)
                {
                    addToCart = By.Name("add_cart_product");
                }
                return addToCart;
            }
        }

        static public By Quantity
        {
            get
            {
                if (quantity == null)
                {
                    quantity = By.CssSelector("#cart > a.content > span.quantity");
                }
                return quantity;
            }
        }

    }
}
