using System;

namespace UniversityManagement.Application.Students.Commands.UpdateStudent
{
    public sealed record UpdateStudentRequest
    (
        Guid Id,
        string FirstName,
        string LastName,
        string Email
    );
}
