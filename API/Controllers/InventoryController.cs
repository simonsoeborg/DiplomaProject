using ClassLibrary.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly GroenlundDbContext _context;

        public InventoryController(GroenlundDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var CategoryProductCount = _context.CategoryProductCount.ToList();

            if (CategoryProductCount == null || CategoryProductCount.Count == 0)
            {
                return new NoContentResult();
            }

            return new OkObjectResult(CategoryProductCount);
        }
    }
}
