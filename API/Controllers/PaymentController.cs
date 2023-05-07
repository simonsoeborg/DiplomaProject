using ClassLibrary.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace API.Controllers
{
    [EnableCors]

    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly GroenlundDbContext _context;
        private readonly IConfiguration _configuration;

        public PaymentController(GroenlundDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
        }

        // STRIPE PAYMENTS
        //[HttpPost]
        //public async Task<IActionResult> CreateStripePayment([FromBody] StripePaymentModel stripe)
        //{
        //    StripeConfiguration.ApiKey = "sk_test_51MxptJFjBrRZR0Ef1iKvUe8bcu8zg463eKwPvSZatwZgY4ilss1FpLlN6kFUNvluJR22jCxwjEyLWvQvkGttPCLq009ghU5JDA";
        //    var paymentIntentService = new PaymentIntentService();
        //    var paymentIntentCreateOptions = new PaymentIntentCreateOptions
        //    {
        //        Amount = stripe.Amount,
        //        Currency = stripe.Currency,
        //        PaymentMethod = stripe.PaymentMethodId,
        //        Customer = stripe.CustomerId.ToString(),
        //        Confirm = true
        //    };
        //    try
        //    {
        //        var paymentIntent = await paymentIntentService.CreateAsync(paymentIntentCreateOptions);
        //        if (paymentIntent.Status == "succeeded")
        //        {
        //            // Payment success
        //            return Ok(new { PaymentStatus = "succeeded" });
        //        }
        //        else
        //        {
        //            // Payment failed
        //            return BadRequest(new { PaymentStatus = "failed", Error = "Payment failed." });
        //        }
        //    }
        //    catch (StripeException ex)
        //    {
        //        return BadRequest(new { PaymentStatus = "failed", Error = ex.Message });
        //    }
        //}


        // GET: api/Payment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            return await _context.Payments.ToListAsync();
        }

        // GET: api/Payment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return payment;
        }

        // PUT: api/Payment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, Payment payment)
        {
            if (id != payment.Id)
            {
                return BadRequest();
            }

            _context.Entry(payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
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

        // POST: api/Payment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Payment>> PostPayment(Payment payment)
        {
            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPayment", new { id = payment.Id }, payment);
        }

        // DELETE: api/Payment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);
            if (payment == null)
            {
                return NotFound();
            }

            _context.Payments.Remove(payment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.Id == id);
        }
    }
}
