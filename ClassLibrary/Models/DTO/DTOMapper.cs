using ClassLibrary.DTOModels;

namespace ClassLibrary.Models.DTO
{
    public static class DTOMapper
    {
        public static ProductItemDTO MapProductItemToWebDTO(ProductItem pi)
        {
            var piDTO = new ProductItemDTO
            {
                Id = pi.Id,
                CurrentPrice = pi.CurrentPrice,
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
            piDTO.ImageUrls = imageUrls.ToArray();

            return piDTO;
        }

        public static OrderDTO mapOrderToOrderDTO(Order order)
        {
            List<int> productItemIds = new();
            foreach (var productItem in order.ProductItems)
            {
                productItemIds.Add(productItem.Id);
            }

            var ord = new OrderDTO
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                PaymentId = order.PaymentId,
                DiscountCodeId = order.DiscountCode?.Id != null ? order.DiscountCode.Id : 0,
                DeliveryStatus = order.DeliveryStatus,
                OrderStatus = order.OrderStatus,
                TotalPrice = order.TotalPrice,
                Active = order.Active,
                CreatedDate = order.CreatedDate,
                ProductItemIds = productItemIds.ToList()
            };

            return ord;
        }



        public static ProductItemDTO MapProductItemToBackofficeDTO(ProductItem pi)
        {
            var piDTO = new ProductItemDTO
            {
                Id = pi.Id,
                CurrentPrice = pi.CurrentPrice,
                PurchasePrice = pi.PurchasePrice,
                Sold = pi.Sold == 1 ? true : false,
                SoldDate = pi.SoldDate,
                CreatedDate = pi.CreatedDate,
                Condition = pi.Condition,
                Quality = pi.Quality,
                Weight = pi.Weight,
                CustomText = pi.CustomText != null || pi.CustomText != string.Empty ? pi.CustomText! : "",
                ProductId = pi.ProductId,
            };
            List<int> imageIds = new();
            List<string> imageUrls = new();
            foreach (var image in pi.Images)
            {
                imageIds.Add(image.Id);
                imageUrls.Add(image.Url);
            }
            piDTO.ImageIds = imageIds.ToArray();
            piDTO.ImageUrls = imageUrls.ToArray();

            List<int> priceHistoryIds = new();
            if (pi.PriceHistories != null)
            {
                foreach (PriceHistory priceHistory in pi.PriceHistories)
                {
                    priceHistoryIds.Add(priceHistory.Id);
                }
            }
            piDTO.PriceHistoryIds = priceHistoryIds.ToArray();

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
