using MillionBackend.Core.Models;

namespace MillionBackend.Core.Repositories;

public interface IPropertyRepository
{
    Task<PagedList<Property>> GetPropertiesAsync(PropertyQueryParameters parameters);
    Task<Property?> GetPropertyByIdAsync(string id, bool includeRelated = false);
    Task<Property> CreatePropertyAsync(Property property);
    Task<Property?> UpdatePropertyAsync(Property property);
    Task<bool> DeletePropertyAsync(string id);
    Task<PropertyImage> AddPropertyImageAsync(PropertyImage image);
    Task<PropertyTrace> AddPropertyTraceAsync(PropertyTrace trace);
    Task<List<PropertyImage>> GetPropertyImagesAsync(string propertyId);
    Task<List<PropertyTrace>> GetPropertyTracesAsync(string propertyId);
}
