using ClassLibrary.WebScraping;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using WebScraper.Controllers;

namespace WebScraper
{
    public class SniperContext : IDisposable
    {
        private readonly IWebDriver _driver;
        public SniperContext()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");
            chromeOptions.AddArgument("--disable-gpu");
            _driver = new ChromeDriver(chromeOptions);
        }

        public void Dispose()
        {
            _driver.Quit();
        }

        public List<Lauritz> GetLauritz(string? modelnumber, string? eanNumber, string? title)
        {
            var results = new List<Lauritz>();
            LauritzController lauritzController = new LauritzController();

            string lauritzUrl = "https://www.lauritz.com/da/";
            _driver.Navigate().GoToUrl(lauritzUrl);

            Thread.Sleep(1000);
            // Handle cookies
            _driver.FindElement(By.Id("CybotCookiebotDialogBodyButtonDecline")).Click();

            if (modelnumber != null)
            {
                results = lauritzController.SearchLauritz(modelnumber, _driver);
            }

            if (eanNumber != null) {
                results = lauritzController.SearchLauritz(eanNumber, _driver);
            }
            if (title != null)
            {
                results = lauritzController.SearchLauritz(title, _driver);
            }


            return results;
        }
    }
}
