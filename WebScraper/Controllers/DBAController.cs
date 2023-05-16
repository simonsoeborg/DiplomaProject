using ClassLibrary.WebScraping;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using HtmlAgilityPack;
using ScrapySharp.Extensions;
using ScrapySharp.Network;
using Newtonsoft.Json.Linq;
using System.Collections.Concurrent;
using SeleniumExtras.WaitHelpers;

namespace WebScraper.Controllers
{
    public class DBAController : Controller
    {
        private readonly ILogger<DBAController> _logger;

        public DBAController(ILogger<DBAController> logger)
        {
            _logger = logger;
        }

        public List<ScrapingModel> SearchDBA(string arg, IWebDriver _driver)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(0.100));
            var results = new ConcurrentBag<ScrapingModel>();

            var searchField = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("searchField")));

            if (arg != null)
            {
                searchField.SendKeys(arg);
                wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("button[class='btn btn-large btn-search'"))).Click();
                //html/body/div[4]/header/div/div/form/div/button

                wait.Until(ExpectedConditions.ElementExists(By.CssSelector("tr.dbaListing.listing")));
                var tableEntries = _driver.FindElements(By.CssSelector("tr.dbaListing.listing"));

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
                            IWebElement descriptionElement = null;
                            IWebElement script = null;
                            string description = "";
                            string scriptData = "";
                            JObject json = null;
                            var model = new ScrapingModel();

                            int i = tableEntries.IndexOf(tableEntry) + 1;

                            if (!(i >= 11))
                            {
                                wait.Until(d => d.FindElements(By.XPath("//script[@type='application/ld+json']")).Count > 0);
                                script = tableEntry.FindElement(By.XPath(".//script[@type='application/ld+json']"));
                                scriptData = script.GetAttribute("innerHTML"); 
                                descriptionElement = tableEntry.FindElement(By.XPath(".//div[@class='expandable-box expandable-box-collapsed']/a[@class='listingLink']"));

                                description = descriptionElement.Text;

                                json = JObject.Parse(scriptData);
                                string? imageUrl = json["image"]?.ToString();
                                string? name = json["name"]?.ToString();
                                string? itemUrl = json["url"]?.ToString();
                                string? price = json["offers"]["price"]?.ToString();

                                Console.WriteLine($"Image URL: {imageUrl}");
                                Console.WriteLine($"Item URL: {itemUrl}");
                                Console.WriteLine($"Price: {price}");
                                Console.WriteLine($"Description: {description}");

#pragma warning disable CS8601 // Possible null reference assignment.
                                model.DBAItemImages = imageUrl;
                                model.DBAItemLink = itemUrl;
                                model.DBAItemPrice = price;
                                model.DBAItemDescription = description;
                                model.Source = "dba.dk";
                                model.ItemTitle = name.Replace("&amp;quot;", "").Replace("...", "");
#pragma warning restore CS8601 // Possible null reference assignment.
                                results.Add(model);
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
    }
}
