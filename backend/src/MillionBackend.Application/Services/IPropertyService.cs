using MillionBackend.Core.Models;

namespace MillionBackend.Application.Services;

public interface IPropertyService
{
    Task<PagedList<Property>> GetPropertiesAsync(PropertyQueryParameters parameters);
    Task<Property?> GetPropertyByIdAsync(string id, bool includeRelated = false);
    Task<Property> CreatePropertyAsync(Property property);
    Task<Property?> UpdatePropertyAsync(string id, Core.Models.UpdatePropertyDto updateDto);
    Task<bool> DeletePropertyAsync(string id);
    Task<PropertyImage> AddPropertyImageAsync(PropertyImage image);
    Task<PropertyTrace> AddPropertyTraceAsync(PropertyTrace trace);
    Task<List<PropertyImage>> GetPropertyImagesAsync(string propertyId);
    Task<List<PropertyTrace>> GetPropertyTracesAsync(string propertyId);
}
