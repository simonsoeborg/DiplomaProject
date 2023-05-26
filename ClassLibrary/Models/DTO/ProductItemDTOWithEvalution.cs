
namespace ClassLibrary.Models.DTO
{
    public class ProductItemWithEvalution
    {    
        public ProductItem Productitem { get; set; } = null!;
        public Product Product { get; set; } = null!;
        public decimal SalesValue { get; set; }
    }
}
