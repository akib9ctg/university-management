using Moq;
using UniversityManagement.Application.Classes.Command.CreateClass;
using UniversityManagement.Application.Classes.Interfaces;
using UniversityManagement.Domain.Entities;

namespace UniversityManagement.UnitTests.Classes
{
    public class CreateClassCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Creates_Class_And_Returns_Response()
        {
            var request = new CreateClassRequest("Algorithms", "Study of algorithms");
            Class? savedClass = null;

            var repositoryMock = new Mock<IClassRepository>();
            repositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Class>(), It.IsAny<CancellationToken>()))
                .Callback<Class, CancellationToken>((entity, _) => savedClass = entity)
                .Returns(Task.CompletedTask);

            var handler = new CreateClassCommandHandler(repositoryMock.Object);
            var command = new CreateClassCommand(request);

            var response = await handler.Handle(command, CancellationToken.None);

            repositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Class>(), It.IsAny<CancellationToken>()), Times.Once);
            repositoryMock.VerifyNoOtherCalls();

            Assert.NotNull(savedClass);
            Assert.Equal(request.name, savedClass!.Name);
            Assert.Equal(request.description, savedClass.Description);

            Assert.Equal(savedClass.Id, response.Id);
            Assert.Equal(savedClass.Name, response.Name);
            Assert.Equal(savedClass.Description, response.Description);
            Assert.Equal(savedClass.CreatedAt, response.CreatedAt);
        }
    }
}

