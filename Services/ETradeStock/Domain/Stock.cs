

using MongoDB.Bson.Serialization.Attributes;

namespace Domain
{
    public class Stock : BaseEntity
    {
        public string ProductId { get;private set; }
        public int StockCount { get;private set; }
    }
}