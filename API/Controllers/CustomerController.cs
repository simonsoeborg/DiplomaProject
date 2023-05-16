using ClassLibrary.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Net;

namespace API.Controllers

{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly GroenlundDbContext _context;

        public CustomerController(GroenlundDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var customers = _context.Customers.ToList();

            if (customers == null || customers.Count == 0)
            {
                return new NoContentResult();
            }

            return new OkObjectResult(customers);
        }


        [HttpPost]
        public IActionResult Post([FromBody] ClassLibrary.Models.Customer customer)
        {
            var newCustomer = new ClassLibrary.Models.Customer()
            {
                FirstName = "",
                LastName = "",
                Email = "",
                Phone = 0,
                City = "",
                ZipCode = "",
                Country = "",
                CountryCode = "",
                Address = "",
            };

            // Check if the email already exists in the database
            if (_context.Customers.Any(c => c.Email == customer.Email))
            {
                // Email already exists, return OK and the existing customer object
                var existingCustomer = _context.Customers.FirstOrDefault(c => c.Email == customer.Email);
                return new OkObjectResult(customer);
            }

            // If the email doesn't exist, create a new customer
            newCustomer = new ClassLibrary.Models.Customer()
            {
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone,
                City = customer.City,
                ZipCode = customer.ZipCode,
                Country = customer.Country,
                CountryCode = customer.CountryCode,
                Address = customer.Address,
            };
            _context.Customers.Add(newCustomer);

            try
            {
                _context.SaveChanges();
                return new OkObjectResult(newCustomer);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

        private static bool PropertiesHasValues(ClassLibrary.Models.Customer customer)
        {
            if (string.IsNullOrEmpty(customer.Email))
            {
                return false;
            }

            return true;
        }
    }
}
