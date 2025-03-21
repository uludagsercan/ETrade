

using Application.Dtos.OrderDetail;

namespace Application.Dtos.Order
{
    public record CreateOrderDto(
        string CustomerName,
        List<CreateOrderDetailDto> OrderDetails,
        AddressDto Address);
}