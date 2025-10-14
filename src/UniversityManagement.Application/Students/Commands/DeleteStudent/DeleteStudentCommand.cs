using System;
using MediatR;

namespace UniversityManagement.Application.Students.Commands.DeleteStudent
{
    public sealed record DeleteStudentCommand(Guid Id) : IRequest<bool>;
}
