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
        public async Task<ActionResult<Order>> PostOrder(CreateOrderDTO order)
        {
            // Create Payment
            Payment newPayment = new Payment();
            newPayment.Amount = (double)order.TotalPrice;
            newPayment.Method = order.PaymentForm.PaymentMethod;
            newPayment.DatePaid = DateTime.Now;

            var payment = _context.Payments.Add(newPayment);
            await _context.SaveChangesAsync();

            // Create Order
            Order newOrder = new Order();
            newOrder.CustomerId = order.Customer.Id;
            newOrder.PaymentId = payment.Entity.Id;

            ProductItem newProduct = new ProductItem();

            List<OrderElements> listOfProducts = new List<OrderElements>();

            var productItems = new List<ProductItem>();
            for (int i = 0; i < order.ProductItemIds.Count; i++)
            {
                var temp = await _context.ProductItems.FindAsync(order.ProductItemIds[i]);
                if( temp != null)
                    productItems.Add(temp);
            }

            foreach (var productItem in productItems)
            {
                newProduct.ProductId = productItem.ProductId;
                newProduct.Product = productItem.Product;
                newProduct.CurrentPrice = productItem.CurrentPrice;
                newProduct.Condition = productItem.Condition;
                newProduct.Weight = productItem.Weight;
                newProduct.CreatedDate = productItem.CreatedDate;
                newProduct.SoldDate = productItem.SoldDate;
                newProduct.CustomText = productItem.CustomText;
                newProduct.Images = productItem.Images;
                newProduct.Quality = productItem.Quality;

                listOfProducts.Add(new OrderElements{ ProductItemId = productItem.Id, ProductItem = newProduct });
            }
            if(listOfProducts.Count > 0)
            {
                newOrder.OrderElements = listOfProducts;
            } else
            {
                newOrder.OrderElements = null;
            }

            var createdOrder = _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            foreach(var element in createdOrder.Entity.OrderElements)
            {
                element.OrderId = createdOrder.Entity.Id;
            }

            await _context.SaveChangesAsync();

            // Format the Confirmation Object and return it.
            ConfirmationModel Confirmation = new ConfirmationModel();
            List<ProductItem> newProducts = new List<ProductItem>();
            foreach (var orderElement in createdOrder.Entity.OrderElements)
            {
                ProductItem temp = await _context.ProductItems
                    .Where(p => p.Id == orderElement.ProductItemId)
                    .Select(p => new ProductItem
                    {
                        Id = p.Id,
                        ProductId = p.ProductId,
                        Condition = p.Condition,
                        Quality = p.Quality,
                        Sold = p.Sold,
                        PurchasePrice = p.PurchasePrice,
                        CurrentPrice = p.CurrentPrice,
                        CreatedDate = p.CreatedDate != null ? p.CreatedDate : DateTime.Now,
                        CustomText = p.CustomText ?? "",
                        Images = p.Images,
                    }).FirstOrDefaultAsync();

                newProducts.Add(temp);
            }
            Confirmation.Order = createdOrder.Entity;
            Confirmation.Customer = order.Customer;
            Confirmation.ProductItems = newProducts;
            Confirmation.Payment = payment.Entity;


            if (createdOrder == null)
            {
                return NotFound();
            }

            return CreatedAtAction("GetOrder", new { id = order.Id }, Confirmation);
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