
using Microsoft.Extensions.DependencyInjection;
using MediatR;
namespace Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationService(this IServiceCollection services)
        {
            var assembly = typeof(ServiceRegistration).Assembly;
            services.AddMediatR(c => c.RegisterServicesFromAssembly(assembly));
        }
    }
}