using ClassLibrary.WebScraping;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System.Collections.Concurrent;

namespace WebScraper.Controllers
{
    public class LauritzController : Controller, ISniperController
    {
        private readonly ILogger<LauritzController> _logger;

        public LauritzController(ILogger<LauritzController> logger)
        {
            _logger = logger;
        }

        public List<ScrapingModel> SearchLauritz(string arg, IWebDriver _driver)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(0.100));
            var results = new ConcurrentBag<ScrapingModel>();

            string itemStringItemLabel = "ItemTitle";
            string itemStringItemDescription = "ItemDescription";
            string itemStringPriceLabel = "PriceLabel";
            string itemStringValuation = "ValuationLabel";
            string itemStringLotIdLabel = "LotIdLabel";
            string itemStringImageUrl = "itemImage";
            string itemStringItemUrl = "ItemTitle";
            string itemStringBuyNowLabel = "BuyNowPriceLabel";

            var searchFieldLauritz = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("SearchTextBox")));

            if (arg != null)
            {
                searchFieldLauritz.SendKeys(arg);
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("FilterControl_SearchButton"))).Click();
                var tableEntries = _driver.FindElements(By.CssSelector("div[class='lotList item']"));

                int chunkSize = 10;
                var chunks = tableEntries.Select((value, index) => new { Index = index, Value = value })
                                          .GroupBy(x => x.Index / chunkSize)
                                          .Select(x => x.Select(v => v.Value).ToList())
                                          .ToList();

                List<Thread> threads = new List<Thread>();
                foreach (var chunk in chunks)
                {
                    var thread = new Thread(() =>
                    {
                        foreach (IWebElement tableEntry in chunk)
                        {
                            string getItemTitle = "";
                            string getPrice = "";
                            string getValuation = "";
                            string getItemDesc = "";
                            IWebElement getItemLotIdLabel;
                            string getImageURL = "";
                            string getItemURL = "";
                            string getBuyNowPriceLabel = "";
                            int lotId = 0;

                            int i = tableEntries.IndexOf(tableEntry) + 1;

                            if (!(i >= 11))
                            {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                                if (HandleItemSearch(i, itemStringPriceLabel, wait) != null && !string.IsNullOrEmpty(HandleItemSearch(i, itemStringPriceLabel, wait).Text))
                                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                                    getPrice = HandleItemSearch(i, itemStringPriceLabel, wait).Text;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                                }
                                else
                                {
                                    getPrice = "";
                                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                                if (HandleItemSearch(i, itemStringValuation, wait) != null && !string.IsNullOrEmpty(HandleItemSearch(i, itemStringValuation, wait).Text))
                                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                                    getValuation = HandleItemSearch(i, itemStringValuation, wait).Text;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                                }
                                else
                                {
                                    getValuation = "";
                                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                                if (HandleItemSearch(i, itemStringBuyNowLabel, wait) != null && !string.IsNullOrEmpty(HandleItemSearch(i, itemStringBuyNowLabel, wait).Text))
                                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                                    getBuyNowPriceLabel = HandleItemSearch(i, itemStringBuyNowLabel, wait).Text;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                                }
                                else
                                {
                                    getBuyNowPriceLabel = "";
                                }
#pragma warning restore CS8602 // Dereference of a possibly null reference.

#pragma warning disable CS8602 // Dereference of a possibly null reference.
                                getItemTitle = HandleItemSearch(i, itemStringItemLabel, wait).Text;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                                getItemDesc = HandleItemSearch(i, itemStringItemDescription, wait).Text;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                                getItemLotIdLabel = HandleItemSearch(i, itemStringLotIdLabel, wait);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                                getImageURL = HandleImageSearch(i, itemStringImageUrl, wait);
                                getItemURL = HandleHrefExtractor(i, itemStringItemUrl, wait);
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                                var strongElement = getItemLotIdLabel.FindElement(By.TagName("strong"));
#pragma warning restore CS8602 // Dereference of a possibly null reference.
                                lotId = int.Parse(strongElement.Text);

                                var imageUrls = new List<string>();
                                imageUrls.Add(getImageURL);
                                var data = new ScrapingModel
                                {
                                    ItemTitle = getItemTitle,
                                    Varenummer = lotId,
                                    Description = getItemDesc,
                                    NextBid = getPrice,
                                    PriceEstimate = getValuation,
                                    ImageUrls = imageUrls,
                                    ItemUrl = getItemURL,
                                    BuyNowPrice = getBuyNowPriceLabel,
                                    Source = "Lauritz.com"
                                };
                                results.Add(data);
                            }
                        }
                    });
                    threads.Add(thread);
                    thread.Start();
                }

                foreach (Thread thread in threads)
                {
                    thread.Join();
                }
            }

            return results.ToList();
        }

#pragma warning disable CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
        public IWebElement? HandleItemSearch(int index, string itemString, WebDriverWait wait)
#pragma warning restore CS8766 // Nullability of reference types in return type doesn't match implicitly implemented member (possibly because of nullability attributes).
        {
            try
            {
                if (index < 10)
                    return wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl0{index}_{itemString}")));
                else
                    return wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl{index}_{itemString}")));
            }
            catch (WebDriverTimeoutException)
            {
                Console.WriteLine($"Error: {index}; {itemString} not found!");
                _logger.LogInformation($"Error: {index}; {itemString} not found!");
            }
            return null;
        }

        public string HandleImageSearch(int index, string itemString, WebDriverWait wait)
        {
            if (index < 10)
                return wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl0{index}_{itemString}"))).GetAttribute("src");
            else
                return wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl{index}_{itemString}"))).GetAttribute("src");
        }

        public string HandleHrefExtractor(int index, string itemString, WebDriverWait wait)
        {
            if (index < 10)
                return wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl0{index}_{itemString}"))).GetAttribute("href");
            else
                return wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl{index}_{itemString}"))).GetAttribute("href");
        }
    }
}
