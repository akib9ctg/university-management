using MediatR;

namespace UniversityManagement.Application.Classes.Queries.GetClassById
{
    public sealed record GetClassByIdQuery(Guid Id) : IRequest<ClassResponse>;
}
