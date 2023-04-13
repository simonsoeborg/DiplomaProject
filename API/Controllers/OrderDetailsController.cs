using ClassLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailsController : ControllerBase
    {
        private readonly GroenlundDbContext _context;

        public OrderDetailsController(GroenlundDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var orderDetails = _context.OrderDetails.ToList();

            if (orderDetails == null)
            {
                return new NoContentResult();
            }

            return new OkObjectResult(orderDetails);
        }
    }
}
