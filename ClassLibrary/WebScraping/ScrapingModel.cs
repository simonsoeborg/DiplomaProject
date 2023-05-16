
namespace ClassLibrary.WebScraping
{
    public class ScrapingModel
    {
        public string Source { get; set; }
        public int Varenummer { get; set; }
        public string ItemTitle { get; set; } = null!;
        public string BuyNowPrice { get; set; } = null!;
        public string NextBid { get; set; } = null!;
        public string PriceEstimate { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<string> ImageUrls { get; set; } = new List<string>();
        public string ItemUrl { get; set; } = null!;
        public string DBAItemLink { get; set; } = null!;
        public string DBAItemPrice { get; set; } = null!;
        public string DBAItemImages { get; set; } = null!;
        public string DBAItemDescription { get; set; } = null!;
    }
}
