using ClassLibrary.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace API.Controllers

{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly GroenlundDbContext _context;

        public ImageController(GroenlundDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var images = _context.Images.ToList();

            if (images == null || images.Count == 0)
            {
                return new NoContentResult();
            }

            return new OkObjectResult(images);
        }


        [EnableCors]
        [HttpPost]
        public HttpResponseMessage Post([FromBody] Image req)
        {
            if (PropertiesHasValues(req))
            {
                // Removing ID property from request since database auto-increments.
                Image img = new()
                {
                    Id = req.Id,
                    Url = req.Url,
                };
                _context.Images.Add(img);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            try
            {
                _context.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.Created);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }


        [HttpPut("{id}")]
        public HttpResponseMessage Put(int id, [FromBody] Image req)
        {
            var img = _context.Images.Find(id);

            if (img == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            if (!PropertiesHasValues(req))
            {
                return new HttpResponseMessage(HttpStatusCode.BadRequest);
            }

            img.Id = req.Id;
            img.Url = req.Url;

            try
            {
                _context.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }


        [HttpDelete("{id}")]
        public HttpResponseMessage Delete(int id)
        {
            var img = _context.Images.Find(id);

            if (img == null)
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            _context.Images.Remove(img);

            try
            {
                _context.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }


        private static bool PropertiesHasValues(Image img)
        {
            if (string.IsNullOrEmpty(img.Url)
                || img.Id <= 0
            )
            {
                return false;
            }

            return true;
        }
    }
}
