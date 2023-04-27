using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ClassLibrary.Models;
using Microsoft.AspNetCore.Cors;

namespace API.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductItemDetailsController : ControllerBase
    {
        private readonly GroenlundDbContext _context;

        public ProductItemDetailsController(GroenlundDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var productItemDetails = _context.ProductsWithWeight.ToList();

            if (productItemDetails == null)
            {
                return new NoContentResult();
            }

            return new OkObjectResult(productItemDetails);
        }
    }
}
