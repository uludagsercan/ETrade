

using Application.Repositories.OrderRepository;
using Domain.Entities.Concretes;
using Persistence.Contexts;

namespace Persistence.Repositories.OrderRepository
{
    public class OrderReadRepository : ReadRepository<Order>, IOrderReadRepository
    {
        public OrderReadRepository(OrderServiceDbContext context) : base(context)
        {
        }
    }

}