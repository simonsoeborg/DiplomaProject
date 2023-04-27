using ClassLibrary.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers

{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class SalesSummaryController : ControllerBase
    {
        private readonly GroenlundDbContext _context;

        public SalesSummaryController(GroenlundDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var salesSummaries = _context.SalesSummary
                .Select(s => new SalesSummary { TotalSales = s.TotalSales, TotalAmount = (double)s.TotalAmount })
                .ToList();

            if (salesSummaries == null)
            {
                return new NoContentResult();
            }

            return new OkObjectResult(salesSummaries);
        }
    }
}
