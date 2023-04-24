using ClassLibrary.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers

{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly GroenlundDbContext _context;

        public CategoryController(GroenlundDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var categories = _context.Categories.ToList();

            if (categories == null || categories.Count == 0)
            {
                return new NoContentResult();
            }

            return new OkObjectResult(categories);
        }


        [HttpPost]
        public HttpResponseMessage Post([FromBody] Category value)
        {
            if (PropertiesHasValues(value))
            {
                // Removing ID property from request since database auto-increments.
                Category reqCategory = new()
                {
                    Name = value.Name,
                    Description = value.Description,
                    ImageUrl = value.ImageUrl,
                    Order = value.Order,
                };
                _context.Categories.Add(reqCategory);
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
        public HttpResponseMessage Put(int id, [FromBody] Category value)
        {
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            if (!PropertiesHasValues(value))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            category.Name = value.Name;
            category.Description = value.Description;
            category.ImageUrl = value.ImageUrl;
            category.Order = value.Order;

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
            var category = _context.Categories.Find(id);

            if (category == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            _context.Categories.Remove(category);

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


        private static bool PropertiesHasValues(Category category)
        {
            if (string.IsNullOrEmpty(category.Name)
                || string.IsNullOrEmpty(category.Description)
                || string.IsNullOrWhiteSpace(category.ImageUrl)
                || category.Order <= 0
            )
            {
                return false;
            }

            return true;
        }
    }
}
