using ClassLibrary.WebScraping;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using WebScraper.Controllers;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SniperController : Controller
    {
        private readonly SniperHandler _sniperHandler;
        //private readonly ILogger _logger;

        public SniperController(SniperHandler sniperHandler)
        {
            _sniperHandler = sniperHandler;
            //_logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lauritz>>> RunLauritzSniper(string? arg)
        {
            //_logger.LogInformation(_sniperHandler?.GetLauritz(arg)?[0]?.Varenummer.ToString());
            return Ok(_sniperHandler.GetLauritz(arg));
        }
    }
}
