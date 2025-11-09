using MillionBackend.Core.Models;
using MillionBackend.Core.Repositories;

namespace MillionBackend.Application.Services;

public class OwnerService : IOwnerService
{
    private readonly IOwnerRepository _ownerRepository;

    public OwnerService(IOwnerRepository ownerRepository)
    {
        _ownerRepository = ownerRepository;
    }

    public async Task<Owner?> GetOwnerByIdAsync(string id)
    {
        return await _ownerRepository.GetOwnerByIdAsync(id);
    }

    public async Task<List<Owner>> GetOwnersAsync()
    {
        return await _ownerRepository.GetOwnersAsync();
    }

    public async Task<Owner> CreateOwnerAsync(Owner owner)
    {
        return await _ownerRepository.CreateOwnerAsync(owner);
    }

    public async Task<Owner?> UpdateOwnerAsync(Owner owner)
    {
        return await _ownerRepository.UpdateOwnerAsync(owner);
    }

    public async Task<bool> DeleteOwnerAsync(string id)
    {
        return await _ownerRepository.DeleteOwnerAsync(id);
    }
}
