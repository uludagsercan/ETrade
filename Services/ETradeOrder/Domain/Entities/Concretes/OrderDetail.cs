

using Domain.Entities.Abstracts;

namespace Domain.Entities.Concretes
{
    public class OrderDetail : BaseEntity
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get;private set; } = null!;
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
    }
}