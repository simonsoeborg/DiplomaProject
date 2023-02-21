using GroenlundEntityFramework.Models;
using GroenlundModels.DTOModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Reflection;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GroenlundAPI.Controllers


{
    [EnableCors]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {


        private readonly GroenlundDbContext _context;

        public CategoryController(GroenlundDbContext context)
        {
            _context = context;
        }
        //// GET: api/Categories
        //public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        //{
        //    return await _context.Categories.ToListAsync();
        //}

        // POST api/<CategoryController>
        [EnableCors]
        [HttpPost]
        public void Post([FromBody] Category value)
        {
            this._context.Categories.Add(value);
            try
            {
                this._context.SaveChanges();
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
