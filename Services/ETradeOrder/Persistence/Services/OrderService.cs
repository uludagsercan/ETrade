using Application.Dtos.Order;
using Application.MessageBrokers;
using Application.Repositories.OrderRepository;
using Application.Services;
using Domain.Entities.Concretes;
using Mapster;
using BusShared.Events;
using Microsoft.EntityFrameworkCore;
namespace Persistence.Services
{
    public class OrderService(IOrderWriteRepository orderWriteRepository, IBusService busService, IOrderReadRepository orderReadRepository) : IOrderService
    {
        public async Task<bool> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            var order = createOrderDto.Adapt<Order>();

            await orderWriteRepository.AddAsync(order);
            var result = await orderWriteRepository.SaveAsync();
            if (result < 0)
                return false;
            OrderCreatedEvent orderCreatedEvent = new(order.Id, order.CustomerName, order.OrderDetails.Adapt<List<BusShared.Events.OrderDetail>>());
            await busService.PublishAsync(orderCreatedEvent);
            return true;
        }

        public async Task<bool> UpdateOrderStatusAsync(UpdateOrderDto updateOrderDto)
        {
            var order = await orderReadRepository.GetWhere(x => x.Id == updateOrderDto.Id).FirstOrDefaultAsync();
            if (order == null)
                return false;
            var updateOrder = updateOrderDto.Adapt(order);
            orderWriteRepository.Update(updateOrder);
            var result = await orderWriteRepository.SaveAsync();
            return result > 0;
        }
    }
}