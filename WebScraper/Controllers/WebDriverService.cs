using OpenQA.Selenium;

namespace WebScraper.Controllers
{
    public class WebDriverService : IWebDriverService
    {
        private readonly IWebDriver _driver;

        public WebDriverService(IWebDriver webDriver)
        {
            _driver = webDriver;
        }

        public IWebDriver driverService()
        {
            return _driver;
        }
    }
}
