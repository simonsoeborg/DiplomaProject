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
        private readonly ILogger _logger;

        public SniperController(SniperHandler sniperHandler, ILogger<SniperHandler> logger)
        {
            _sniperHandler = sniperHandler;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> RunSniper(string? arg)
        {
            var lauritz = _sniperHandler.GetLauritz(arg);
            var dba = _sniperHandler.getDBA(arg);

            List<ScrapingModel> data = new List<ScrapingModel>();
            foreach (ScrapingModel item in lauritz)
            {
                data.Add(item);
            }
            foreach (ScrapingModel item in dba)
            {
                data.Add(item);
            }

            return new OkObjectResult(data);
        }
    }
}
