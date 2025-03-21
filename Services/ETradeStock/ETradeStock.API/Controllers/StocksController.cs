using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Abstracts;
using Microsoft.AspNetCore.Mvc;

namespace ETradeStock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StocksController(IStockService stockService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> IsStockAvailable([FromQuery] string productId, int quantity)
        {
            var result = await stockService.IsStockAvailableAsync(productId, quantity);
            return Ok(result);
        }
    }
}