using MediatR;

namespace UniversityManagement.Application.Classes.Command.DeleteClass
{
    public sealed record DeleteClassCommand(Guid Id) : IRequest<bool>;
}
