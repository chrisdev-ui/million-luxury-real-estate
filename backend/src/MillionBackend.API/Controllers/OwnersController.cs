using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MillionBackend.API.DTOs;
using MillionBackend.Application.Services;
using MillionBackend.Core.Models;

namespace MillionBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class OwnersController : ApiControllerBase
{
    private readonly IOwnerService _ownerService;
    private readonly IMapper _mapper;

    public OwnersController(IOwnerService ownerService, IMapper mapper)
    {
        _ownerService = ownerService;
        _mapper = mapper;
    }

    /// <summary>
    /// Get all owners
    /// </summary>
    /// <returns>List of owners</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<OwnerDto>>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<ActionResult<ApiResponse<List<OwnerDto>>>> GetOwners()
    {
        try
        {
            var owners = await _ownerService.GetOwnersAsync();
            var ownerDtos = _mapper.Map<List<OwnerDto>>(owners);

            return Ok(ApiResponse<List<OwnerDto>>.SuccessResult(ownerDtos, "Owners retrieved successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<List<OwnerDto>>.ErrorResult(
                "An error occurred while fetching owners",
                new List<string> { ex.Message }
            ));
        }
    }

    /// <summary>
    /// Get a specific owner by ID
    /// </summary>
    /// <param name="id">Owner ID</param>
    /// <returns>Owner details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<OwnerDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<ActionResult<ApiResponse<OwnerDto>>> GetOwner(string id)
    {
        try
        {
            var owner = await _ownerService.GetOwnerByIdAsync(id);
            if (owner == null)
            {
                return NotFound(ApiResponse<OwnerDto>.ErrorResult("Owner not found"));
            }

            var ownerDto = _mapper.Map<OwnerDto>(owner);
            return Ok(ApiResponse<OwnerDto>.SuccessResult(ownerDto, "Owner retrieved successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<OwnerDto>.ErrorResult(
                "An error occurred while fetching the owner",
                new List<string> { ex.Message }
            ));
        }
    }

    /// <summary>
    /// Create a new owner
    /// </summary>
    /// <param name="createDto">Owner creation data</param>
    /// <returns>Created owner</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<OwnerDto>), 201)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    public async Task<ActionResult<ApiResponse<OwnerDto>>> CreateOwner([FromBody] CreateOwnerDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequestFromModelState<OwnerDto>();
            }

            var owner = _mapper.Map<Owner>(createDto);
            var createdOwner = await _ownerService.CreateOwnerAsync(owner);
            var ownerDto = _mapper.Map<OwnerDto>(createdOwner);

            return CreatedAtAction(
                nameof(GetOwner),
                new { id = ownerDto.IdOwner },
                ApiResponse<OwnerDto>.SuccessResult(ownerDto, "Owner created successfully")
            );
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<OwnerDto>.ErrorResult(
                "An error occurred while creating the owner",
                new List<string> { ex.Message }
            ));
        }
    }

    /// <summary>
    /// Update an existing owner
    /// </summary>
    /// <param name="id">Owner ID</param>
    /// <param name="updateDto">Owner update data</param>
    /// <returns>Updated owner</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<OwnerDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<ActionResult<ApiResponse<OwnerDto>>> UpdateOwner(string id, [FromBody] CreateOwnerDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequestFromModelState<OwnerDto>();
            }

            var existingOwner = await _ownerService.GetOwnerByIdAsync(id);
            if (existingOwner == null)
            {
                return NotFound(ApiResponse<OwnerDto>.ErrorResult("Owner not found"));
            }

            _mapper.Map(updateDto, existingOwner);
            var updatedOwner = await _ownerService.UpdateOwnerAsync(existingOwner);

            var ownerDto = _mapper.Map<OwnerDto>(updatedOwner!);
            return Ok(ApiResponse<OwnerDto>.SuccessResult(ownerDto, "Owner updated successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<OwnerDto>.ErrorResult(
                "An error occurred while updating the owner",
                new List<string> { ex.Message }
            ));
        }
    }

    /// <summary>
    /// Delete an owner
    /// </summary>
    /// <param name="id">Owner ID</param>
    /// <returns>Success message</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), 200)]
    [ProducesResponseType(typeof(ApiResponse<object>), 400)]
    [ProducesResponseType(typeof(ApiResponse<object>), 404)]
    public async Task<ActionResult<ApiResponse>> DeleteOwner(string id)
    {
        try
        {
            var result = await _ownerService.DeleteOwnerAsync(id);
            if (!result)
            {
                return NotFound(ApiResponse.ErrorResult("Owner not found"));
            }

            return Ok(ApiResponse.SuccessResult("Owner deleted successfully"));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse.ErrorResult(
                "An error occurred while deleting the owner",
                new List<string> { ex.Message }
            ));
        }
    }
}
