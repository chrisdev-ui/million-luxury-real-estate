using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MillionBackend.API.DTOs;
using MillionBackend.Application.Services;
using MillionBackend.Core.Models;

namespace MillionBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class PropertiesController : ApiControllerBase
{
    private readonly IPropertyService _propertyService;
    private readonly IMapper _mapper;

    public PropertiesController(IPropertyService propertyService, IMapper mapper)
    {
        _propertyService = propertyService;
        _mapper = mapper;
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedListDto<PropertyDto>>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<ActionResult<ApiResponse<PagedListDto<PropertyDto>>>> GetProperties(
        [FromQuery] DTOs.PropertyQueryParameters queryParameters)
    {
        try
        {
            // Convert DTO parameters to Core model parameters
            var coreParameters = new MillionBackend.Core.Models.PropertyQueryParameters
            {
                Name = queryParameters.Name,
                Address = queryParameters.Address,
                MinPrice = queryParameters.MinPrice,
                MaxPrice = queryParameters.MaxPrice,
                Year = queryParameters.Year,
                CodeInternal = queryParameters.CodeInternal,
                IdOwner = queryParameters.IdOwner,
                Enabled = queryParameters.Enabled,
                PageNumber = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize,
                IncludeOwner = queryParameters.IncludeOwner,
                IncludeImages = true,
                IncludeTraces = queryParameters.IncludeTraces,
                SortBy = queryParameters.SortBy,
                SortDescending = queryParameters.SortDescending
            };

            var properties = await _propertyService.GetPropertiesAsync(coreParameters);

            if (coreParameters.IncludeImages)
            {
                foreach (var property in properties)
                {
                    property.Images = await _propertyService.GetPropertyImagesAsync(property.IdProperty);
                }
            }

            var propertyDtos = _mapper.Map<List<PropertyDto>>(properties);

            var pagedResult = new PagedListDto<PropertyDto>
            {
                Items = propertyDtos,
                CurrentPage = properties.CurrentPage,
                TotalPages = properties.TotalPages,
                PageSize = properties.PageSize,
                TotalCount = properties.TotalCount
            };

            return Ok(ApiResponse<PagedListDto<PropertyDto>>.SuccessResult(pagedResult, "Properties retrieved successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<PagedListDto<PropertyDto>>.ErrorResult(
                "An error occurred while fetching properties",
                new List<string> { ex.Message }
            ));
        }
    }

    [HttpGet("filter")]
    [ProducesResponseType(typeof(ApiResponse<PagedListDto<PropertyDto>>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<ActionResult<ApiResponse<PagedListDto<PropertyDto>>>> GetFilteredProperties(
        [FromQuery] string? name,
        [FromQuery] string? address,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            var parameters = new MillionBackend.Core.Models.PropertyQueryParameters
            {
                Name = name,
                Address = address,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                Enabled = true,
                PageNumber = pageNumber,
                PageSize = pageSize,
                IncludeImages = true
            };

            var properties = await _propertyService.GetPropertiesAsync(parameters);

            if (parameters.IncludeImages)
            {
                foreach (var property in properties)
                {
                    property.Images = await _propertyService.GetPropertyImagesAsync(property.IdProperty);
                }
            }

            var propertyDtos = _mapper.Map<List<PropertyDto>>(properties);

            var pagedResult = new PagedListDto<PropertyDto>
            {
                Items = propertyDtos,
                CurrentPage = properties.CurrentPage,
                TotalPages = properties.TotalPages,
                PageSize = properties.PageSize,
                TotalCount = properties.TotalCount
            };

            return Ok(ApiResponse<PagedListDto<PropertyDto>>.SuccessResult(pagedResult, "Properties retrieved successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<PagedListDto<PropertyDto>>.ErrorResult(
                "An error occurred while fetching properties",
                new List<string> { ex.Message }
            ));
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<PropertyDetailDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<ActionResult<ApiResponse<PropertyDetailDto>>> GetProperty(string id)
    {
        try
        {
            var property = await _propertyService.GetPropertyByIdAsync(id, includeRelated: true);
            if (property == null)
            {
                return NotFound(ApiResponse<PropertyDetailDto>.ErrorResult("Property not found"));
            }

            var propertyDto = _mapper.Map<PropertyDetailDto>(property);
            return Ok(ApiResponse<PropertyDetailDto>.SuccessResult(propertyDto, "Property retrieved successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<PropertyDetailDto>.ErrorResult(
                "An error occurred while fetching the property",
                new List<string> { ex.Message }
            ));
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<PropertyDto>), 201)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<ActionResult<ApiResponse<PropertyDto>>> CreateProperty([FromBody] CreatePropertyDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequestFromModelState<PropertyDto>();
            }

            var property = _mapper.Map<Property>(createDto);
            var createdProperty = await _propertyService.CreatePropertyAsync(property);
            var propertyDto = _mapper.Map<PropertyDto>(createdProperty);

            return CreatedAtAction(
                nameof(GetProperty),
                new { id = propertyDto.IdProperty },
                ApiResponse<PropertyDto>.SuccessResult(propertyDto, "Property created successfully")
            );
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<PropertyDto>.ErrorResult(
                "An error occurred while creating the property",
                new List<string> { ex.Message }
            ));
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<PropertyDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<ActionResult<ApiResponse<PropertyDto>>> UpdateProperty(string id, [FromBody] DTOs.UpdatePropertyDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequestFromModelState<PropertyDto>();
            }

            // Map API DTO to Core DTO
            var coreUpdateDto = _mapper.Map<Core.Models.UpdatePropertyDto>(updateDto);
            var updatedProperty = await _propertyService.UpdatePropertyAsync(id, coreUpdateDto);
            if (updatedProperty == null)
            {
                return NotFound(ApiResponse<PropertyDto>.ErrorResult("Property not found"));
            }

            var propertyDto = _mapper.Map<PropertyDto>(updatedProperty);
            return Ok(ApiResponse<PropertyDto>.SuccessResult(propertyDto, "Property updated successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<PropertyDto>.ErrorResult(
                "An error occurred while updating the property",
                new List<string> { ex.Message }
            ));
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<ActionResult<ApiResponse>> DeleteProperty(string id)
    {
        try
        {
            var result = await _propertyService.DeletePropertyAsync(id);
            if (!result)
            {
                return NotFound(ApiResponse.ErrorResult("Property not found"));
            }

            return Ok(ApiResponse.SuccessResult("Property deleted successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse.ErrorResult(
                "An error occurred while deleting the property",
                new List<string> { ex.Message }
            ));
        }
    }

    [HttpPost("{id}/images")]
    [ProducesResponseType(typeof(ApiResponse<PropertyImageDto>), 201)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<ActionResult<ApiResponse<PropertyImageDto>>> AddPropertyImage(string id, [FromBody] CreatePropertyImageDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequestFromModelState<PropertyImageDto>();
            }

            // Verify property exists
            var property = await _propertyService.GetPropertyByIdAsync(id);
            if (property == null)
            {
                return NotFound(ApiResponse<PropertyImageDto>.ErrorResult("Property not found"));
            }

            var image = _mapper.Map<PropertyImage>(createDto);
            image.IdProperty = id;

            var createdImage = await _propertyService.AddPropertyImageAsync(image);
            var imageDto = _mapper.Map<PropertyImageDto>(createdImage);

            return CreatedAtAction(
                nameof(GetPropertyImages),
                new { id },
                ApiResponse<PropertyImageDto>.SuccessResult(imageDto, "Property image added successfully")
            );
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<PropertyImageDto>.ErrorResult(
                "An error occurred while adding the property image",
                new List<string> { ex.Message }
            ));
        }
    }

    [HttpPost("{id}/traces")]
    [ProducesResponseType(typeof(ApiResponse<PropertyTraceDto>), 201)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<ActionResult<ApiResponse<PropertyTraceDto>>> AddPropertyTrace(string id, [FromBody] CreatePropertyTraceDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequestFromModelState<PropertyTraceDto>();
            }

            // Verify property exists
            var property = await _propertyService.GetPropertyByIdAsync(id);
            if (property == null)
            {
                return NotFound(ApiResponse<PropertyTraceDto>.ErrorResult("Property not found"));
            }

            var trace = _mapper.Map<PropertyTrace>(createDto);
            trace.IdProperty = id;

            var createdTrace = await _propertyService.AddPropertyTraceAsync(trace);
            var traceDto = _mapper.Map<PropertyTraceDto>(createdTrace);

            return CreatedAtAction(
                nameof(GetPropertyTraces),
                new { id },
                ApiResponse<PropertyTraceDto>.SuccessResult(traceDto, "Property trace added successfully")
            );
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<PropertyTraceDto>.ErrorResult(
                "An error occurred while adding the property trace",
                new List<string> { ex.Message }
            ));
        }
    }

