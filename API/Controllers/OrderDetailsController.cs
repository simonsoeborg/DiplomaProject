using ClassLibrary.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers

{
    [EnableCors]
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

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetails>> GetOrder(int id)
        {
            var order = await _context.OrderDetails.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }
    }
}
