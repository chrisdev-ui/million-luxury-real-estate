using AutoMapper;
using MillionBackend.Application.Services;
using MillionBackend.Core.Models;
using MillionBackend.Core.Repositories;
using Moq;

namespace MillionBackend.Tests.Services;

[TestFixture]
public class PropertyServiceTests
{
    private Mock<IPropertyRepository> _mockRepository;
    private Mock<IMapper> _mockMapper;
    private PropertyService _service;

    [SetUp]
    public void Setup()
    {
        _mockRepository = new Mock<IPropertyRepository>();
        _mockMapper = new Mock<IMapper>();
        _service = new PropertyService(_mockRepository.Object, _mockMapper.Object);
    }

    [Test]
    public async Task GetPropertiesAsync_ReturnsPagedList()
    {
        var parameters = new PropertyQueryParameters { PageNumber = 1, PageSize = 10 };
        var properties = new List<Property>
        {
            new Property { IdProperty = "1", Name = "Property 1", Price = 100000 },
            new Property { IdProperty = "2", Name = "Property 2", Price = 200000 }
        };
        var pagedList = new PagedList<Property>(properties, 2, 1, 10);

        _mockRepository.Setup(r => r.GetPropertiesAsync(parameters))
            .ReturnsAsync(pagedList);

        var result = await _service.GetPropertiesAsync(parameters);

        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result.TotalCount, Is.EqualTo(2));
        _mockRepository.Verify(r => r.GetPropertiesAsync(parameters), Times.Once);
    }

    [Test]
    public async Task GetPropertyByIdAsync_ValidId_ReturnsProperty()
    {
        var property = new Property { IdProperty = "1", Name = "Test Property", Price = 100000 };

        _mockRepository.Setup(r => r.GetPropertyByIdAsync("1", false))
            .ReturnsAsync(property);

        var result = await _service.GetPropertyByIdAsync("1");

        Assert.That(result, Is.Not.Null);
        Assert.That(result.IdProperty, Is.EqualTo("1"));
        Assert.That(result.Name, Is.EqualTo("Test Property"));
    }

    [Test]
    public async Task GetPropertyByIdAsync_InvalidId_ReturnsNull()
    {
        _mockRepository.Setup(r => r.GetPropertyByIdAsync("invalid", false))
            .ReturnsAsync((Property?)null);

        var result = await _service.GetPropertyByIdAsync("invalid");

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreatePropertyAsync_ReturnsCreatedProperty()
    {
        var property = new Property { Name = "New Property", Price = 100000 };
        var createdProperty = new Property { IdProperty = "1", Name = "New Property", Price = 100000 };

        _mockRepository.Setup(r => r.CreatePropertyAsync(property))
            .ReturnsAsync(createdProperty);

        var result = await _service.CreatePropertyAsync(property);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.IdProperty, Is.EqualTo("1"));
        _mockRepository.Verify(r => r.CreatePropertyAsync(property), Times.Once);
    }

    [Test]
    public async Task UpdatePropertyAsync_ValidId_ReturnsUpdatedProperty()
    {
        var existingProperty = new Property { IdProperty = "1", Name = "Old Name", Price = 100000 };
        var updateDto = new UpdatePropertyDto { Name = "New Name", Price = 150000 };

        _mockRepository.Setup(r => r.GetPropertyByIdAsync("1", false))
            .ReturnsAsync(existingProperty);
        _mockRepository.Setup(r => r.UpdatePropertyAsync(It.IsAny<Property>()))
            .ReturnsAsync(existingProperty);

        var result = await _service.UpdatePropertyAsync("1", updateDto);

        Assert.That(result, Is.Not.Null);
        _mockRepository.Verify(r => r.UpdatePropertyAsync(It.IsAny<Property>()), Times.Once);
    }

    [Test]
    public async Task UpdatePropertyAsync_InvalidId_ReturnsNull()
    {
        var updateDto = new UpdatePropertyDto { Name = "New Name" };

        _mockRepository.Setup(r => r.GetPropertyByIdAsync("invalid", false))
            .ReturnsAsync((Property?)null);

        var result = await _service.UpdatePropertyAsync("invalid", updateDto);

        Assert.That(result, Is.Null);
        _mockRepository.Verify(r => r.UpdatePropertyAsync(It.IsAny<Property>()), Times.Never);
    }

    [Test]
    public async Task DeletePropertyAsync_ValidId_ReturnsTrue()
    {
        _mockRepository.Setup(r => r.DeletePropertyAsync("1"))
            .ReturnsAsync(true);

        var result = await _service.DeletePropertyAsync("1");

        Assert.That(result, Is.True);
        _mockRepository.Verify(r => r.DeletePropertyAsync("1"), Times.Once);
    }

    [Test]
    public async Task DeletePropertyAsync_InvalidId_ReturnsFalse()
    {
        _mockRepository.Setup(r => r.DeletePropertyAsync("invalid"))
            .ReturnsAsync(false);

        var result = await _service.DeletePropertyAsync("invalid");

        Assert.That(result, Is.False);
    }

    [Test]
    public async Task GetPropertyByIdAsync_WithIncludeRelated_LoadsAllRelatedData()
    {
        var propertyId = "1";
        var property = new Property
        {
            IdProperty = propertyId,
            Name = "Test Property",
            IdOwner = "owner1"
        };
        var owner = new Owner { IdOwner = "owner1", Name = "Test Owner" };
        var images = new List<PropertyImage>
        {
            new PropertyImage { IdProperty = propertyId, File = "image1.jpg" }
        };
        var traces = new List<PropertyTrace>
        {
            new PropertyTrace { IdProperty = propertyId, Name = "Sale 1" }
        };

        _mockRepository.Setup(r => r.GetPropertyByIdAsync(propertyId, true))
            .ReturnsAsync(property);

        var result = await _service.GetPropertyByIdAsync(propertyId, includeRelated: true);

        Assert.That(result, Is.Not.Null);
        _mockRepository.Verify(r => r.GetPropertyByIdAsync(propertyId, true), Times.Once);
    }

    [Test]
    public async Task GetPropertyImagesAsync_ReturnsImageList()
    {
        var propertyId = "1";
        var images = new List<PropertyImage>
        {
            new PropertyImage { IdProperty = propertyId, File = "image1.jpg" },
            new PropertyImage { IdProperty = propertyId, File = "image2.jpg" }
        };

        _mockRepository.Setup(r => r.GetPropertyImagesAsync(propertyId))
            .ReturnsAsync(images);

        var result = await _service.GetPropertyImagesAsync(propertyId);

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].File, Is.EqualTo("image1.jpg"));
    }

    [Test]
    public async Task GetPropertyTracesAsync_ReturnsTraceList()
    {
        var propertyId = "1";
        var traces = new List<PropertyTrace>
        {
            new PropertyTrace { IdProperty = propertyId, Name = "Initial Sale" },
            new PropertyTrace { IdProperty = propertyId, Name = "Resale" }
        };

        _mockRepository.Setup(r => r.GetPropertyTracesAsync(propertyId))
            .ReturnsAsync(traces);

        var result = await _service.GetPropertyTracesAsync(propertyId);

        Assert.That(result, Has.Count.EqualTo(2));
        Assert.That(result[0].Name, Is.EqualTo("Initial Sale"));
    }
}
