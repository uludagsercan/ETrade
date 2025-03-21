

using Application.Abstracts;
using Application.Dtos;
using Domain;
using Mapster;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Driver;
using Persistence.Contexts;

namespace Persistence.Seeder
{
    public class StockSeeder(IConfiguration configuration) : IStockSeeder
    {

        public async Task Seed()
        {
            var mongoConnectionString = configuration.GetConnectionString("MongoDb");
            var mongoClient = new MongoClient(mongoConnectionString);
            var db = StockDbContext.Create(mongoClient.GetDatabase("worker"));
            var result = db.Database.EnsureCreated();
            if (result)
            {
                var stocks = new List<CreateStockDto>
                {
                    new CreateStockDto("abbce07c-291a-4eb0-a18f-7adeccb25b61", 5, DateTime.Now),
                    new CreateStockDto("f0bd4bca-a04d-45a1-ac18-3d38aaf49f6c", 10, DateTime.Now),
                    new CreateStockDto("9eae7937-a59b-465f-a129-aa67e49ff42f", 3, DateTime.Now),
                    new CreateStockDto("b3c2a290-a1a1-4779-abf3-39e24544374b", 45, DateTime.Now),
                    new CreateStockDto("5f9ccd56-fe3d-44cd-a824-38f66242aebc", 3, DateTime.Now),
                    new CreateStockDto("643ca539-886b-4e44-9c88-8002b4609141", 6, DateTime.Now),
                    new CreateStockDto("5e3740e2-69e3-4bde-a5b0-17678bd46dec", 200, DateTime.Now),
                    new CreateStockDto("2dc3a2cb-348a-4d3e-a44b-5634859a775a", 100, DateTime.Now),
                    new CreateStockDto("0e541313-0b89-4dfb-ab09-1b6e52a07df4", 123, DateTime.Now),
                    new CreateStockDto("f38fdde3-7728-43e9-8aa9-bf46b5af19c0", 25, DateTime.Now),


                };
                await db.Stocks.AddRangeAsync(stocks.Adapt<List<Stock>>());
                await db.SaveChangesAsync();
            }

        }
    }
}