using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MillionBackend.Core.Models;

public class Property
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdProperty { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("address")]
    public string Address { get; set; } = string.Empty;

    [BsonElement("price")]
    public decimal Price { get; set; }

    [BsonElement("codeInternal")]
    public string CodeInternal { get; set; } = string.Empty;

    [BsonElement("year")]
    public int Year { get; set; }

    [BsonElement("idOwner")]
    [BsonRepresentation(BsonType.ObjectId)]
    public string IdOwner { get; set; } = string.Empty;

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("enabled")]
    public bool Enabled { get; set; } = true;

    // Navigation properties (not stored in MongoDB)
    [BsonIgnore]
    public Owner? Owner { get; set; }

    [BsonIgnore]
    public List<PropertyImage>? Images { get; set; }

    [BsonIgnore]
    public List<PropertyTrace>? Traces { get; set; }
}
