using MediatR;
using UniversityManagement.Application.Common.Utilities;
using UniversityManagement.Application.Users.Interfaces;
using UniversityManagement.Domain.Entities;
using UniversityManagement.Domain.Enums;

namespace UniversityManagement.Application.Auth.Commands.SignUp
{
    public sealed class SignUpCommandHandler : IRequestHandler<SignUpCommand, SignUpResponse>
    {
        private readonly IUserRepository _userRepository;
        public SignUpCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<SignUpResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var signUpRequest = request.SignUpRequest ?? throw new ArgumentNullException(nameof(request.SignUpRequest));

            if (string.IsNullOrWhiteSpace(signUpRequest.Email))
            {
                throw new ArgumentException("Email is required.", nameof(signUpRequest.Email));
            }

            var normalizedEmail = signUpRequest.Email.Trim();
            var existing = await _userRepository.GetByEmailAsync(normalizedEmail, cancellationToken);

            if (existing is not null)
            {
                throw new InvalidOperationException("User already exists with this email.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = normalizedEmail,
                FirstName = signUpRequest.FirstName?.Trim() ?? string.Empty,
                LastName = signUpRequest.LastName?.Trim() ?? string.Empty,
                PasswordHash = PasswordHasher.Hash(signUpRequest.Password),
                Role = UserRole.Staff
            };

            await _userRepository.AddAsync(user, cancellationToken);
            return new SignUpResponse(user.Id, user.Email);
        }
    }
}
