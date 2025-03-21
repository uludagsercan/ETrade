namespace Application.Dtos.OrderDetail
{
    public record CreateOrderDetailDto(
        Guid ProductId,
        string ProductName,
        decimal Price,
        int Quantity
    );
}