
using Application.MessageBrokers;
using Infrastructure.MessageBrokers;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumer<StockNotAvailableEventConsumer>();
                x.AddConsumer<PaymentCompletedEventConsumer>();
                x.AddConsumer<PaymentFailedEventConsumer>();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration.GetConnectionString("RabbitMQ"));
                    cfg.ReceiveEndpoint("etrade.order.stock-not-available-event-queue", e =>
                    {
                        e.ConfigureConsumer<StockNotAvailableEventConsumer>(context);

                    });
                    cfg.ReceiveEndpoint("etrade.order.payment-completed-event-queue", e =>
                    {
                        e.ConfigureConsumer<PaymentCompletedEventConsumer>(context);
                    });
                    cfg.ReceiveEndpoint("etrade.order.payment-failed-event-queue", e =>
                    {
                        e.ConfigureConsumer<PaymentFailedEventConsumer>(context);
                    });
                });
            });
            services.AddScoped<IBusService, BusService>();
        }
    }
}