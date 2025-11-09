using MongoDB.Driver;
using MillionBackend.Core.Models;
using MillionBackend.Core.Repositories;

namespace MillionBackend.Infrastructure.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly IMongoCollection<Property> _properties;
    private readonly IMongoCollection<Owner> _owners;
    private readonly IMongoCollection<PropertyImage> _propertyImages;
    private readonly IMongoCollection<PropertyTrace> _propertyTraces;

    public PropertyRepository(IMongoDatabase database)
    {
        _properties = database.GetCollection<Property>("properties");
        _owners = database.GetCollection<Owner>("owners");
        _propertyImages = database.GetCollection<PropertyImage>("propertyImages");
        _propertyTraces = database.GetCollection<PropertyTrace>("propertyTraces");
    }

    public async Task<PagedList<Property>> GetPropertiesAsync(PropertyQueryParameters parameters)
    {
        var filterBuilder = Builders<Property>.Filter;
        var filters = new List<FilterDefinition<Property>>();

        // Text search filters
        if (!string.IsNullOrEmpty(parameters.Name))
        {
            filters.Add(filterBuilder.Regex(x => x.Name, new MongoDB.Bson.BsonRegularExpression(parameters.Name, "i")));
        }

        if (!string.IsNullOrEmpty(parameters.Address))
        {
            filters.Add(filterBuilder.Regex(x => x.Address, new MongoDB.Bson.BsonRegularExpression(parameters.Address, "i")));
        }

        // Price range filter
        if (parameters.MinPrice.HasValue || parameters.MaxPrice.HasValue)
        {
            var priceFilter = filterBuilder.Empty;

            if (parameters.MinPrice.HasValue)
                priceFilter &= filterBuilder.Gte(x => x.Price, parameters.MinPrice.Value);

            if (parameters.MaxPrice.HasValue)
                priceFilter &= filterBuilder.Lte(x => x.Price, parameters.MaxPrice.Value);

            filters.Add(priceFilter);
        }

        // Enabled filter
        if (parameters.Enabled.HasValue)
        {
            filters.Add(filterBuilder.Eq(x => x.Enabled, parameters.Enabled.Value));
        }

        var finalFilter = filters.Any() ? filterBuilder.And(filters) : filterBuilder.Empty;

        var totalCount = await _properties.CountDocumentsAsync(finalFilter);

        var properties = await _properties
            .Find(finalFilter)
            .SortByDescending(x => x.CreatedAt)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Limit(parameters.PageSize)
            .ToListAsync();

        return new PagedList<Property>(properties, (int)totalCount, parameters.PageNumber, parameters.PageSize);
    }

    public async Task<Property?> GetPropertyByIdAsync(string id, bool includeRelated = false)
    {
        var property = await _properties.Find(x => x.IdProperty == id).FirstOrDefaultAsync();

        if (property != null && includeRelated)
        {
            property.Owner = await _owners.Find(o => o.IdOwner == property.IdOwner).FirstOrDefaultAsync();
            property.Images = await GetPropertyImagesAsync(id);
            property.Traces = await GetPropertyTracesAsync(id);
        }

        return property;
    }

    public async Task<Property> CreatePropertyAsync(Property property)
    {
        await _properties.InsertOneAsync(property);
        return property;
    }

    public async Task<Property?> UpdatePropertyAsync(Property property)
    {
        property.UpdatedAt = DateTime.UtcNow;
        var result = await _properties.ReplaceOneAsync(x => x.IdProperty == property.IdProperty, property);
        return result.IsAcknowledged && result.ModifiedCount > 0 ? property : null;
    }

    public async Task<bool> DeletePropertyAsync(string id)
    {
        var result = await _properties.DeleteOneAsync(x => x.IdProperty == id);
        return result.IsAcknowledged && result.DeletedCount > 0;
    }

    public async Task<PropertyImage> AddPropertyImageAsync(PropertyImage image)
    {
        await _propertyImages.InsertOneAsync(image);
        return image;
    }

    public async Task<PropertyTrace> AddPropertyTraceAsync(PropertyTrace trace)
    {
        await _propertyTraces.InsertOneAsync(trace);
        return trace;
    }

    public async Task<List<PropertyImage>> GetPropertyImagesAsync(string propertyId)
    {
        return await _propertyImages
            .Find(img => img.IdProperty == propertyId && img.Enabled)
            .ToListAsync();
    }

    public async Task<List<PropertyTrace>> GetPropertyTracesAsync(string propertyId)
    {
        return await _propertyTraces
            .Find(trace => trace.IdProperty == propertyId)
            .ToListAsync();
    }
}
