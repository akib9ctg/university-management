using MediatR;
using UniversityManagement.Application.Courses.Interfaces;

namespace UniversityManagement.Application.Courses.Command.UpdateCourse
{
    public sealed class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, CourseResponse>
    {
        private readonly ICourseRepository _courseRepository;

        public UpdateCourseCommandHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<CourseResponse> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await _courseRepository.GetByIdAsync(request.updateCourseRequest.Id, cancellationToken);

            if (course is null)
            {
                throw new KeyNotFoundException($"Course with Id {request.updateCourseRequest.Id} not found.");
            }

            course.Name = request.updateCourseRequest.Name;
            course.Description = request.updateCourseRequest.Description;

            await _courseRepository.UpdateAsync(course, cancellationToken);

            return CourseResponse.FromEntity(course);
        }
    }
}
