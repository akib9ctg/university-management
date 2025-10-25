using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UniversityManagement.Application.Auth.Commands.Login;
using UniversityManagement.Application.Auth.Commands.SignUp;

namespace UniversityManagement.API.Controllers
{
    [Route("api/auth")]
    public class AuthController : BaseApiController
    {
        private readonly ISender _sender;
        public AuthController(ISender sender)
        {
            _sender = sender;
        }

        [Authorize(Policy = PolicyNames.StaffOnly)]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpRequest request, CancellationToken token = default)
        {
            var result = await _sender.Send(new SignUpCommand(request), token);
            return Success(result, "Signup successful", StatusCodes.Status201Created);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request, CancellationToken token = default)
        {
            var result = await _sender.Send(new LoginCommand(request.Email, request.Password), token);
            return Success(result, "Login successful");
        }
    }
}
