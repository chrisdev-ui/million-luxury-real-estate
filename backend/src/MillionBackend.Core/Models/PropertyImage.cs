using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MillionBackend.Core.Models;

public class PropertyImage
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdPropertyImage { get; set; } = string.Empty;

    [BsonElement("idProperty")]
    public string IdProperty { get; set; } = string.Empty;

    [BsonElement("file")]
    public string File { get; set; } = string.Empty;

    [BsonElement("enabled")]
    public bool Enabled { get; set; } = true;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
