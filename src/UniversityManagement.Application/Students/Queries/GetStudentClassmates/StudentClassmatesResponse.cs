namespace UniversityManagement.Application.Students.Queries.GetStudentClassmates
{
    public sealed record StudentClassmatesResponse(
        Guid CourseId,
        string CourseName,
        Guid ClassId,
        string ClassName,
        IReadOnlyList<StudentResponse> Classmates);
}
