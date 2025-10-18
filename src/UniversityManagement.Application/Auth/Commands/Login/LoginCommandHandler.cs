using MediatR;
using UniversityManagement.Application.Common.Interfaces;
using UniversityManagement.Application.Common.Utilities;
using UniversityManagement.Application.Users.Interfaces;

namespace UniversityManagement.Application.Auth.Commands.Login
{
    public sealed class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtService _jwtService;

        public LoginCommandHandler(IUserRepository userRepository, IJwtService jwtService)
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

            if (user == null || !PasswordHasher.Verify(user.PasswordHash, request.Password))
                throw new UnauthorizedAccessException("Invalid credentials.");

            var accessToken = _jwtService.GenerateToken(user);
            var refreshToken = _jwtService.GenerateRefreshToken();
            return new LoginResponse
            (
                 accessToken, refreshToken
            );
        }
    }
}
