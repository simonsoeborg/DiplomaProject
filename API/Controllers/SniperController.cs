using ClassLibrary.WebScraping;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebScraper.Controllers;

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
        public async Task<ActionResult<IEnumerable<Lauritz>>> RunLauritzSniper(string? arg)
        {
            List<Lauritz> test = _context.Context.GetLauritz(arg);
            foreach (Lauritz item in test)
            {
                Console.WriteLine(item);
            }
            return Ok(test);
        }
    }

    public class SniperContext
    {
        private SniperHandler _context;
        public SniperContext()
        {
            _context = new SniperHandler();
        }

        public SniperHandler Context { get { return _context; } }
    }
}
