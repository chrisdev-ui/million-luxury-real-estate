using MongoDB.Driver;
using MillionBackend.Core.Models;

namespace MillionBackend.Infrastructure.Data;

public class MongoDbIndexInitializer
{
    private readonly IMongoDatabase _database;

    public MongoDbIndexInitializer(IMongoDatabase database)
    {
        _database = database;
    }

    public async Task InitializeIndexesAsync()
    {
        await CreatePropertyIndexesAsync();
        await CreateOwnerIndexesAsync();
    }

    private async Task CreatePropertyIndexesAsync()
    {
        var collection = _database.GetCollection<Property>("properties");
        var indexKeys = Builders<Property>.IndexKeys;

        var indexes = new[]
        {
            new CreateIndexModel<Property>(
                indexKeys.Ascending(p => p.Name),
                new CreateIndexOptions { Name = "idx_name" }
            ),
            new CreateIndexModel<Property>(
                indexKeys.Ascending(p => p.Address),
                new CreateIndexOptions { Name = "idx_address" }
            ),
            new CreateIndexModel<Property>(
                indexKeys.Ascending(p => p.Price),
                new CreateIndexOptions { Name = "idx_price" }
            ),
            new CreateIndexModel<Property>(
                indexKeys.Ascending(p => p.Enabled),
                new CreateIndexOptions { Name = "idx_enabled" }
            ),
            new CreateIndexModel<Property>(
                indexKeys.Ascending(p => p.IdOwner),
                new CreateIndexOptions { Name = "idx_idowner" }
            ),
            new CreateIndexModel<Property>(
                indexKeys.Combine(
                    indexKeys.Ascending(p => p.Name),
                    indexKeys.Ascending(p => p.Address),
                    indexKeys.Ascending(p => p.Price)
                ),
                new CreateIndexOptions { Name = "idx_name_address_price" }
            )
        };

        await collection.Indexes.CreateManyAsync(indexes);
    }

    private async Task CreateOwnerIndexesAsync()
    {
        var collection = _database.GetCollection<Owner>("owners");
        var indexKeys = Builders<Owner>.IndexKeys;

        var indexes = new[]
        {
            new CreateIndexModel<Owner>(
                indexKeys.Ascending(o => o.Name),
                new CreateIndexOptions { Name = "idx_owner_name" }
            ),
            new CreateIndexModel<Owner>(
                indexKeys.Ascending(o => o.Address),
                new CreateIndexOptions { Name = "idx_owner_address" }
            )
        };

        await collection.Indexes.CreateManyAsync(indexes);
    }
}
