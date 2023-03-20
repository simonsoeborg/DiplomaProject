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
        private readonly SniperHandler _sniperHandler;

        public SniperController(SniperHandler sniperHandler)
        {
            _sniperHandler = sniperHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lauritz>>> RunLauritzSniper(string? arg)
        {
            List<Lauritz> test = _sniperHandler.GetLauritz(arg);
            return Ok(test);
        }
    }
}
