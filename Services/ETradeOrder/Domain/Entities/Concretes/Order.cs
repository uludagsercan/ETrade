
using Domain.Entities.Abstracts;
using Domain.Entities.Concretes.Enums;

namespace Domain.Entities.Concretes
{
    public class Order : BaseEntity
    {
        public string CustomerName { get;private set; } =null!;
        public decimal TotalPrice => this.OrderDetails.Sum(x => x.Price * x.Quantity);
        public OrderStatus Status { get;private set; } = OrderStatus.Suspend;
        public Address Address { get;private set; } = default!;
        public IReadOnlyCollection<OrderDetail> OrderDetails { get; set; }

        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }
    }
}