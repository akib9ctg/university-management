using MediatR;
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
            var createRequest = request.createCourseRequest ?? throw new ArgumentNullException(nameof(request.createCourseRequest));
            if (string.IsNullOrWhiteSpace(createRequest.name))
            {
                throw new ArgumentException("Course name is required.", nameof(createRequest.name));
            }

            var trimmedName = createRequest.name.Trim();
            var exists = await _courseRepository.ExistsByNameAsync(trimmedName, cancellationToken);

            if (exists)
            {
                throw new InvalidOperationException($"A course named '{trimmedName}' already exists.");
            }

            var course = new Course
            {
                Name = trimmedName,
                Description = createRequest.description
            };

            await _courseRepository.AddAsync(course, cancellationToken);

            return CourseResponse.FromEntity(course);
        }
    }
}
