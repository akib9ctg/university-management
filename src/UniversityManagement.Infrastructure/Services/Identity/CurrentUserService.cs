using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using UniversityManagement.Application.Common.Interfaces;

namespace UniversityManagement.Infrastructure.Services.Identity
{
    public sealed class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string? GetUserId()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
        }
            
    }
}
