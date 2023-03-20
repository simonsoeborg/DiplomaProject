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
        public List<Lauritz> SearchLauritz(string arg, IWebDriver _driver)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(0.100));
            var results = new ConcurrentBag<Lauritz>();

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

                            if (!(i >= tableEntries.Count))
                            {
                                if (HandleItemSearch(i, itemStringPriceLabel, wait) != null && !string.IsNullOrEmpty(HandleItemSearch(i, itemStringPriceLabel, wait).Text))
                                {
                                    getPrice = HandleItemSearch(i, itemStringPriceLabel, wait).Text;
                                }
                                else
                                {
                                    getPrice = "";
                                }

                                if (HandleItemSearch(i, itemStringValuation, wait) != null && !string.IsNullOrEmpty(HandleItemSearch(i, itemStringValuation, wait).Text))
                                {
                                    getValuation = HandleItemSearch(i, itemStringValuation, wait).Text;
                                }
                                else
                                {
                                    getValuation = "";
                                }

                                if (HandleItemSearch(i, itemStringBuyNowLabel, wait) != null && !string.IsNullOrEmpty(HandleItemSearch(i, itemStringBuyNowLabel, wait).Text))
                                {
                                    getBuyNowPriceLabel = HandleItemSearch(i, itemStringBuyNowLabel, wait).Text;
                                }
                                else
                                {
                                    getBuyNowPriceLabel = "";
                                }

                                getItemTitle = HandleItemSearch(i, itemStringItemLabel, wait).Text;
                                getItemDesc = HandleItemSearch(i, itemStringItemDescription, wait).Text;
                                getItemLotIdLabel = HandleItemSearch(i, itemStringLotIdLabel, wait);
                                getImageURL = HandleImageSearch(i, itemStringImageUrl, wait);
                                getItemURL = HandleHrefExtractor(i, itemStringItemUrl, wait);
                                var strongElement = getItemLotIdLabel.FindElement(By.TagName("strong"));
                                lotId = int.Parse(strongElement.Text);

                                var imageUrls = new List<string>();
                                imageUrls.Add(getImageURL);
                                var data = new Lauritz
                                {
                                    ItemTitle = getItemTitle,
                                    Varenummer = lotId,
                                    Description = getItemDesc,
                                    NextBid = getPrice,
                                    PriceEstimate = getValuation,
                                    ImageUrls = imageUrls,
                                    ItemUrl = getItemURL,
                                    BuyNowPrice = getBuyNowPriceLabel
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

        public IWebElement HandleItemSearch(int index, string itemString, WebDriverWait wait)
        {
            try
            {
                if (index < 10)
                    return wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl0{index}_{itemString}")));
                else
                    return wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl{index}_{itemString}")));
            } catch (WebDriverTimeoutException e)
            {
                Console.WriteLine($"Error: {index}; {itemString} not found!");
            }
            return null;
        }

        public string HandleImageSearch(int index, string itemString, WebDriverWait wait)
        {
            if(index < 10)
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
