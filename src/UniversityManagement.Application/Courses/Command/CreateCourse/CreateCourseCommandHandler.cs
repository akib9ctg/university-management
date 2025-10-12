using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityManagement.Application.Courses.Interfaces;
using UniversityManagement.Domain.Entities;

namespace UniversityManagement.Application.Courses.Command.CreateCourse
{
    public sealed class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, CourseResponse>
    {
        private readonly ICourseRepository _courseRepository;
        public CreateCourseCommandHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }
        public async Task<CourseResponse> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
        {
            var course = new Course
            {
                Name = request.createCourseRequest.name,
                Description = request.createCourseRequest.description,
            };
            await _courseRepository.AddAsync(course,cancellationToken);

            return new CourseResponse
            (
                course.Id,
                course.Name,
                course.Description,
                course.CreatedAt,
                course.ModifiedAt
            );
        }
    }
}
