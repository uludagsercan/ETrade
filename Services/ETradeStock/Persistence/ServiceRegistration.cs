
using Application.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Seeder;
using Persistence.Services;

namespace Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceService(this IServiceCollection services)
        {
            services.AddSingleton<IStockSeeder, StockSeeder>();
            services.AddSingleton<IStockService, StockService>();
        }
    }
}