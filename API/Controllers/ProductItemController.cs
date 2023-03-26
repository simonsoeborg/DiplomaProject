using ClassLibrary.DTOModels;
using ClassLibrary.Models;
using ClassLibrary.Models.DTO;
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

        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<ProductItemDTO>> GetProductItems()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var productItems = _context.ProductItems
                .Where(p => p.Sold != 1)
                .Include(pi => pi.Product).ThenInclude(p => p.Subcategories).ThenInclude(p => p.Category)
                .Include(pi => pi.Images)
                .Include(pi => pi.PriceHistories)
                .ToList();

            List<ProductItemDTO> productItemDTOs = new List<ProductItemDTO>();
            foreach (var productItem in productItems)
            {
                productItemDTOs.Add(DTOMapper.MapProductItemToDTO(productItem));
            }

            if (productItems == null || productItems.Count == 0)
            {
                return new NoContentResult();
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("\nIt took {0} seconds to read and convert ProductItems from database", (elapsedMs / 1000));
            return productItemDTOs;
        }


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
        public IActionResult UpdateProductItem(int id, [FromBody] ProductItem req)
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
    }
}
