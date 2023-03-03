using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesBusiness.Api.Data;
using SalesBusiness.Api.Data.Entities;

namespace SalesBusiness.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class OrdersController : ControllerBase
    {
        private readonly SalesBusinessContext _salesContext;

        public OrdersController(SalesBusinessContext salesContext)
        {
            _salesContext = salesContext;
        }

        [HttpGet]

        public async Task<IActionResult> GetAsync() // get all data from Orders Table
        {
            var orders = await _salesContext.Orders.ToListAsync();
            return Ok(orders);
        }



        [HttpGet]
        [Route("{id}")] // get by Id Method
        public async Task<IActionResult> GetAsync(int id)
        {
            var order = await _salesContext.Orders.FindAsync(id);
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync(Orders newOrder)
        {
            _salesContext.Add(newOrder);
            await _salesContext.SaveChangesAsync();
            return CreatedAtAction("Get", new { id = newOrder.Id }, newOrder); // reverse back to Get method to create Url with Id parameter. sample http://localhost:5288/Orders/1
        }
    }
}
