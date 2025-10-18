using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using UniversityManagement.Application.Classes.Interfaces;
using UniversityManagement.Application.Courses.Interfaces;

namespace UniversityManagement.Application.Courses.Command.AssignClassToCourse
{
    public sealed class AssignClassToCourseCommandHandler : IRequestHandler<AssignClassToCourseCommand, bool>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IClassRepository _classRepository;

        public AssignClassToCourseCommandHandler(ICourseRepository courseRepository, IClassRepository classRepository)
        {
            _courseRepository = courseRepository;
            _classRepository = classRepository;
        }

        public async Task<bool> Handle(AssignClassToCourseCommand request, CancellationToken cancellationToken)
        {
            var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);
            if (course is null)
            {
                throw new KeyNotFoundException($"Course with Id {request.CourseId} was not found.");
            }

            var @class = await _classRepository.GetByIdAsync(request.ClassId, cancellationToken);
            if (@class is null)
            {
                throw new KeyNotFoundException($"Class with Id {request.ClassId} was not found.");
            }

            return await _courseRepository.AddClassToCourseAsync(request.CourseId, request.ClassId, cancellationToken);
        }
    }
}
