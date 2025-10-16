using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using UniversityManagement.Application.Common.Models;

namespace UniversityManagement.API.Controllers
{
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected Guid? GetCurrentUserId()
        {
            var subjectClaim = User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            return Guid.TryParse(subjectClaim, out var studentId)
                ? studentId
                : null;
        }
        protected IActionResult Success<T>(T data, string message = "Success")
        {
             return Ok(ApiResponse<T>.Ok(data, message));
        }

        protected IActionResult Failure(string message, int status = 400)
        {
            return StatusCode(status, ApiResponse<object>.Fail(message));
        }
    }
}
