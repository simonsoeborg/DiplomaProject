using ClassLibrary.Models;
using ClassLibrary.Models.DTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        [HttpGet("{id}")]
        public ActionResult<Subcategory> Get(int id)
        {
            var subcategory = _context.Subcategories.Find(id);
            if (subcategory == null)
            {
                return NotFound();
            }

            return Ok(subcategory);
        }



        [HttpPost]
        public ActionResult Post([FromBody] SubcategoryDTO req)
        {
            try
            {
                var category = _context.Categories.Find(req.CategoryId);
                if (category == null)
                {
                    return BadRequest("Could not find category with categoryid: " + req.CategoryId);
                }

                // Removing ID property from request since database auto-increments.
                Subcategory reqSubcategory = new()
                {
                    Name = req.Name,
                    Order = req.Order,
                    ImageUrl = req.ImageUrl ?? null,
                    Description = req.Description ?? null,
                    CategoryId = req.CategoryId,
                    Category = category,
                    Products = new List<Product>()
                };

                var result = _context.Subcategories.Add(reqSubcategory);
                _context.SaveChanges();
                result.Entity.Products = new List<Product>();

                // Return 201 Created with the URI of the newly created resource
                return CreatedAtAction(nameof(Get), new { id = result.Entity.Id }, result.Entity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }
        }



        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] SubcategoryDTO req)
        {
            var subCategory = _context.Subcategories.Include(sc => sc.Products).FirstOrDefault(sc => sc.Id == id);

            if (subCategory == null)
            {
                return NotFound(); // HTTP 404 Not Found
            }

            if (!PropertiesHasValues(req))
            {
                return BadRequest(); // HTTP 400 Bad Request
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
                subCategory.Products = new List<Product>(); ;
                return Ok(subCategory); // HTTP 200 OK with the updated object
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500); // HTTP 500 Internal Server Error
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


        private static bool PropertiesHasValues(SubcategoryDTO subCategory)
        {
            if (string.IsNullOrEmpty(subCategory.Name) || subCategory.CategoryId <= 0 || subCategory.Order <= 0)
            {
                return false;
            }

            return true;
        }
    }
}
