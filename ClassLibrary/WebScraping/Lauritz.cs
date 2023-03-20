using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.WebScraping
{
    public class Lauritz
    {
        public int Varenummer { get; set; }
        public string ItemTitle { get; set; }
        public string BuyNowPrice { get; set; }
        public string NextBid { get; set; }
        public string PriceEstimate { get; set; }
        public string Description { get; set; }
        public List<string> ImageUrls { get; set; }
        public string ItemUrl { get; set; }
    }
}
