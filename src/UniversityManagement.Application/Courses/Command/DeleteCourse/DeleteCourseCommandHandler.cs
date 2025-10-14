using MediatR;
using UniversityManagement.Application.Courses.Interfaces;

namespace UniversityManagement.Application.Courses.Command.DeleteCourse
{
    public sealed class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, bool>
    {
        private readonly ICourseRepository _courseRepository;

        public DeleteCourseCommandHandler(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }

        public async Task<bool> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await _courseRepository.GetByIdAsync(request.Id, cancellationToken);

            if (course is null)
            {
                throw new KeyNotFoundException($"Course with Id {request.Id} not found.");
            }

            await _courseRepository.DeleteAsync(course, cancellationToken);

            return true;
        }
    }
}
