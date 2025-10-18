using MediatR;
using UniversityManagement.Application.Common.Models;

namespace UniversityManagement.Application.Students.Queries.GetStudents
{
    public sealed record GetStudentsQuery(GetStudentsRequest Request) : IRequest<PaginatedResult<StudentResponse>>;
}
