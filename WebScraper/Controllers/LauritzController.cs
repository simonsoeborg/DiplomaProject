using ClassLibrary.WebScraping;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace WebScraper.Controllers
{
    public class LauritzController : Controller, ISniperController
    {
        public List<Lauritz> SearchLauritz(string arg, IWebDriver _driver)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
            var results = new List<Lauritz>();

            string itemStringItemLabel = "ItemTitle";
            string itemStringItemDescription = "ItemDescription";
            string itemStringPriceLabel = "PriceLabel";
            string itemStringValuation = "ValuationLabel";
            string itemStringLotIdLabel = "LotIdLabel";
            string itemStringImageUrl = "itemImage";
            string itemStringItemUrl = "ItemTitle";

            var searchFieldLauritz = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("SearchTextBox")));

            if (arg != null)
            {
                searchFieldLauritz.SendKeys(arg);
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("FilterControl_SearchButton"))).Click();
                var tableEntries = _driver.FindElements(By.CssSelector("div[class='lotList item']"));

                int i = 1;

                foreach (IWebElement tableEntry in tableEntries)
                {
                    string getItemTitle = "";
                    string getPrice = "";
                    string getValuation = "";
                    string getItemDesc = "";
                    IWebElement getItemLotIdLabel;
                    string getImageURL = "";
                    string getItemURL = "";
                    int lotId = 0;

                    if (!(i >= tableEntries.Count))
                    {
                        getItemTitle = HandleItemSearch(i, itemStringItemLabel, wait).Text;
                        getPrice = HandleItemSearch(i, itemStringPriceLabel, wait).Text;
                        getValuation = HandleItemSearch(i, itemStringValuation, wait).Text;
                        getItemDesc = HandleItemSearch(i, itemStringItemDescription, wait).Text;
                        getItemLotIdLabel = HandleItemSearch(i, itemStringLotIdLabel, wait);
                        var strongElement = getItemLotIdLabel.FindElement(By.TagName("strong"));
                        lotId = int.Parse(strongElement.Text);
                        getImageURL = HandleImageSearch(i, itemStringImageUrl, wait);
                        getItemURL = HandleHrefExtractor(i, itemStringItemUrl, wait);

                        string price = getPrice;
                        string itemTitle = getItemTitle;
                        string valuation = getValuation;
                        string description = getItemDesc;

                        var imageUrls = new List<string>();
                        imageUrls.Add(getImageURL);
                        var data = new Lauritz
                        {
                            ItemTitle = itemTitle,
                            Varenummer = lotId,
                            Description = description,
                            NextBid = price,
                            PriceEstimate = valuation,
                            ImageUrls = imageUrls,
                            ItemUrl = getItemURL
                        };

                        results.Add(data);
                    }

                    i++;
                }
            }

            return results;
        }

        public IWebElement HandleItemSearch(int index, string itemString, WebDriverWait wait)
        {
            if (index < 10)
                return wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl0{index}_{itemString}")));
            else
                return wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl{index}_{itemString}")));
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
