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
            var existing = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
            if (existing is not null)
                throw new InvalidOperationException("User already exists with this email.");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PasswordHash = PasswordHasher.Hash(request.Password),
                Role = UserRole.Student
            };
            await _userRepository.AddAsync(user, cancellationToken);
            return new SignUpResponse(user.Id, user.Email);
        }
    }
}
