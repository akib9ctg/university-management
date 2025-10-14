using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UniversityManagement.Application.Classes;
using UniversityManagement.Application.Courses.Interfaces;

namespace UniversityManagement.Application.Courses.Queries.GetCourseClasses
{
    public sealed class GetCourseClassesQueryHandler : IRequestHandler<GetCourseClassesQuery, List<ClassResponse>>
    {
        private readonly ICourseRepository _courseRepository;

        public GetCourseClassesQueryHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<List<ClassResponse>> Handle(GetCourseClassesQuery request, CancellationToken cancellationToken)
        {
            var classes = await _courseRepository.GetClassesByCourseIdAsync(request.CourseId, cancellationToken);
            return classes.Select(ClassResponse.FromEntity).ToList();
        }
    }
}
