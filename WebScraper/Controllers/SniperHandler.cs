using ClassLibrary.WebScraping;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WebScraper.Controllers
{
    public class SniperHandler : Controller
    {
        private readonly IWebDriver _driver;
        private LauritzController _lauritzController;

        public SniperHandler()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless");
            chromeOptions.AddArgument("--disable-gpu");
            _driver = new ChromeDriver(chromeOptions);
            _lauritzController = new LauritzController();
        }

        public List<Lauritz> GetLauritz(string? arg)
        {
            var results = new List<Lauritz>();

            string lauritzUrl = "https://www.lauritz.com/da/";
            _driver.Navigate().GoToUrl(lauritzUrl);

            Thread.Sleep(1000);
            // Handle cookies
            _driver.FindElement(By.Id("CybotCookiebotDialogBodyButtonDecline")).Click();

            if (arg != null)
            {
                results = _lauritzController.SearchLauritz(arg, _driver);
            }


            return results;
        }
    }
}
