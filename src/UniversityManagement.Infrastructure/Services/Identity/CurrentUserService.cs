using Microsoft.AspNetCore.Http;
using UniversityManagement.Application.Common.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UniversityManagement.Infrastructure.Services.Identity
{
    public sealed class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext?.User;

            var userIdString = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                ?? principal?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

            if (Guid.TryParse(userIdString, out var userId))
            {
                return userId;
            }

            return null;
        }

    }
}
