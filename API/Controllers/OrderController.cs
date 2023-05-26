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
        public async Task<ActionResult<OrderDTO>> PostOrder(CreateOrderDTO orderDTO)
        {
            Console.WriteLine("OrderDTOValues:", OrderDTOContainsValues(orderDTO));
            //if (!OrderDTOContainsValues(orderDTO))
            //{
            //    return BadRequest();
            //}
            var customer = _context.Customers.Add(orderDTO.Customer);
            await _context.SaveChangesAsync();

            // Retrieve all the product items in the order from the database
            var productItemsFromDb = _context.ProductItems.Where(po => orderDTO!.ProductItemIds.Contains(po.Id)).ToList();

            // Check if discountcode && discountcode is valid
            // TODO

            var totalPriceAmount = (double)productItemsFromDb.Sum(productItem => productItem.CurrentPrice);

            // Create Payment
            Payment newPayment = new()
            {
                DatePaid = DateTime.Now,
                Amount = totalPriceAmount,
                Approved = 1,
                Method = orderDTO.Payment.Method
            };

            var payment = _context.Payments.Add(newPayment);
            await _context.SaveChangesAsync();

            // Create Order
            var orderStatus = payment.Entity.Approved != null ? "Approved" : "Awaiting payment";
            Order newOrder = new()
            {
                CreatedDate = DateTime.Now,
                Customer = customer.Entity,
                CustomerId = customer.Entity.Id,
                PaymentId = payment.Entity.Id,
                Payment = payment.Entity,
                Active = true,
                DiscountCode = "",
                DeliveryStatus = "Pending",
                OrderElements = new List<OrderElements>()
            };

            var order = _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            List<OrderElements> orderElements = new();
            foreach (var productItemInOrder in productItemsFromDb)
            {
                var orderElement = new OrderElements()
                {
                    OrderId = order.Entity.Id,
                    Order = order.Entity,
                    ProductItemId = productItemInOrder.ProductId,
                    ProductItem = productItemInOrder
                };
                _context.OrderElements.Add(orderElement);
                await _context.SaveChangesAsync();
            }


            //return CreatedAtAction("GetOrder", order);
            return DTOMapper.mapOrderToOrderDTO(order.Entity);
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

        private bool OrderDTOContainsValues(CreateOrderDTO dto)
        {
            // Check if dto itself is not null
            if (dto == null)
            {
                return false;
            }

            // Check if Customer properties are not null or empty
            if (dto.Customer == null ||
                string.IsNullOrWhiteSpace(dto.Customer.FirstName) ||
                string.IsNullOrWhiteSpace(dto.Customer.LastName) ||
                string.IsNullOrWhiteSpace(dto.Customer.Email) ||
                string.IsNullOrWhiteSpace(dto.Customer.Address) ||
                string.IsNullOrWhiteSpace(dto.Customer.ZipCode) ||
                string.IsNullOrWhiteSpace(dto.Customer.City) ||
                string.IsNullOrWhiteSpace(dto.Customer.Country) ||
                string.IsNullOrWhiteSpace(dto.Customer.CountryCode))
            {
                return false;
            }

            // Check if DiscountCode properties are not null
            if (dto.DiscountCode == null || string.IsNullOrEmpty(dto.DiscountCode.Code))
            {
                return false;
            }

            // Check if Payment properties are not null
            if (dto.Payment == null || /*dto.Payment.Amount == null ||*/ string.IsNullOrEmpty(dto.Payment.Method))
            {
                return false;
            }

            // Check if ProductItemIds is not empty
            if (!dto.ProductItemIds.Any())
            {
                return false;
            }

            // If everything above is valid, then return true
            return true;
        }

    }
}