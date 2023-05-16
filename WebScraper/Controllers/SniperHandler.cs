using ClassLibrary.WebScraping;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.Generic;

namespace WebScraper.Controllers
{
    public class SniperHandler : Controller
    {
        private readonly IWebDriver _driver;
        private LauritzController _lauritzController;
        private DBAController _dbaController;
        private bool hasRunnedOnce = false;
        private bool hasRunnedOnceDBA = false;

        public SniperHandler(IWebDriverService webDriverService, ILogger<LauritzController> lauritzLogger, ILogger<DBAController> dbaLogger)
        {
            _driver = webDriverService.driverService();
            _lauritzController = new LauritzController(lauritzLogger);
            _dbaController = new DBAController(dbaLogger);
        }

        public List<ScrapingModel> getDBA(string? arg)
        {
            var results = new List<ScrapingModel>();
            if (arg != null) { 
                if (arg.Contains(" "))
                {
                    arg = arg.Replace(" ", "+");
                }
                var url = "https://www.dba.dk/";
                _driver.Navigate().GoToUrl(url);
                Thread.Sleep(1000);
                if (!hasRunnedOnceDBA)
                    try
                    {
                        var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(20));
                        IWebElement iframe = _driver.FindElement(By.XPath("//iframe[contains(@src, 'samtykke.dba.dk')]"));

                        _driver.SwitchTo().Frame(iframe);

                        IWebElement CookieButton = _driver.FindElement(By.XPath("//button[@title='Kun nødvendige']"));
                        CookieButton.Click();

                        _driver.SwitchTo().DefaultContent();
                    }
                    catch (NoSuchElementException)
                    {
                        Console.WriteLine("Could not find Cookie Button");
                    }

                if (arg != null)
                {
                    results = _dbaController.SearchDBA(arg, _driver);
                    hasRunnedOnceDBA = true;
                }
                Console.WriteLine(url);

                return results;
            }
            else
            {
                return results;
            }
        }

        public List<ScrapingModel> GetLauritz(string? arg)
        {
            var results = new List<ScrapingModel>();

            string lauritzUrl = "https://www.lauritz.com/da/";
            _driver.Navigate().GoToUrl(lauritzUrl);

            Thread.Sleep(1000);
            // Handle cookies
            if (!hasRunnedOnce)
                try
                {
                    _driver.FindElement(By.Id("CybotCookiebotDialogBodyButtonDecline")).Click();
                }
                catch (NoSuchElementException e)
                {
                    Console.WriteLine("CybotCookiebotDialogBodyButtonDecline not found!" + e);
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
