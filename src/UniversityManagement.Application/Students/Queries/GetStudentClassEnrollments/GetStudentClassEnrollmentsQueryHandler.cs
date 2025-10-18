using MediatR;
using UniversityManagement.Application.Users.Interfaces;
using UniversityManagement.Domain.Enums;

namespace UniversityManagement.Application.Students.Queries.GetStudentClassEnrollments
{
    public sealed class GetStudentClassEnrollmentsQueryHandler
        : IRequestHandler<GetStudentClassEnrollmentsQuery, List<StudentClassEnrollmentResponse>>
    {
        private readonly IUserRepository _userRepository;

        public GetStudentClassEnrollmentsQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<StudentClassEnrollmentResponse>> Handle(
            GetStudentClassEnrollmentsQuery request,
            CancellationToken cancellationToken)
        {
            var student = await _userRepository.GetByIdAsync(request.StudentId, cancellationToken);

            if (student is null || student.Role != UserRole.Student)
            {
                throw new KeyNotFoundException($"Student with Id {request.StudentId} not found.");
            }

            var enrollments = await _userRepository.GetStudentClassEnrollmentsAsync(request.StudentId, cancellationToken);

            return enrollments
                .Select(StudentClassEnrollmentResponse.FromEntity)
                .ToList();
        }
    }
}
