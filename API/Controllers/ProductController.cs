using ClassLibrary;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers

{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly GroenlundDbContext _context;

        public ProductController(GroenlundDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return new NotFoundObjectResult("Product with id: " + id + " not found");
            }

            return new OkObjectResult(product);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _context.Products.ToList();

            if (products == null || products.Count == 0)
            {
                return new NoContentResult();
            }

            return new OkObjectResult(products);
        }


        [EnableCors]
        [HttpPost]
        public HttpResponseMessage Post([FromBody] Product req)
        {
            if (PropertiesHasValues(req))
            {
                // Removing ID property from request since database auto-increments.
                Product reqProduct = new()
                {
                    Name = req.Name,
                    SubCatId = req.SubCatId,
                    ModelNumber = req.ModelNumber,
                    Material = req.Material,
                };
                if (req.Design != null) reqProduct.Design = req.Design;
                if (req.Dimension != null) reqProduct.Dimension = req.Dimension;
                _context.Products.Add(reqProduct);
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
        public HttpResponseMessage Put(int id, [FromBody] Product req)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            if (!PropertiesHasValues(req))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            product.Name = req.Name;
            product.SubCatId = req.SubCatId;
            product.ModelNumber = req.ModelNumber;
            product.Material = req.Material;
            if (req.Design != null) product.Design = req.Design;
            if (req.Dimension != null) product.Dimension = req.Dimension;

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
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            _context.Products.Remove(product);

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


        private static bool PropertiesHasValues(Product product)
        {
            if (string.IsNullOrEmpty(product.Name)
                || product.ModelNumber > 0
                || string.IsNullOrWhiteSpace(product.Material)
                || product.SubCatId! >= 0
            )
            {
                return false;
            }

            return true;
        }
    }
}
