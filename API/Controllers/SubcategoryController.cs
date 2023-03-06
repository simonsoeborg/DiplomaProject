using ClassLibrary;
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


        [EnableCors]
        [HttpPost]
        public HttpResponseMessage Post([FromBody] Subcategory req)
        {
            if (PropertiesHasValues(req))
            {
                // Removing ID property from request since database auto-increments.
                Subcategory reqSubcategory = new()
                {
                    Name = req.Name,
                    Order = req.Order,
                    CategoryId = req.CategoryId
                };
                if (req.ImageUrl != null) { reqSubcategory.ImageUrl = req.ImageUrl; }
                if (req.Description != null) { reqSubcategory.Description = req.Description; }

                _context.Subcategories.Add(reqSubcategory);
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
        public HttpResponseMessage Put(int id, [FromBody] Subcategory req)
        {
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
            if (req.ImageUrl != null) { subCategory.ImageUrl = req.ImageUrl; }
            if (req.Description != null) { subCategory.Description = req.Description; }

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
            if (string.IsNullOrEmpty(subCategory.Name)
                //|| string.IsNullOrEmpty(subCategory.Description)
                //|| string.IsNullOrWhiteSpace(subCategory.ImageUrl)
                || subCategory.Order <= 0
                || subCategory.CategoryId <= 0
            )
            {
                return false;
            }

            return true;
        }
    }
}
