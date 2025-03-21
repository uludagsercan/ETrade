using Application.Dtos.Order;
using Application.Services;
using BusShared.Events;
using Domain.Entities.Concretes.Enums;
using MassTransit;

namespace Infrastructure.MessageBrokers
{
    public class PaymentCompletedEventConsumer(IOrderService orderService) : IConsumer<PaymentCompletedEvent>
    {
        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            await orderService.UpdateOrderStatusAsync(new UpdateOrderDto(context.Message.OrderId, OrderStatus.Completed));
        }
    }

}