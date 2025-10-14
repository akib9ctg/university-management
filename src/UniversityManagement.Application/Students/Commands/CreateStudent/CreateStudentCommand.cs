using MediatR;

namespace UniversityManagement.Application.Students.Commands.CreateStudent
{
    public sealed record CreateStudentCommand(CreateStudentRequest Request) : IRequest<StudentResponse>;
}
