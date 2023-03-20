using ClassLibrary.WebScraping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;

namespace WebScraper.Controllers
{
    public class SniperHandler : Controller
    {
        private readonly IWebDriver _driver;
        private LauritzController _lauritzController;
        private bool hasRunnedOnce = false;

        public SniperHandler(IWebDriverService webDriverService, ILogger<LauritzController> logger)
        {
            _driver = webDriverService.driverService();
            _lauritzController = new LauritzController(logger);
        }

        public List<Lauritz> GetLauritz(string? arg)
        {
            var results = new List<Lauritz>();

            string lauritzUrl = "https://www.lauritz.com/da/";
            _driver.Navigate().GoToUrl(lauritzUrl);

            Thread.Sleep(1000);
            // Handle cookies
            if(!hasRunnedOnce)
                try
                {
                    _driver.FindElement(By.Id("CybotCookiebotDialogBodyButtonDecline")).Click();
                } catch (NoSuchElementException e)
                {
                    Console.WriteLine("CybotCookiebotDialogBodyButtonDecline not found!");
                }

            if (arg != null)
            {
                results = _lauritzController.SearchLauritz(arg, _driver);
                hasRunnedOnce = true;
            }

            return results;
        }
    }
}
