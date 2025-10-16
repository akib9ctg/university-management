using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UniversityManagement.Application.Users.Interfaces;
using UniversityManagement.Domain.Enums;

namespace UniversityManagement.Application.Students.Queries.GetStudentCoursesAndClasses
{
    public sealed class GetStudentCoursesAndClassesQueryHandler
        : IRequestHandler<GetStudentCoursesAndClassesQuery, List<StudentCourseClassResponse>>
    {
        private readonly IUserRepository _userRepository;

        public GetStudentCoursesAndClassesQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<StudentCourseClassResponse>> Handle(
            GetStudentCoursesAndClassesQuery request,
            CancellationToken cancellationToken)
        {
            var student = await _userRepository.GetByIdAsync(request.StudentId, cancellationToken);

            if (student is null || student.Role != UserRole.Student)
            {
                throw new KeyNotFoundException($"Student with Id {request.StudentId} not found.");
            }

            var enrollments = await _userRepository.GetStudentClassEnrollmentsAsync(request.StudentId, cancellationToken);

            if (enrollments.Count == 0)
            {
                return new List<StudentCourseClassResponse>();
            }

            return enrollments
                .Where(enrollment => enrollment.Course is not null && enrollment.Class is not null)
                .Select(StudentCourseClassResponse.FromEntity)
                .ToList();
        }
    }
}
