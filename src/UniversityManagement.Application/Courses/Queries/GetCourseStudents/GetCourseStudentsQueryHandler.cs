using MediatR;
using UniversityManagement.Application.Courses.Interfaces;
using UniversityManagement.Application.Students;

namespace UniversityManagement.Application.Courses.Queries.GetCourseStudents
{
    public sealed class GetCourseStudentsQueryHandler : IRequestHandler<GetCourseStudentsQuery, List<StudentResponse>>
    {
        private readonly ICourseRepository _courseRepository;

        public GetCourseStudentsQueryHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<List<StudentResponse>> Handle(GetCourseStudentsQuery request, CancellationToken cancellationToken)
        {
            var students = await _courseRepository.GetStudentsByCourseIdAsync(request.CourseId, cancellationToken);
            return students.Select(StudentResponse.FromEntity).ToList();
        }
    }
}
