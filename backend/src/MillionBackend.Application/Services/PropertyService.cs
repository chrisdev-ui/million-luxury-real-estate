using AutoMapper;
using MillionBackend.Core.Models;
using MillionBackend.Core.Repositories;

namespace MillionBackend.Application.Services;

public class PropertyService : IPropertyService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IMapper _mapper;

    public PropertyService(IPropertyRepository propertyRepository, IMapper mapper)
    {
        _propertyRepository = propertyRepository;
        _mapper = mapper;
    }

    public async Task<PagedList<Property>> GetPropertiesAsync(PropertyQueryParameters parameters)
    {
        return await _propertyRepository.GetPropertiesAsync(parameters);
    }

    public async Task<Property?> GetPropertyByIdAsync(string id, bool includeRelated = false)
    {
        return await _propertyRepository.GetPropertyByIdAsync(id, includeRelated);
    }

    public async Task<Property> CreatePropertyAsync(Property property)
    {
        return await _propertyRepository.CreatePropertyAsync(property);
    }

    public async Task<Property?> UpdatePropertyAsync(string id, Core.Models.UpdatePropertyDto updateDto)
    {
        var existingProperty = await _propertyRepository.GetPropertyByIdAsync(id);
        if (existingProperty == null) return null;

        _mapper.Map(updateDto, existingProperty);
        return await _propertyRepository.UpdatePropertyAsync(existingProperty);
    }

    public async Task<bool> DeletePropertyAsync(string id)
    {
        return await _propertyRepository.DeletePropertyAsync(id);
    }

    public async Task<PropertyImage> AddPropertyImageAsync(PropertyImage image)
    {
        return await _propertyRepository.AddPropertyImageAsync(image);
    }

    public async Task<PropertyTrace> AddPropertyTraceAsync(PropertyTrace trace)
    {
        return await _propertyRepository.AddPropertyTraceAsync(trace);
    }

    public async Task<List<PropertyImage>> GetPropertyImagesAsync(string propertyId)
    {
        return await _propertyRepository.GetPropertyImagesAsync(propertyId);
    }

    public async Task<List<PropertyTrace>> GetPropertyTracesAsync(string propertyId)
    {
        return await _propertyRepository.GetPropertyTracesAsync(propertyId);
    }
}
