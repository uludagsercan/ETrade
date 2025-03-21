

using Domain.Entities.Concretes.Enums;

namespace Application.Dtos.Order
{
    public record UpdateOrderDto(Guid Id,OrderStatus Status);
}