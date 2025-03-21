

using Application.Repositories.OrderRepository;
using Domain.Entities.Concretes;
using Persistence.Contexts;

namespace Persistence.Repositories.OrderRepository
{
    public class OrderWriteRepository : WriteRepository<Order>, IOrderWriteRepository
    {
        public OrderWriteRepository(OrderServiceDbContext context) : base(context)
        {
        }
    }

}