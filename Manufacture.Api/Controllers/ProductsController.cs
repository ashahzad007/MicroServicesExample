using Manufacture.Api.Data;
using Manufacture.Api.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Manufacture.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ManufactureContext _context;

        public ProductsController(ManufactureContext context)
        {
            _context = context;
        } 

        [HttpGet]

        public async Task<IActionResult> GetAsync() // get all data from products
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }



        [HttpGet]  // get by Id
        [Route("{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            return Ok(product);
        }

        [HttpPost] // Create Action for posting data or adding record in the database 
        public async Task<IActionResult> PostAsync(Products newProduct )
        {
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Get", new {id=newProduct.Id},newProduct);
        }
    }
}
