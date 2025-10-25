using MediatR;
using UniversityManagement.Application.Common.Utilities;
using UniversityManagement.Application.Users.Interfaces;
using UniversityManagement.Domain.Entities;
using UniversityManagement.Domain.Enums;

namespace UniversityManagement.Application.Students.Commands.CreateStudent
{
    public sealed class CreateStudentCommandHandler : IRequestHandler<CreateStudentCommand, StudentResponse>
    {
        private readonly IUserRepository _userRepository;

        public CreateStudentCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<StudentResponse> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var createRequest = request.Request ?? throw new ArgumentNullException(nameof(request.Request));

            if (!string.Equals(createRequest.Password, createRequest.ConfirmPassword, StringComparison.Ordinal))
            {
                throw new InvalidOperationException("Password and confirm password do not match.");
            }

            if (string.IsNullOrWhiteSpace(createRequest.Email))
            {
                throw new ArgumentException("Email is required.", nameof(createRequest.Email));
            }

            var normalizedEmail = createRequest.Email.Trim();
            var existing = await _userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

            if (existing is not null)
            {
                throw new InvalidOperationException("A user already exists with this email address.");
            }

            var student = new User
            {
                Id = Guid.NewGuid(),
                FirstName = createRequest.FirstName?.Trim() ?? string.Empty,
                LastName = createRequest.LastName?.Trim() ?? string.Empty,
                Email = normalizedEmail,
                PasswordHash = PasswordHasher.Hash(createRequest.Password),
                Role = UserRole.Student
            };

            await _userRepository.AddAsync(student, cancellationToken);

            return StudentResponse.FromEntity(student);
        }
    }
}
