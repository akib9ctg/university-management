using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UniversityManagement.Application.Classes.Interfaces;
using UniversityManagement.Application.Students;
using UniversityManagement.Application.Users.Interfaces;
using UniversityManagement.Domain.Enums;

namespace UniversityManagement.Application.Students.Commands.EnrollStudentInClass
{
    public sealed class EnrollStudentInClassCommandHandler : IRequestHandler<EnrollStudentInClassCommand, StudentResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IClassRepository _classRepository;

        public EnrollStudentInClassCommandHandler(IUserRepository userRepository, IClassRepository classRepository)
        {
            _userRepository = userRepository;
            _classRepository = classRepository;
        }

        public async Task<StudentResponse> Handle(EnrollStudentInClassCommand request, CancellationToken cancellationToken)
        {
            var enrollRequest = request.Request ?? throw new ArgumentNullException(nameof(request.Request));

            var student = await _userRepository.GetByIdAsync(enrollRequest.StudentId, cancellationToken);

            if (student is null || student.Role != UserRole.Student)
            {
                throw new KeyNotFoundException($"Student with Id {enrollRequest.StudentId} not found.");
            }

            var @class = await _classRepository.GetByIdAsync(enrollRequest.ClassId, cancellationToken);
            if (@class is null)
            {
                throw new KeyNotFoundException($"Class with Id {enrollRequest.ClassId} not found.");
            }

            await _userRepository.EnrollStudentInClassAsync(student.Id, @class.Id, enrollRequest.AssignedByUserId, cancellationToken);

            return StudentResponse.FromEntity(student);
        }
    }
}
