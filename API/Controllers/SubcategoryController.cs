using ClassLibrary.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers

{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoryController : ControllerBase
    {
        private readonly GroenlundDbContext _context;

        public SubcategoryController(GroenlundDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var Subcategories = _context.Subcategories.ToList();

            if (Subcategories == null || Subcategories.Count == 0)
            {
                return new NoContentResult();
            }

            return new OkObjectResult(Subcategories);
        }


        [HttpPost]
        public ActionResult<OkResult> Post([FromBody] Subcategory req)
        {

            try
            {
                var category = _context.Categories.Find(req.CategoryId);
                if (category == null)
                {
                    return new BadRequestObjectResult("Could not find category with categoryid: " + req.CategoryId);
                }
                // Removing ID property from request since database auto-increments.
                Subcategory reqSubcategory = new()
                {
                    Name = req.Name,
                    Order = req.Order,
                    ImageUrl = req.ImageUrl != null ? req.ImageUrl : null,
                    Description = req.Description != null ? req.Description : null,
                    CategoryId = req.CategoryId,
                    Category = category,
                    Products = new List<Product>()
                };

                _context.Subcategories.Add(reqSubcategory);
                _context.SaveChanges();
                return new OkResult();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new BadRequestResult();
            }

            return new NoContentResult();
        }


        [HttpPut("{id}")]
        public HttpResponseMessage Put(int id, [FromBody] Subcategory req)
        {
            Console.WriteLine("req Id: " + req.Id);
            var subCategory = _context.Subcategories.Find(id);

            if (subCategory == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            if (!PropertiesHasValues(req))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            subCategory.Name = req.Name;
            subCategory.Description = req.Description;
            subCategory.ImageUrl = req.ImageUrl;
            subCategory.Order = req.Order;
            subCategory.CategoryId = req.CategoryId;
            if (req.ImageUrl != null) { subCategory.ImageUrl = req.ImageUrl; }
            if (req.Description != null) { subCategory.Description = req.Description; }

            try
            {
                _context.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.OK);
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
            var subCategory = _context.Subcategories.Find(id);

            if (subCategory == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            _context.Subcategories.Remove(subCategory);

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


        private static bool PropertiesHasValues(Subcategory subCategory)
        {
            if (string.IsNullOrEmpty(subCategory.Name) || subCategory.CategoryId <= 0 || subCategory.Order <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
