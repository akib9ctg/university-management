using Moq;
using UniversityManagement.Application.Auth.Commands.Login;
using UniversityManagement.Application.Common.Interfaces;
using UniversityManagement.Application.Common.Utilities;
using UniversityManagement.Application.Users.Interfaces;
using UniversityManagement.Domain.Entities;

namespace UniversityManagement.UnitTests.Auth
{
    public class LoginCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Valid_Credentials_With_Hashed_Password_Returns_Tokens()
        {
            const string password = "Test123";
            var hashed = PasswordHasher.Hash(password);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@gmail.com",
                PasswordHash = hashed
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock
                .Setup(repo => repo.GetByEmailAsync(user.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var jwtServiceMock = new Mock<IJwtService>();
            jwtServiceMock.Setup(service => service.GenerateToken(user)).Returns("access-token");
            jwtServiceMock.Setup(service => service.GenerateRefreshToken()).Returns("refresh-token");

            var handler = new LoginCommandHandler(userRepositoryMock.Object, jwtServiceMock.Object);
            var command = new LoginCommand(user.Email, password);

            var response = await handler.Handle(command, CancellationToken.None);

            Assert.Equal("access-token", response.AccessToken);
            Assert.Equal("refresh-token", response.RefreshToken);
            userRepositoryMock.Verify(repo => repo.GetByEmailAsync(user.Email, It.IsAny<CancellationToken>()), Times.Once);
            jwtServiceMock.Verify(service => service.GenerateToken(user), Times.Once);
            jwtServiceMock.Verify(service => service.GenerateRefreshToken(), Times.Once);
        }

        [Fact]
        public async Task Handle_Invalid_Password_Throws_Unauthorized_Access_Exception()
        {
            const string storedPassword = "Test123";
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@gmail.com",
                PasswordHash = PasswordHasher.Hash(storedPassword)
            };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock
                .Setup(repo => repo.GetByEmailAsync(user.Email, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var jwtServiceMock = new Mock<IJwtService>();
            var handler = new LoginCommandHandler(userRepositoryMock.Object, jwtServiceMock.Object);
            var command = new LoginCommand(user.Email, "test");

            await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () => handler.Handle(command, CancellationToken.None));

            jwtServiceMock.Verify(service => service.GenerateToken(It.IsAny<User>()), Times.Never);
            jwtServiceMock.Verify(service => service.GenerateRefreshToken(), Times.Never);
        }
    }
}

