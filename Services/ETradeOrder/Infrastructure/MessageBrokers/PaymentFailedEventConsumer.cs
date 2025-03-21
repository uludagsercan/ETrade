

using Application.Dtos.Order;
using Application.Services;
using BusShared.Events;
using Domain.Entities.Concretes.Enums;
using MassTransit;

namespace Infrastructure.MessageBrokers
{
    public class PaymentFailedEventConsumer(IOrderService orderService) : IConsumer<PaymentFailedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentFailedEvent> context)
        {
            await orderService.UpdateOrderStatusAsync(new UpdateOrderDto(context.Message.OrderId, OrderStatus.PaymentFailed));
        }
    }
}