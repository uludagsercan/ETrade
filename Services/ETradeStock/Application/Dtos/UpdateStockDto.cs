using MongoDB.Bson;

namespace Application.Dtos
{
    public record UpdateStockDto(ObjectId Id,string ProductId, int StockCount);
    
}