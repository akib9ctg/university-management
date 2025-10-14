using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniversityManagement.Application.Classes.Interfaces;
using UniversityManagement.Application.Courses;

namespace UniversityManagement.Application.Classes.Queries.GetClassCourses
{
    public sealed class GetClassCoursesQueryHandler : IRequestHandler<GetClassCoursesQuery, List<CourseResponse>>
    {
        private readonly IClassRepository _classRepository;

        public GetClassCoursesQueryHandler(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task<List<CourseResponse>> Handle(GetClassCoursesQuery request, CancellationToken cancellationToken)
        {
            var courses = await _classRepository.GetCoursesByClassIdAsync(request.ClassId, cancellationToken);
            return courses.Select(CourseResponse.FromEntity).ToList();
        }
    }
}
