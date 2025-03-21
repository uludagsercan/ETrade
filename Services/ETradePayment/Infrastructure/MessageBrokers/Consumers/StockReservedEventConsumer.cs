
using Application.MessageBrokers;
using BusShared.Events;
using MassTransit;

namespace Infrastructure.MessageBrokers.Consumers
{
    public class StockReservedEventConsumer(IBusService busService) : IConsumer<StockReservedEvent>
    {
        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            var stockReservedEvent = context.Message;
            //payment service will be called here
            if (true)
            {
                PaymentCompletedEvent paymentCompletedEvent = new PaymentCompletedEvent(stockReservedEvent.OrderId);
                await busService.PublishAsync(paymentCompletedEvent);
            }
            else
            {
                PaymentFailedEvent paymentFailedEvent = new PaymentFailedEvent(stockReservedEvent.OrderId, "Payment failed");
                await busService.PublishAsync(paymentFailedEvent);
            }


        }
    }
}