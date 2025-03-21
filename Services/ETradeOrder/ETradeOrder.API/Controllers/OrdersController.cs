
using Application.Features.Commands.CreateOrder;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ETradeOrder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> CreateOrderAsync([FromBody] CreateOrderCommandRequest request)
        {
            var response = await sender.Send(request);
            return Ok(response);
        }
    }
}