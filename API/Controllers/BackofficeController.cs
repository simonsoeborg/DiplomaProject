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
    [Route("api/[controller]")/*, Authorize*/]
    [ApiController]
    public class BackofficeController : ControllerBase
    {
        private readonly GroenlundDbContext _context;

        public BackofficeController(GroenlundDbContext context)
        {
            _context = context;
        }

        //[Authorize(Roles = "Admin, SuperAdmin")]
        [HttpGet("ProductItem")]
        public ActionResult<IEnumerable<ProductItemDTO>> GetProductItems()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var productItems = _context.ProductItems
                .Include(pi => pi.Product).ThenInclude(p => p.Subcategories)/*.ThenInclude(p => p.Category)*/
                .Include(pi => pi.Images)
                .Include(pi => pi.PriceHistories)
                .ToList();

            List<ProductItemDTO> productItemDTOs = new();
            foreach (var productItem in productItems)
            {
                productItemDTOs.Add(DTOMapper.MapProductItemToBackofficeDTO(productItem));
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

        [HttpGet("OrderElements")]
        public ActionResult<IEnumerable<OrderElements>> GetOrderElements()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var orderElementsFromDb = _context.OrderElements.ToList();

            List<OrderElements> orderElements = new();
            foreach (var oe in orderElementsFromDb)
            {
                OrderElements orderElement = new()
                {
                    Id = oe.Id,
                    //OrderId = oe.OrderId,
                    ProductItemId = oe.ProductItemId,
                };
                orderElements.Add(orderElement);
            }

            if (orderElementsFromDb == null || orderElementsFromDb.Count == 0)
            {
                return new NoContentResult();
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("\nIt took {0} seconds to read and convert ProductItems from database", (elapsedMs / 1000));
            return orderElements;
        }

        [HttpPost("ProductItem")]
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


        [HttpPut("ProductItem/{id}")]
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


        [HttpDelete("ProductItem/{id}")]
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

        [HttpGet("PriceHistory")]
        public IActionResult GetPriceHistories()
        {
            var priceHistories = _context.PriceHistories.ToList();

            if (priceHistories == null || priceHistories.Count == 0)
            {
                return new NoContentResult();
            }

            return new OkObjectResult(priceHistories);
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
