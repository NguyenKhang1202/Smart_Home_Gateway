using MongoDB.Bson;

namespace Gateway.Core.Entities.Base
{
    public class MongoEntity
    {
        public ObjectId Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
