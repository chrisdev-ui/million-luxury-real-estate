using MillionBackend.Core.Models;

namespace MillionBackend.Application.Services;

public interface IOwnerService
{
    Task<Owner?> GetOwnerByIdAsync(string id);
    Task<List<Owner>> GetOwnersAsync();
    Task<Owner> CreateOwnerAsync(Owner owner);
    Task<Owner?> UpdateOwnerAsync(Owner owner);
    Task<bool> DeleteOwnerAsync(string id);
}
