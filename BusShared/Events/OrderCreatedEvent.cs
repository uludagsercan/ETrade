namespace BusShared.Events
{
    public record OrderCreatedEvent(
        Guid OrderId,
        string CustomerName,
        List<OrderDetail> OrderDetails);
    public record OrderDetail(Guid ProductId, int Quantity, decimal Price);
}