using MediatR;

namespace UniversityManagement.Application.Students.Commands.EnrollStudentInCourse
{
    public sealed record EnrollStudentInCourseCommand
        (EnrollStudentInCourseRequest Request)
        : IRequest<StudentResponse>;
}
