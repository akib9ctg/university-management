using UniversityManagement.Domain.Entities;

namespace UniversityManagement.Application.Courses
{
    public sealed record CourseResponse(
        Guid Id,
        string Name,
        string? Description,
        DateTime CreatedAt,
        DateTime? ModifiedAt)
    {
        public static CourseResponse FromEntity(Course course) =>
            new(
                course.Id,
                course.Name,
                course.Description,
                course.CreatedAt,
                course.ModifiedAt);
    }
}
