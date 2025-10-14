using System;

namespace UniversityManagement.Application.Students.Commands.EnrollStudentInClass
{
    public sealed record EnrollStudentInClassRequest(
        Guid StudentId,
        Guid ClassId,
        Guid? AssignedByUserId);
}
