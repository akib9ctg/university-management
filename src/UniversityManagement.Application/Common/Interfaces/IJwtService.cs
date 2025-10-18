using UniversityManagement.Domain.Entities;

namespace UniversityManagement.Application.Common.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(User user);
        string GenerateRefreshToken();
    }
}
