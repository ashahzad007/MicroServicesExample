using MassTransit;
using SalesBusiness.Api.Data;
using SalesBusiness.Api.Data.Entities;
using Shared.Models.RabbitMQModels;

namespace SalesBusiness.Api.Consumer
{
    public class ProductCreatedConsumer : IConsumer<ProductCreated>
    {
        private readonly SalesBusinessContext _salesBusinessContext;

        public ProductCreatedConsumer(SalesBusinessContext salesBusinessContext)
        {
            _salesBusinessContext = salesBusinessContext;
        }

        // reading the rabbit MQ messgaes and saving into the database. 
        // with the help of shared model ProductCreated Class.
        // which sends by product controller in manufuture API
        public async Task Consume(ConsumeContext<ProductCreated> context)
        {
            var product = new Products // its sales business product class . data is already comes in message.id , and messgae.name, now we are copyin it into sales business product class or database table 
            {
                Id = context.Message.Id,
                Name = context.Message.Name,
            };
            _salesBusinessContext.Products.Add(product);
            await _salesBusinessContext.SaveChangesAsync();
        }
    }
}
