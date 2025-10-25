using Microsoft.AspNetCore.Mvc;
using UniversityManagement.Application.Common.Models;

namespace UniversityManagement.API.Controllers
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected IActionResult Success<T>(T data, string message = "Success", int statusCode = StatusCodes.Status200OK)
        {
            return StatusCode(statusCode, ApiResponse<T>.Ok(data, message));
        }

        protected IActionResult SuccessCreatedAtAction<T>(
            string actionName,
            object? routeValues,
            T data,
            string message = "Created")
        {
            return CreatedAtAction(actionName, routeValues, ApiResponse<T>.Ok(data, message));
        }

        protected IActionResult Failure(string message, int status = 400)
        {
            return StatusCode(status, ApiResponse<object>.Fail(message));
        }
    }
}
