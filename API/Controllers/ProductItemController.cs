using ClassLibrary.DTOModels;
using ClassLibrary.Models;
using ClassLibrary.Models.DTO;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductItemController : ControllerBase
    {
        private readonly GroenlundDbContext _context;

        public ProductItemController(GroenlundDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public ActionResult<IEnumerable<ProductItemDTO>> GetProductItems()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            var productItems = _context.ProductItems
                .Where(p => p.Sold != 1)
                .Include(pi => pi.Product).ThenInclude(p => p.Subcategories).ThenInclude(p => p.Category)
                .Include(pi => pi.Images)
                .Include(pi => pi.PriceHistories)
                .ToList();

            List<ProductItemDTO> productItemDTOs = new();
            foreach (var productItem in productItems)
            {
                productItemDTOs.Add(DTOMapper.MapProductItemToWebDTO(productItem));
            }

            if (productItems == null || productItems.Count == 0)
            {
                return new NoContentResult();
            }

            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("\nIt took {0} seconds to read and convert ProductItems from database", (elapsedMs / 1000));
            return productItemDTOs;
        }

    }
}
