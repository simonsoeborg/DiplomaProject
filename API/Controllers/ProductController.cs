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
    public class ProductController : ControllerBase
    {
        private readonly GroenlundDbContext _context;

        public ProductController(GroenlundDbContext context)
        {
            _context = context;
        }


        [HttpGet("GetAll")]
        public IActionResult GetProducts()
        {
            //var watch = System.Diagnostics.Stopwatch.StartNew();
            var products = _context.Products.Include(p => p.Subcategories).ToList();
            //watch.Stop();
            //var elapsedMs = watch.ElapsedMilliseconds;
            //Console.WriteLine("took {0} seconds to read from database", (elapsedMs / 1000));

            //var watch2 = System.Diagnostics.Stopwatch.StartNew();
            var productDTOs = new List<ProductDTO>();
            foreach (var product in products)
            {
                productDTOs.Add(DTOMapper.MapProductToDTO(product));    
            }

            if (products == null)
            {
                return new NoContentResult();
            }

            //var elapsedMs2 = watch2.ElapsedMilliseconds;
            //Console.WriteLine("took {0} seconds to convert to DTO", (elapsedMs2 / 1000));

            return Ok(productDTOs);
        }


        [HttpPost]
        public HttpResponseMessage Post([FromBody] Product req)
        {
            if (PropertiesHasValues(req))
            {
                // Removing ID property from request since database auto-increments.
                Product reqProduct = new()
                {
                    Name = req.Name,
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
                || int.Parse(product.ModelNumber) <= 0
                || product.Material <= 0
            )
            {
                return false;
            }

            return true;
        }
    }

}