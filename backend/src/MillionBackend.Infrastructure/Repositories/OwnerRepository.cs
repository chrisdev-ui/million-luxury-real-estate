using MongoDB.Driver;
using MillionBackend.Core.Models;
using MillionBackend.Core.Repositories;

namespace MillionBackend.Infrastructure.Repositories;

public class OwnerRepository : IOwnerRepository
{
    private readonly IMongoCollection<Owner> _owners;

    public OwnerRepository(IMongoDatabase database)
    {
        _owners = database.GetCollection<Owner>("owners");
    }

    public async Task<Owner?> GetOwnerByIdAsync(string id)
    {
        return await _owners.Find(x => x.IdOwner == id).FirstOrDefaultAsync();
    }

    public async Task<List<Owner>> GetOwnersAsync()
    {
        return await _owners.Find(_ => true).SortBy(x => x.Name).ToListAsync();
    }

    public async Task<Owner> CreateOwnerAsync(Owner owner)
    {
        await _owners.InsertOneAsync(owner);
        return owner;
    }

    public async Task<Owner?> UpdateOwnerAsync(Owner owner)
    {
        var result = await _owners.ReplaceOneAsync(x => x.IdOwner == owner.IdOwner, owner);
        return result.IsAcknowledged && result.ModifiedCount > 0 ? owner : null;
    }

    public async Task<bool> DeleteOwnerAsync(string id)
    {
        var result = await _owners.DeleteOneAsync(x => x.IdOwner == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }
}
