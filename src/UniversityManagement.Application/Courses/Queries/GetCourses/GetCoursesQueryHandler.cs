using System.Linq;
using MediatR;
using UniversityManagement.Application.Common.Models;
using UniversityManagement.Application.Courses.Interfaces;

namespace UniversityManagement.Application.Courses.Queries.GetCourses
{
    public sealed class GetCoursesQueryHandler : IRequestHandler<GetCoursesQuery, PaginatedResult<CourseResponse>>
    {
        private readonly ICourseRepository _courseRepository;

        public GetCoursesQueryHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<PaginatedResult<CourseResponse>> Handle(GetCoursesQuery request, CancellationToken cancellationToken)
        {
            var filter = request.Request ?? new GetCoursesRequest();
            var pagedCourses = await _courseRepository.GetPagedAsync(filter, cancellationToken);

            var items = pagedCourses.Items
                .Select(CourseResponse.FromEntity)
                .ToList();

            return new PaginatedResult<CourseResponse>(
                items,
                pagedCourses.PageNumber,
                pagedCourses.PageSize,
                pagedCourses.TotalCount,
                pagedCourses.TotalPages);
        }
    }
}
