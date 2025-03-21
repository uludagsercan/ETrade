
using Application.Abstracts;
using Application.Dtos;
using Domain;
using Mapster;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Persistence.Contexts;

namespace Persistence.Services
{
    public class StockService(IConfiguration configuration) : IStockService
    {
        public async Task<bool> IsStockAvailableAsync(string productId, int quantity)
        {
            var mongoConnectionString = configuration.GetConnectionString("MongoDb");
            var mongoClient = new MongoClient(mongoConnectionString);
            var collection = mongoClient.GetDatabase("worker").GetCollection<Stock>("stocks");
            var filter = Builders<Stock>.Filter.Eq(x => x.ProductId, productId);
            var stock = (await collection.FindAsync(filter)).FirstOrDefault();
            if (stock == null)
            {
                return false;
            }
            if (stock.StockCount < quantity)
            {
                return false;
            }
            UpdateStockDto updateStockDto = new UpdateStockDto(stock.Id, stock.ProductId, (stock.StockCount - quantity));
            await collection.UpdateOneAsync(filter, Builders<Stock>.Update.Set(x => x.StockCount, updateStockDto.StockCount));
            return stock.StockCount >= quantity;
        }
    }

}