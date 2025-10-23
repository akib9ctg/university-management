using UniversityManagement.Domain.Entities;
using UniversityManagement.Domain.Enums;

namespace UniversityManagement.Application.Students
{
    public sealed record StudentResponse(
        Guid Id,
        string FirstName,
        string LastName,
        string Email,
        DateTime CreatedAt,
        DateTime? ModifiedAt)
    {
        public static StudentResponse FromEntity(User user) =>
            new(
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.CreatedAt,
                user.ModifiedAt);
    }
}
