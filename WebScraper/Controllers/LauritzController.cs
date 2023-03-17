using ClassLibrary.WebScraping;
using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace WebScraper.Controllers
{
    public class LauritzController : Controller
    {
        public List<Lauritz> SearchLauritz(string arg, IWebDriver _driver)
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(2));
            var results = new List<Lauritz>();

            var searchFieldLauritz = wait.Until(ExpectedConditions.ElementIsVisible(By.Id("SearchTextBox")));

            if (arg != null)
            {
                searchFieldLauritz.SendKeys(arg);
                wait.Until(ExpectedConditions.ElementToBeClickable(By.Id("FilterControl_SearchButton"))).Click();
                // _driver.FindElement(By.Id("FilterControl_SearchButton")).Click();

                var tableEntries = _driver.FindElements(By.CssSelector("div[class='lotList item']"));

                // Regex regex = new Regex(@"List_LotListRepeater_ctl(\d{2})_(ItemTitle|Price|Valuation|ItemDescription|LotIdLabel)");

                int i = 1;

                foreach (IWebElement tableEntry in tableEntries)
                {
                    string getItemTitle = "";
                    string getPrice = "";
                    string getValuation = "";
                    string getItemDesc = "";
                    string getItemLotIdLabel = "";
                    string getImageURL = "";

                    if(i < 9)
                    {
                        var zero = 0;
                        getItemTitle = wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl{zero}{i}_ItemTitle"))).Text;
                        getPrice = wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl{zero}{i}_PriceLabel"))).Text;
                        getValuation = wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl{zero}{i}_ValuationLabel"))).Text;
                        getItemDesc = wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl{zero}{i}_ItemDescription"))).Text;
                        getItemLotIdLabel = wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl{zero}{i}_LotIdLabel"))).Text;
                        getImageURL = wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl{zero}{i}_itemImage"))).GetAttribute("src");
                    } else
                    {
                        getItemTitle = wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl{i}_ItemTitle"))).Text;
                        getPrice = wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl{i}_Price"))).Text;
                        getValuation = wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl{i}_ValuationLabel"))).Text;
                        getItemDesc = wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl{i}_ItemDescription"))).Text;
                        getImageURL = wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl{i}_itemImage"))).GetAttribute("src");
                        getItemLotIdLabel = wait.Until(ExpectedConditions.ElementIsVisible(By.Id($"List_LotListRepeater_ctl{i}_LotIdLabel"))).Text;
                    }


                    string price = getPrice;
                    string itemTitle = getItemTitle;
                    string valuation = getValuation;
                    string description = getItemDesc;
                    string lotId = getItemLotIdLabel.Split(":")[1];

                    var imageUrls = new List<string>();
                    imageUrls.Add(getImageURL);
                    var data = new Lauritz();
                    data.ItemTitle = itemTitle;
                    data.Varenummer = int.Parse(lotId);
                    data.Description = description;
                    data.NextBid = price;
                    data.PriceEstimate = valuation;
                    data.ImageUrls = imageUrls;
                    results.Add(data);

                    i++;
                }
            }

            return results;
        }
    }
}
