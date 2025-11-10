using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MillionBackend.API.Controllers;
using MillionBackend.API.DTOs;
using MillionBackend.Application.Services;
using MillionBackend.Core.Models;
using Moq;

namespace MillionBackend.Tests.Controllers;

[TestFixture]
public class PropertiesControllerTests
{
    private Mock<IPropertyService> _mockService;
    private Mock<IMapper> _mockMapper;
    private PropertiesController _controller;

    [SetUp]
    public void Setup()
    {
        _mockService = new Mock<IPropertyService>();
        _mockMapper = new Mock<IMapper>();
        _controller = new PropertiesController(_mockService.Object, _mockMapper.Object);
    }

    [Test]
    public async Task GetProperties_ReturnsOkResult()
    {
        var parameters = new Core.Models.PropertyQueryParameters { PageNumber = 1, PageSize = 10 };
        var properties = new PagedList<Property>(new List<Property>(), 0, 1, 10);
        var propertyDtos = new List<PropertyDto>();

        _mockService.Setup(s => s.GetPropertiesAsync(It.IsAny<Core.Models.PropertyQueryParameters>()))
            .ReturnsAsync(properties);
        _mockMapper.Setup(m => m.Map<List<PropertyDto>>(It.IsAny<PagedList<Property>>()))
            .Returns(propertyDtos);

        var result = await _controller.GetProperties(new API.DTOs.PropertyQueryParameters());

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task GetProperty_ValidId_ReturnsOkResult()
    {
        var property = new Property { IdProperty = "1", Name = "Test" };
        var propertyDto = new PropertyDetailDto { IdProperty = "1" };

        _mockService.Setup(s => s.GetPropertyByIdAsync("1", true))
            .ReturnsAsync(property);
        _mockMapper.Setup(m => m.Map<PropertyDetailDto>(property))
            .Returns(propertyDto);

        var result = await _controller.GetProperty("1");

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task GetProperty_InvalidId_ReturnsNotFound()
    {
        _mockService.Setup(s => s.GetPropertyByIdAsync("invalid", true))
            .ReturnsAsync((Property?)null);

        var result = await _controller.GetProperty("invalid");

        Assert.That(result.Result, Is.TypeOf<NotFoundObjectResult>());
    }

    [Test]
    public async Task CreateProperty_ValidData_ReturnsCreatedResult()
    {
        var createDto = new CreatePropertyDto { Name = "New Property", Price = 100000 };
        var property = new Property { Name = "New Property", Price = 100000 };
        var createdProperty = new Property { IdProperty = "1", Name = "New Property", Price = 100000 };
        var propertyDto = new PropertyDto { IdProperty = "1" };

        _mockMapper.Setup(m => m.Map<Property>(createDto)).Returns(property);
        _mockService.Setup(s => s.CreatePropertyAsync(property)).ReturnsAsync(createdProperty);
        _mockMapper.Setup(m => m.Map<PropertyDto>(createdProperty)).Returns(propertyDto);

        var result = await _controller.CreateProperty(createDto);

        Assert.That(result.Result, Is.TypeOf<CreatedAtActionResult>());
    }

    [Test]
    public async Task GetFilteredProperties_ReturnsOkResult()
    {
        var properties = new PagedList<Property>(new List<Property>(), 0, 1, 10);
        var propertyDtos = new List<PropertyDto>();

        _mockService.Setup(s => s.GetPropertiesAsync(It.IsAny<Core.Models.PropertyQueryParameters>()))
            .ReturnsAsync(properties);
        _mockMapper.Setup(m => m.Map<List<PropertyDto>>(It.IsAny<PagedList<Property>>()))
            .Returns(propertyDtos);

        var result = await _controller.GetFilteredProperties("test", "address", 100, 200, 1, 10);

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
    }

    [Test]
    public async Task GetFilteredProperties_WithPagination_ReturnsPagedResult()
    {
        var testProperties = new List<Property>
        {
            new Property { IdProperty = "1", Name = "Property 1", Price = 150000 },
            new Property { IdProperty = "2", Name = "Property 2", Price = 180000 }
        };
        var properties = new PagedList<Property>(testProperties, 25, 1, 10);
        var propertyDtos = new List<PropertyDto>
        {
            new PropertyDto { IdProperty = "1", Name = "Property 1" },
            new PropertyDto { IdProperty = "2", Name = "Property 2" }
        };

        _mockService.Setup(s => s.GetPropertiesAsync(It.IsAny<Core.Models.PropertyQueryParameters>()))
            .ReturnsAsync(properties);
        _mockMapper.Setup(m => m.Map<List<PropertyDto>>(It.IsAny<PagedList<Property>>()))
            .Returns(propertyDtos);

        var result = await _controller.GetFilteredProperties("luxury", "miami", 100000, 200000, 1, 10);

        Assert.That(result.Result, Is.TypeOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        var response = okResult?.Value as ApiResponse<PagedListDto<PropertyDto>>;

        Assert.That(response, Is.Not.Null);
        Assert.That(response!.Success, Is.True);
        Assert.That(response.Data, Is.Not.Null);
        Assert.That(response.Data!.TotalCount, Is.EqualTo(25));
        Assert.That(response.Data.CurrentPage, Is.EqualTo(1));
        Assert.That(response.Data.PageSize, Is.EqualTo(10));
        Assert.That(response.Data.TotalPages, Is.EqualTo(3));
    }
}
