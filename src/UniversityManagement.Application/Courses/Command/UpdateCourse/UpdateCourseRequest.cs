namespace UniversityManagement.Application.Courses.Command.UpdateCourse
{
    public sealed record UpdateCourseRequest
    (
        Guid Id,
        string Name,
        string Description
    );
}
