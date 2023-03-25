
namespace ClassLibrary.WebScraping
{
    public class Lauritz
    {
        public int Varenummer { get; set; }
        public string ItemTitle { get; set; } = null!;
        public string BuyNowPrice { get; set; } = null!;
        public string NextBid { get; set; } = null!;
        public string PriceEstimate { get; set; } = null!;
        public string Description { get; set; } = null!;
        public List<string> ImageUrls { get; set; } = new List<string>();
        public string ItemUrl { get; set; } = null!;
    }
}
