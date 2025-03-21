

using Application.Abstracts;
using BusShared.Events;
using Infrastructure.MessageBrokers.Publishers;
using MassTransit;

namespace Infrastructure.MessageBrokers.Consumers
{
    public class OrderCreatedEventConsumer(IPublishEndpoint publishEndpoint) : IConsumer<OrderCreatedEvent>
    {
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var orderCreatedEvent = context.Message;
            var stockService = context.GetServiceOrCreateInstance<IStockService>();
            foreach (var item in orderCreatedEvent.OrderDetails)
            {
                var isStockAvailable = await stockService.IsStockAvailableAsync(item.ProductId.ToString(), item.Quantity);
                if (!isStockAvailable)
                {
                    await publishEndpoint.Publish(new StockNotAvailableEvent(orderCreatedEvent.OrderId, "Stock is not available"));
                }
                else
                {
                    await publishEndpoint.Publish(new StockReservedEvent(orderCreatedEvent.OrderId));
                }
            }
        }
    }
}