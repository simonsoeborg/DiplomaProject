using ClassLibrary;
using ClassLibrary.WebScraping;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using WebScraper;

namespace API.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class SniperController : Controller
    {
        private readonly SniperContext _context;

        public SniperController()
        {
            _context = new SniperContext();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lauritz>>> RunLauritzSniper(string? arg1, string? arg2, string? arg3)
        {
            List<Lauritz> test = _context.GetLauritz(arg1, arg2, arg3);
            foreach (Lauritz item in test)
            {
                Console.WriteLine(item);
            }
            return Ok(test);
        }
    }
}
