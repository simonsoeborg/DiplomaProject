using ClassLibrary;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
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

        [HttpGet]
        public IActionResult Get()
        {
            var productItems = _context.ProductItems.ToList();

            if (productItems == null || productItems.Count == 0)
            {
                return new NoContentResult();
            }

            return new OkObjectResult(productItems);
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
        public HttpResponseMessage Put(int id, [FromBody] ProductItem req)
        {
            var productItem = _context.ProductItems.Find(id);

            if (productItem == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            if (!PropertiesHasValues(req))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }


            productItem.ProductId = req.ProductId;
            productItem.Condition = req.Condition;
            productItem.Quality = req.Quality;
            productItem.Sold = req.Sold;

            if (req.Weight != null) productItem.Weight = req.Weight;

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
                || string.IsNullOrWhiteSpace(productItem.Condition)
                || string.IsNullOrEmpty(productItem.Quality)
            )
            {
                return false;
            }

            return true;
        }
    }
}
