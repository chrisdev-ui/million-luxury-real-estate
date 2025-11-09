using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MillionBackend.Core.Models;

public class Owner
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdOwner { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("address")]
    public string Address { get; set; } = string.Empty;

    [BsonElement("photo")]
    public string Photo { get; set; } = string.Empty;

    [BsonElement("birthday")]
    public DateTime Birthday { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
