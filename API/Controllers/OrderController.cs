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
            var orders = await _context.Orders
                .Include(o => o.ProductItems).ThenInclude(po => po.Images)
                .Include(o => o.Payment)
                .Include(o => o.DiscountCode)
                .Include(o => o.Customer)
                .ToListAsync();

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
            if (!OrderDTOContainsValues(orderDTO))
            {
                return BadRequest();
            }
            var customer = _context.Customers.Add(orderDTO.Customer);
            await _context.SaveChangesAsync();

            // Retrieve all the product items in the order from the database
            List<ProductItem> productItems = new();
            List<ProductItem> productItemsFromDb = _context.ProductItems.Where(po => orderDTO!.ProductItemIds.Contains(po.Id)).ToList();
            foreach (var item in productItemsFromDb)
            {
                item.Sold = 1;
                item.SoldDate = DateTime.Now;
                productItems.Add(item);
                _context.Update(item);
                await _context.SaveChangesAsync();
            }
            var totalPriceAmount = (double)productItems.Sum(productItem => productItem.CurrentPrice);

            // Check if discountcode && discountcode is valid
            DiscountCode discountCode = new();
            if (orderDTO.DiscountCode != null)
            {
                var discountCodeFromDb = await _context.DiscountCodes.FindAsync(orderDTO.DiscountCode.Id);
                if (discountCodeFromDb != null && discountCodeFromDb.Code != null && discountCodeFromDb.DiscountPercentage != 0)
                {
                    // Calculate the discount in the final price
                    totalPriceAmount *= (100 - discountCodeFromDb.DiscountPercentage);
                    discountCode = discountCodeFromDb;
                }
            }

            // Create Payment
            Payment newPayment = new()
            {
                DatePaid = DateTime.Now,
                Amount = totalPriceAmount,
                Status = "Approved",
                Method = orderDTO.Payment.Method,
                TransactionId = ""

            };

            var payment = _context.Payments.Add(newPayment);
            await _context.SaveChangesAsync();

            // Create Order
            Order newOrder = new()
            {
                CustomerId = customer.Entity.Id,
                Customer = customer.Entity,
                PaymentId = payment.Entity.Id,
                Payment = payment.Entity,
                //DiscountCodeId = discountCode?.Id ?? null,
                //DiscountCode = (discountCode?.Id != 0 && string.IsNullOrEmpty(discountCode?.Code) != true && discountCode?.DiscountPercentage != null) ? discountCode : null,
                DeliveryStatus = "Afventer",
                DeliveryMethod = orderDTO.DeliveryMethod,
                OrderStatus = "Afventer levering",
                TotalPrice = totalPriceAmount,
                Active = true,
                CreatedDate = DateTime.Now,
                ProductItems = productItems
            };

            var order = _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            var orderModel = order.Entity;
            foreach (var item in orderModel.ProductItems)
            {
                item.Orders = new List<Order>();
            }
            orderModel.Customer = new Customer
            {
                Id = orderModel.Customer.Id,
                FirstName = orderModel.Customer.FirstName,
                LastName = orderModel.Customer.LastName,
                Phone = orderModel.Customer.Phone,
                Email = orderModel.Customer.Email,
                Address = orderModel.Customer.Address,
                ZipCode = orderModel.Customer.ZipCode,
                City = orderModel.Customer.City,
                Country = orderModel.Customer.Country,
                CountryCode = orderModel.Customer.CountryCode
            };

            return orderModel;
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