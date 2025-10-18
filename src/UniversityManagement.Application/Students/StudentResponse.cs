using UniversityManagement.Domain.Entities;
using UniversityManagement.Domain.Enums;

namespace UniversityManagement.Application.Students
{
    public sealed record StudentResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        UserRole Role,
        DateTime CreatedAt,
        DateTime? ModifiedAt)
    {
        public static StudentResponse FromEntity(User user) =>
            new(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Role,
                user.CreatedAt,
                user.ModifiedAt);
    }
}
