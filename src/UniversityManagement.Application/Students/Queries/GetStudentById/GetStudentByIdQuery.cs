using System;
using MediatR;

namespace UniversityManagement.Application.Students.Queries.GetStudentById
{
    public sealed record GetStudentByIdQuery(Guid Id) : IRequest<StudentResponse>;
}
