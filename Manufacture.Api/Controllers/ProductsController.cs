using Manufacture.Api.Data;
using Manufacture.Api.Data.Entities;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shared.Models.RabbitMQModels;

namespace Manufacture.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ManufactureContext _context;
        private readonly IPublishEndpoint _publishEndpoint;

        public ProductsController(ManufactureContext context, IPublishEndpoint publishEndpoint) // adding IPublishEndpoint in DI 
        {
            _context = context;
            _publishEndpoint = publishEndpoint;
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
        //Install-Package MassTransit -Version 8.0.0
        // Install-Package MassTransit.AspNetCore -Version 7.3.1
        //Install-Package MassTransit.RabbitMQ -Version 8.0.0
        public async Task<IActionResult> PostAsync(Products newProduct )
        {
            _context.Products.Add(newProduct);
            await _context.SaveChangesAsync();
            // using Publish EndPoint to push the data to the Product Model Class which is ProductCreated // Two Microservice communicated with each other 
            // you can create All the propeties of the Product Class. i used only Two.

            await _publishEndpoint.Publish<ProductCreated>(new ProductCreated
            {
                Id = newProduct.Id,
                Name = newProduct.Name
            });


            return CreatedAtAction("Get", new {id=newProduct.Id},newProduct);
        }
    }
}
