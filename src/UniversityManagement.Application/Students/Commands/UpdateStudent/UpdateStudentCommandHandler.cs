using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UniversityManagement.Application.Students;
using UniversityManagement.Application.Users.Interfaces;
using UniversityManagement.Domain.Enums;

namespace UniversityManagement.Application.Students.Commands.UpdateStudent
{
    public sealed class UpdateStudentCommandHandler : IRequestHandler<UpdateStudentCommand, StudentResponse>
    {
        private readonly IUserRepository _userRepository;

        public UpdateStudentCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<StudentResponse> Handle(UpdateStudentCommand request, CancellationToken cancellationToken)
        {
            var updateRequest = request.Request ?? throw new ArgumentNullException(nameof(request.Request));

            var student = await _userRepository.GetByIdAsync(updateRequest.Id, cancellationToken);

            if (student is null || student.Role != UserRole.Student)
            {
                throw new KeyNotFoundException($"Student with Id {updateRequest.Id} not found.");
            }

            if (!string.Equals(student.Email, updateRequest.Email, StringComparison.OrdinalIgnoreCase))
            {
                var existing = await _userRepository.GetByEmailAsync(updateRequest.Email, cancellationToken);
                if (existing is not null && existing.Id != student.Id)
                {
                    throw new InvalidOperationException("Another user already exists with this email address.");
                }
            }

            student.FirstName = updateRequest.FirstName;
            student.LastName = updateRequest.LastName;
            student.Email = updateRequest.Email;
            student.Role = UserRole.Student;

            await _userRepository.UpdateAsync(student, cancellationToken);

            return StudentResponse.FromEntity(student);
        }
    }
}
