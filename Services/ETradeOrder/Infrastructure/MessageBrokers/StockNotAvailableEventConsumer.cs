

using Application.Dtos.Order;
using Application.Services;
using BusShared.Events;
using MassTransit;
using Domain.Entities.Concretes.Enums;
namespace Infrastructure.MessageBrokers
{
    public class StockNotAvailableEventConsumer(IOrderService orderService) : IConsumer<StockNotAvailableEvent>
    {
        public async Task Consume(ConsumeContext<StockNotAvailableEvent> context)
        {
            var stockNotAvailableEvent = context.Message;
            await orderService.UpdateOrderStatusAsync(new UpdateOrderDto(stockNotAvailableEvent.OrderId, OrderStatus.Cancelled));
        }
    }
}