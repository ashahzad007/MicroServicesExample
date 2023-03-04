using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SalesBusiness.Api.Data;
using SalesBusiness.Api.Data.Entities;
using SalesBusiness.Api.DTOs;

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
            //var orders = await _salesContext.Orders.ToListAsync();
            // join orders number and which product user orders its name and Id.
            // we have already get from consumers class product data Id, Name etc
            var orders = await _salesContext.Orders
                    .Join(
                        _salesContext.Products,
                        order => order.ProductId,
                        product => product.Id,
                        (order, product) => new { Order = order, Product = product }
                    )
                    .Select(_ => new OrdersDto
                    {
                        Id = _.Order.Id,
                        OrderDate = _.Order.OrderDate,
                        UserId = _.Order.UserId,
                        ProductInfo = new ProductDto
                        {
                            Id = _.Product.Id,
                            Name = _.Product.Name
                        }
                    }).ToListAsync();

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
