using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BusShared.Events;
using Infrastructure.MessageBrokers.Consumers;
using Application.MessageBrokers;
using Infrastructure.MessageBrokers;
namespace Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IBusService,BusService>();
            services.AddMassTransit(c =>
            {
                c.AddConsumer<StockReservedEventConsumer>();
                c.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration.GetConnectionString("RabbitMQ"));
                    cfg.ReceiveEndpoint("etrade.payment.stock-reserved-event-queue", e =>
                    {
                        e.ConfigureConsumer<StockReservedEventConsumer>(context);
                    });
                });
            });

        }
    }
}