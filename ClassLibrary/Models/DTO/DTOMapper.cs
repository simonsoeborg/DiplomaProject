using ClassLibrary.DTOModels;

namespace ClassLibrary.Models.DTO
{
    public static class DTOMapper
    {
        public static ProductItemDTO MapProductItemToDTO(ProductItem pi)
        {
            var piDTO = new ProductItemDTO
            {
                Id = pi.Id,
                Price = pi.CurrentPrice,
                CreatedDate = pi.CreatedDate,
                Condition = pi.Condition,
                Quality = pi.Quality,
                Weight = pi.Weight,
                CustomText = pi.CustomText != null || pi.CustomText != string.Empty ? pi.CustomText! : "",
                ProductId = pi.ProductId,
            };
            List<string> imageUrls = new();
            foreach (var image in pi.Images)
            {
                imageUrls.Add(image.Url);
            }
            piDTO.Images = imageUrls.ToArray();

            return piDTO;
        }

        public static ProductDTO MapProductToDTO(Product p)
        {
            ProductDTO productDTO = new()
            {
                Id = p.Id,
                Name = p.Name,
                ModelNumber = p.ModelNumber,
                Manufacturer = p.Manufacturer,
                Material = p.Material,
                Design = p.Design != null ? p.Design : "",
                Dimension = p.Dimension != null ? p.Dimension : "",
                SubcategoryIds = new()
            };

            foreach (var subcategory in p.Subcategories)
            {
                productDTO.SubcategoryIds.Add(subcategory.Id);
            }

            return productDTO;
        }
    }
}
