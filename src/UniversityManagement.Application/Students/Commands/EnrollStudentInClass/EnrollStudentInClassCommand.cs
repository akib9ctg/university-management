using MediatR;

namespace UniversityManagement.Application.Students.Commands.EnrollStudentInClass
{
    public sealed record EnrollStudentInClassCommand(EnrollStudentInClassRequest Request) : IRequest<StudentResponse>;
}
