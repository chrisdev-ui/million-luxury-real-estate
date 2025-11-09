using Microsoft.AspNetCore.Mvc;
using MillionBackend.API.DTOs;

namespace MillionBackend.API.Controllers;


[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public abstract class ApiControllerBase : ControllerBase
{
    protected ActionResult<T> OkOrNotFound<T>(T? result, string notFoundMessage = "Resource not found.")
    {
        if (result == null)
        {
            return NotFound(ApiResponse<T>.ErrorResult(notFoundMessage));
        }

        return Ok(ApiResponse<T>.SuccessResult(result));
    }

    protected ActionResult<T> CreatedResult<T>(string actionName, object routeValues, T data, string message = "Resource created successfully.")
    {
        return CreatedAtAction(actionName, routeValues, ApiResponse<T>.SuccessResult(data, message));
    }

    protected ActionResult<T> BadRequestResult<T>(string message, List<string>? errors = null)
    {
        return BadRequest(ApiResponse<T>.ErrorResult(message, errors));
    }

    protected ActionResult<ApiResponse<T>> BadRequestFromModelState<T>()
    {
        var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage)
            .ToList();

        return BadRequest(ApiResponse<T>.ValidationErrorResult(errors));
    }

    protected ActionResult<T> InternalServerErrorResult<T>(string message, List<string>? errors = null)
    {
        return StatusCode(500, ApiResponse<T>.ErrorResult(message, errors));
    }

    protected ActionResult<T> UnauthorizedResult<T>(string message = "Unauthorized access.")
    {
        return Unauthorized(ApiResponse<T>.ErrorResult(message));
    }

    protected ActionResult<T> ForbiddenResult<T>(string message = "Access forbidden.")
    {
        return StatusCode(403, ApiResponse<T>.ErrorResult(message));
    }
}