using System;
using System.Collections.Generic;
using MediatR;

namespace UniversityManagement.Application.Students.Queries.GetStudentClassEnrollments
{
    public sealed record GetStudentClassEnrollmentsQuery(Guid StudentId)
        : IRequest<List<StudentClassEnrollmentResponse>>;
}
