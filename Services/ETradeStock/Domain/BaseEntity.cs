
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain
{
    public class BaseEntity
    {
        [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
        [BsonId]
        public ObjectId Id { get; private set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public virtual DateTime CreatedAt { get; set; }
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public virtual DateTime UpdatedAt { get; set; }
    }
}