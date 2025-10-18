using MediatR;
using UniversityManagement.Application.Courses.Interfaces;

namespace UniversityManagement.Application.Courses.Queries.GetCourseById
{
    public sealed class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, CourseResponse>
    {
        private readonly ICourseRepository _courseRepository;

        public GetCourseByIdQueryHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<CourseResponse> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            var course = await _courseRepository.GetByIdAsync(request.Id, cancellationToken);

            if (course is null)
            {
                throw new KeyNotFoundException($"Course with Id {request.Id} not found.");
            }

            return CourseResponse.FromEntity(course);
        }
    }
}