    [HttpGet("{id}/images")]
    [ProducesResponseType(typeof(ApiResponse<List<PropertyImageDto>>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<ActionResult<ApiResponse<List<PropertyImageDto>>>> GetPropertyImages(string id)
    {
        try
        {
            // Verify property exists
            var property = await _propertyService.GetPropertyByIdAsync(id);
            if (property == null)
            {
                return NotFound(ApiResponse<List<PropertyImageDto>>.ErrorResult("Property not found"));
            }

            var images = await _propertyService.GetPropertyImagesAsync(id);
            var imageDtos = _mapper.Map<List<PropertyImageDto>>(images);

            return Ok(ApiResponse<List<PropertyImageDto>>.SuccessResult(imageDtos, "Property images retrieved successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<List<PropertyImageDto>>.ErrorResult(
                "An error occurred while fetching property images",
                new List<string> { ex.Message }
            ));
        }
    }

    [HttpGet("{id}/traces")]
    [ProducesResponseType(typeof(ApiResponse<List<PropertyTraceDto>>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<ActionResult<ApiResponse<List<PropertyTraceDto>>>> GetPropertyTraces(string id)
    {
        try
        {
            // Verify property exists
            var property = await _propertyService.GetPropertyByIdAsync(id);
            if (property == null)
            {
                return NotFound(ApiResponse<List<PropertyTraceDto>>.ErrorResult("Property not found"));
            }

            var traces = await _propertyService.GetPropertyTracesAsync(id);
            var traceDtos = _mapper.Map<List<PropertyTraceDto>>(traces);

            return Ok(ApiResponse<List<PropertyTraceDto>>.SuccessResult(traceDtos, "Property traces retrieved successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<List<PropertyTraceDto>>.ErrorResult(
                "An error occurred while fetching property traces",
                new List<string> { ex.Message }
            ));
        }
    }

    [HttpPatch("{id}/status")]
    [ProducesResponseType(typeof(ApiResponse<PropertyDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<ActionResult<ApiResponse<PropertyDto>>> UpdatePropertyStatus(string id, [FromBody] bool enabled)
    {
        try
        {
            var coreUpdateDto = new Core.Models.UpdatePropertyDto { Enabled = enabled };
            var updatedProperty = await _propertyService.UpdatePropertyAsync(id, coreUpdateDto);
            if (updatedProperty == null)
            {
                return NotFound(ApiResponse<PropertyDto>.ErrorResult("Property not found"));
            }

            var propertyDto = _mapper.Map<PropertyDto>(updatedProperty);
            var status = enabled ? "enabled" : "disabled";
            return Ok(ApiResponse<PropertyDto>.SuccessResult(propertyDto, $"Property {status} successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<PropertyDto>.ErrorResult(
                "An error occurred while updating property status",
                new List<string> { ex.Message }
            ));
        }
    }
}