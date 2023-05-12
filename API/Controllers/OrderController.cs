using ClassLibrary.DTOModels;
using ClassLibrary.Models;
using ClassLibrary.Models.DTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly GroenlundDbContext _context;

        public OrderController(GroenlundDbContext context)
        {
            _context = context;
        }

        // GET: api/Order
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrders()
        {
            var orders = await _context.Orders.Include(oe => oe.OrderElements).ToListAsync();
            List<OrderDTO> orderDTOs = new();

            foreach (var order in orders)
            {
                orderDTOs.Add(DTOMapper.mapOrderToOrderDTO(order));
            }
            return orderDTOs;
        }

        // GET: api/Order/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Order/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(int id, Order order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Order
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(CreateOrderDTO orderDTO)
        {
            if (orderDTO == null || orderDTO.ProductItemIds.Count == 0 || orderDTO.Customer == null || orderDTO.PaymentForm == null)
            {
                return BadRequest();
            }

            // Retrieve all the product items in the order from the database
            var productItemsFromDb = _context.ProductItems.Where(po => orderDTO.ProductItemIds.Contains(po.Id)).ToList();

            // Create Payment
            Payment newPayment = new()
            {
                Method = orderDTO.PaymentForm.PaymentMethod,
                Approved = 1,
                DatePaid = DateTime.Now,
                Amount = (double)productItemsFromDb.Sum(productItem => productItem.CurrentPrice)
            };

            var payment = _context.Payments.Add(newPayment);
            await _context.SaveChangesAsync();

            // Create Order
            var orderStatus = payment.Entity.Approved != null? "Approved" : "Awaiting payment";
            Order newOrder = new()
            {
                CustomerId = orderDTO.Customer.Id,
                PaymentId = payment.Entity.Id,
                PaymentStatus = orderStatus,
                Active = true,
                DiscountCode = "",
                DeliveryStatus = "Pending",
            };

            var order = _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            //foreach(var productItemInOrder  in productItemsFromDb)
            //{
            //    var orderElement = new OrderElements()
            //    {
            //        OrderId = order.Entity.Id,
            //        Order = order.Entity,
            //        ProductItemId = productItemInOrder.ProductId,
            //        ProductItem = productItemInOrder
            //    };
            //    _context.OrderElements.Add(orderElement);
            //    await _context.SaveChangesAsync();
            //}


            //return CreatedAtAction("GetOrder", order);
            return order.Entity;
        }

        // DELETE: api/Order/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}