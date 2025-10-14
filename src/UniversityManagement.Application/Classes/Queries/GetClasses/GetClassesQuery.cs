using MediatR;
using UniversityManagement.Application.Common.Models;

namespace UniversityManagement.Application.Classes.Queries.GetClasses
{
    public sealed record GetClassesQuery(GetClassesRequest Request) : IRequest<PaginatedResult<ClassResponse>>;
}
