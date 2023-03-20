using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace WebScraper.Controllers
{
    public interface ISniperController
    {
        IWebElement HandleItemSearch(int index, string itemString, WebDriverWait wait);
        string HandleImageSearch(int index, string itemString, WebDriverWait wait);
        string HandleHrefExtractor(int index, string itemString, WebDriverWait wait);
    }
}
