using System;

namespace UniversityManagement.Application.Students.Commands.EnrollStudentInCourse
{
    public sealed record EnrollStudentInCourseRequest(
        Guid StudentId,
        Guid CourseId,
        Guid? AssignedByUserId);
}
