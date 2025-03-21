
using Application.Dtos.Order;
using Application.MessageBrokers;
using Application.Services;
using MediatR;
namespace Application.Features.Commands.CreateOrder
{
    public record CreateOrderCommandRequest(CreateOrderDto CreateOrderDto) : IRequest<CreateOrderCommandResponse>;
    public record CreateOrderCommandResponse(bool IsSuccess);
    public class CreateOrderCommandHandler(IOrderService orderService) : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
    {
        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
        {
            var isSuccess = await orderService.CreateOrderAsync(request.CreateOrderDto);
            return new CreateOrderCommandResponse(isSuccess);
        }
    }


}