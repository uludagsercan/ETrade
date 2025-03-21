

using Application.Repositories.OrderRepository;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Contexts;
using Persistence.Repositories.OrderRepository;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Application.Services;
using Persistence.Services;
namespace Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceService(this IServiceCollection services, IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("PostgreSQL")!;
            services.AddDbContext<OrderServiceDbContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
            services.AddScoped<IOrderService, OrderService>();
        }
    }
}