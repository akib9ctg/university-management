using MediatR;

namespace UniversityManagement.Application.Classes.Command.CreateClass
{
    public sealed record CreateClassCommand(CreateClassRequest CreateClassRequest) : IRequest<ClassResponse>;
}
