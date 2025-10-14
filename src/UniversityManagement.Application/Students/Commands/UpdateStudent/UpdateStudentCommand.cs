using MediatR;

namespace UniversityManagement.Application.Students.Commands.UpdateStudent
{
    public sealed record UpdateStudentCommand(UpdateStudentRequest Request) : IRequest<StudentResponse>;
}
