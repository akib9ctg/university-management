using MediatR;
using UniversityManagement.Application.Courses.Interfaces;
using UniversityManagement.Application.Users.Interfaces;
using UniversityManagement.Domain.Enums;

namespace UniversityManagement.Application.Students.Commands.EnrollStudentInCourse
{
    public sealed class EnrollStudentInCourseCommandHandler : IRequestHandler<EnrollStudentInCourseCommand, StudentResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICourseRepository _courseRepository;

        public EnrollStudentInCourseCommandHandler(IUserRepository userRepository, ICourseRepository courseRepository)
        {
            _userRepository = userRepository;
            _courseRepository = courseRepository;
        }

        public async Task<StudentResponse> Handle(EnrollStudentInCourseCommand request, CancellationToken cancellationToken)
        {
            var enrollRequest = request.Request ?? throw new ArgumentNullException(nameof(request.Request));

            var student = await _userRepository.GetByIdAsync(enrollRequest.StudentId, cancellationToken);
            if (student is null || student.Role != UserRole.Student)
            {
                throw new KeyNotFoundException($"Student with Id {enrollRequest.StudentId} not found.");
            }

            var course = await _courseRepository.GetByIdAsync(enrollRequest.CourseId, cancellationToken);
            if (course is null)
            {
                throw new KeyNotFoundException($"Course with Id {enrollRequest.CourseId} not found.");
            }

            await _userRepository.EnrollStudentInCourseAsync(student.Id, course.Id, enrollRequest.AssignedByUserId, cancellationToken);

            return StudentResponse.FromEntity(student);
        }
    }
}
