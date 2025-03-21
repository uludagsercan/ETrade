
using Application.Dtos.Order;

namespace Application.Services
{
    public interface IOrderService
    {
        Task<bool> CreateOrderAsync(CreateOrderDto createOrderDto);
        Task<bool> UpdateOrderStatusAsync(UpdateOrderDto updateOrderDto);
    }
}