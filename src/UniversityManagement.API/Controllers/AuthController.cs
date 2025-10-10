using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using UniversityManagement.Application.Auth.Commands.Login;
using UniversityManagement.Application.Auth.Commands.SignUp;
using UniversityManagement.Application.Common.Models;

namespace UniversityManagement.API.Controllers
{
    public class AuthController : BaseApiController
    {
        private readonly ILogger<AuthController> _logger;
        private readonly ISender _sender;
        public AuthController(ILogger<AuthController> logger, ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }


        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpRequest request, CancellationToken token)
        {
            try
            {
                var result = await _sender.Send(new SignUpCommand(request.Email, request.Password, request.FirstName, request.LastName), token);
                return Success(result, "Signup successful");
            }
            catch (Exception ex)
            {
                return Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request, CancellationToken token)
        {
            try
            {
                var result = await _sender.Send(new LoginCommand(request.Email, request.Password), token);
                return Success(result, "Login successful");
            }
            catch (UnauthorizedAccessException)
            {
                return Failure("Invalid credentials", StatusCodes.Status401Unauthorized);
            }
            catch (Exception ex)
            {
                return Failure(ex.Message, StatusCodes.Status500InternalServerError);
            }
        }
    }
}
