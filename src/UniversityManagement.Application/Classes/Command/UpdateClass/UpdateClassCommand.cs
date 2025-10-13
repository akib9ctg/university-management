using MediatR;

namespace UniversityManagement.Application.Classes.Command.UpdateClass
{
    public sealed record UpdateClassCommand(UpdateClassRequest UpdateClassRequest) : IRequest<ClassResponse>;
}
