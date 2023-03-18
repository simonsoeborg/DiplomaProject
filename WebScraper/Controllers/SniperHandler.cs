﻿using ClassLibrary.WebScraping;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium;

namespace WebScraper.Controllers
{
    public class SniperHandler : Controller
    {
        private readonly IWebDriver _driver;
        private LauritzController _lauritzController;
        private IServiceProvider _serviceProvider;

        public SniperHandler()
        {
            if(_driver == null)
            {
                _driver = _serviceProvider.GetService<IWebDriver>();
            }
        }
        public SniperHandler(IServiceProvider serviceProvider) => _driver = serviceProvider.GetService<IWebDriver>();

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
