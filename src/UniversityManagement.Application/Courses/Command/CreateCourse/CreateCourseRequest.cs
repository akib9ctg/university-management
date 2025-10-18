namespace UniversityManagement.Application.Courses.Command.CreateCourse
{
    public sealed record CreateCourseRequest
    (
        string name,
        string description
    );
}
