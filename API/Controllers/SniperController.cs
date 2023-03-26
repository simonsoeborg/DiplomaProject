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
        private readonly ILogger _logger;

        public SniperController(SniperHandler sniperHandler, ILogger<SniperHandler> logger)
        {
            _sniperHandler = sniperHandler;
            _logger = logger;
        }

        [HttpGet]
        public JsonResult RunLauritzSniper(string? arg)
        {
            //_logger.LogInformation(_sniperHandler?.GetLauritz(arg)?[0]?.Varenummer.ToString());
            var data = _sniperHandler.GetLauritz(arg);
            _logger.LogInformation(data.ToString());
            return Json(data);
        }
    }
}
