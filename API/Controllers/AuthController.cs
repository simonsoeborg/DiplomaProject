using ClassLibrary.DTOModels;
using ClassLibrary.Models;
using ClassLibrary.Models.DTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly GroenlundDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthController(GroenlundDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("UserExists")]
        public async Task<ActionResult<bool>> UserExists(string email)
        {
            User? response = await _context.Users.SingleOrDefaultAsync(e => e.Email == email);
            //Console.WriteLine(response?.Email);
            if (response == null)
            {
                return NotFound("User not found");
            }
            // TODO : Handle a way for the user to reset password
            return Ok(true);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login(UserLoginDTO request)
        {
            User? dbUser = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

            // Lookup user in DB so we can compare hash and salt
            if (dbUser == null)
            {
                return NotFound("User not found");
            }
            if (!VerifyPasswordHash(request.Password, dbUser.PasswordHash, dbUser.PasswordSalt))
            {
                return BadRequest("Wrong password");
            }

            //Console.WriteLine("User", dbUser.ToString());
            string token = CreateToken(dbUser).Result;
            return Ok(token);
        }

        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register(UserRegistrationDTO newUser)
        {
            var userExists = await _context.Users.AnyAsync(e => e.Email == newUser.Email);
            if (!userExists)
            {
                User user = new()
                {
                    RoleId = 2,
                    FirstName = newUser.FirstName,
                    LastName = newUser.LastName,
                    Email = newUser.Email,
                    CreatedAt = DateOnly.FromDateTime(DateTime.Now).ToString(),
                    Age = null
                };
                CreatePasswordHash(newUser.Password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _context.Users.Add(user);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException)
                {
                    throw;
                }

                UserRegistrationDTO userRegistrationDTO = new()
                {
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,

                };

                return Created("", userRegistrationDTO);
            }
            else
            {
                return Conflict("User already exists!");
            }
        }
        private async Task<string> CreateToken(User user)
        {
            Role? userRole = await _context.Roles.FindAsync(user.RoleId);
            if (userRole != null)
            {
                List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.FirstName+" "+user.LastName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, userRole.Title!),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    _configuration.GetSection("AppSettings:Token").Value!));

                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

                var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: credentials);

                var jwt = new JwtSecurityTokenHandler().WriteToken(token);

                return jwt;
            }
            throw new Exception("User role was not found in AuthController_CreateToken()!");
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
        private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }


        [HttpGet("GetBestSellerProducts")]
        public ActionResult<IEnumerable<Product>> GetProductItems(int amountOfBestSellers)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var soldProductItems = _context.ProductItems
                .Where(p => p.Sold == 1)
                .Include(pi => pi.Product)
                .ToList();

            List<ProductItemWithEvalution> productItemWithVal = new();

            foreach (var productItem in soldProductItems)
            {
                // New custom object for better differentiating the differen classes and new algorithm value. 
                ProductItemWithEvalution tempProductItem = new ProductItemWithEvalution();
                tempProductItem.Productitem = productItem;
                tempProductItem.Product = productItem.Product;
                decimal salesAlgorithmValue = 0;            
               
             // Algorithm for determing value of sales.
            if (productItem.SoldDate.HasValue)
             {
                TimeSpan durration = productItem.SoldDate.Value - productItem.CreatedDate;
                 int daysBetween = durration.Days;        
                 salesAlgorithmValue = (productItem.CurrentPrice - productItem.PurchasePrice)/ daysBetween;
                }
                tempProductItem.SalesValue = salesAlgorithmValue;
                productItemWithVal.Add(tempProductItem);
            }
        
            // Group the items by ProductID and average their SalesValues + adjust algorithm for count. 
            var productItemsUpdated = productItemWithVal.GroupBy(p => p.Productitem.ProductId)
                .Select(g => new ProductItemWithEvalution
                {
                    Productitem = g.First().Productitem,
                    SalesValue = g.Average(p => p.SalesValue) * (1+g.Count()/10)
                }).ToList();

            // Sort the list by SalesValue in descending order, and only return the amount of values indicated in the FE.
            productItemsUpdated = productItemsUpdated.OrderByDescending(p => p.SalesValue).Take(amountOfBestSellers).ToList();


            // Covert into product list
            List<Product> productBestSellers = new();
            foreach (var bestSellerItem in productItemsUpdated)
            {
                Product bestSeller = new Product();
                bestSeller = bestSellerItem.Productitem.Product;
                productBestSellers.Add(bestSeller);
            }


           if (soldProductItems == null || soldProductItems.Count == 0)
            {
                Console.WriteLine("null triggerd");
                return new NoContentResult();
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;

            Console.WriteLine("\nIt took {0} seconds to read and convert sold-ProductItems from database + running sales-algorithm.", (elapsedMs / 1000));
            return productBestSellers;
        }

    }
}
