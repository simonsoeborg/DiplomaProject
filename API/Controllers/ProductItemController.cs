using ClassLibrary;
using ClassLibrary.DTOModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace API.Controllers

{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductItemController : ControllerBase
    {
        private readonly GroenlundDbContext _context;

        public ProductItemController(GroenlundDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetProductItems")]
        public async Task<ActionResult<IEnumerable<ProductItem>>> GetProductItems()
        {
            var productItems = await _context.ProductItems
                .Include(pi => pi.Product)
                .Include(pi => pi.Images)
                .Include(pi => pi.PriceHistories)
                .ToListAsync();

            if (productItems == null || productItems.Count == 0)
            {
                return new NoContentResult();
            }
            return productItems;
        }

        [HttpGet("GetProductItemDTOs")]
        public async Task<ActionResult<IEnumerable<ProductItemDTO>>> GetProductItemDTOs()
        {
            var productItems = await _context.ProductItems
                .Include(pi => pi.Images)
                .Include(pi => pi.PriceHistories)
                .Include(pi => pi.Product)
                .ToListAsync();


            if (productItems == null || productItems.Count == 0)
            {
                return new NoContentResult();
            }
            List<ProductItemDTO> productItemDTOs = new();
            foreach (var productItem in productItems)
            {
                productItemDTOs.Add(MapProductItemToDTO(productItem));
            }
            return productItemDTOs;
        }


        [EnableCors]
        [HttpPost]
        public HttpResponseMessage Post([FromBody] ProductItem req)
        {
            if (PropertiesHasValues(req))
            {
                // Removing ID property from request since database auto-increments.
                ProductItem reqProductItem = new()
                {
                    ProductId = req.ProductId,
                    Condition = req.Condition,
                    Quality = req.Quality,
                    Sold = req.Sold,
                };
                if (req.Weight != null) reqProductItem.Weight = req.Weight;
                _context.ProductItems.Add(reqProductItem);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            try
            {
                _context.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProductItem(int id, [FromBody] ProductItem req)
        {
            var productItem = _context.ProductItems.Find(id);

            if (productItem == null)
            {
                return NotFound();
            }

            if (!PropertiesHasValues(req))
            {
                return BadRequest();
            }


            productItem.ProductId = req.ProductId;
            productItem.Condition = req.Condition;
            productItem.Quality = req.Quality;
            productItem.Sold = req.Sold;

            if (req.Weight != null) productItem.Weight = req.Weight;

            try
            {
                _context.SaveChanges();
                return NoContent();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return NoContent();
            }
        }


        [HttpDelete("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            var productItems = _context.ProductItems.Find(id);

            if (productItems == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            _context.ProductItems.Remove(productItems);

            try
            {
                _context.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }


        private static bool PropertiesHasValues(ProductItem productItem)
        {
            if (
                productItem.ProductId <= 0
                || productItem.Condition > 0
                || productItem.Quality > 0
            )
            {
                return false;
            }

            return true;
        }
        private static ProductItemDTO MapProductItemToDTO(ProductItem pi)
        {
            var piDTO = new ProductItemDTO
            {
                Id = pi.Id,
                Price = pi.CurrentPrice,
                CreatedDate = pi.CreatedDate,
                Condition = pi.Condition,
                Quality = pi.Quality,
                Sold = pi.Sold,
                Weight = pi.Weight,
                CustomText = pi.CustomText,
                Product = pi.Product
            };
            List<string> imageUrls = new();
            foreach (var image in pi.Images)
            {
                imageUrls.Add(image.Url);
            }
            piDTO.Images = imageUrls.ToArray();

            return piDTO;
        }
    }
}
