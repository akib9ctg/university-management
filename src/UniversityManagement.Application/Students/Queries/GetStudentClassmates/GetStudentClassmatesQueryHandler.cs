using MediatR;
using UniversityManagement.Application.Users.Interfaces;
using UniversityManagement.Domain.Enums;

namespace UniversityManagement.Application.Students.Queries.GetStudentClassmates
{
    public sealed class GetStudentClassmatesQueryHandler
        : IRequestHandler<GetStudentClassmatesQuery, List<StudentClassmatesResponse>>
    {
        private readonly IUserRepository _userRepository;

        public GetStudentClassmatesQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<StudentClassmatesResponse>> Handle(
            GetStudentClassmatesQuery request,
            CancellationToken cancellationToken)
        {
            var student = await _userRepository.GetByIdAsync(request.StudentId, cancellationToken);

            if (student is null || student.Role != UserRole.Student)
            {
                throw new KeyNotFoundException($"Student with Id {request.StudentId} not found.");
            }

            var classmates = await _userRepository.GetStudentClassmatesAsync(request.StudentId, cancellationToken);

            if (classmates.Count == 0)
            {
                return new List<StudentClassmatesResponse>();
            }

            var responses = new List<StudentClassmatesResponse>();

            foreach (var group in classmates.GroupBy(ucc => new
            {
                ucc.ClassId,
                ClassName = ucc.Class.Name,
                ucc.CourseId,
                CourseName = ucc.Course.Name
            }))
            {
                var students = group
                    .Where(entry => entry.UserId != request.StudentId)
                    .Select(entry => StudentResponse.FromEntity(entry.User))
                    .ToList();

                if (students.Count == 0)
                {
                    continue;
                }

                responses.Add(new StudentClassmatesResponse(
                    group.Key.CourseId,
                    group.Key.CourseName,
                    group.Key.ClassId,
                    group.Key.ClassName,
                    students));
            }

            return responses;
        }
    }
}
