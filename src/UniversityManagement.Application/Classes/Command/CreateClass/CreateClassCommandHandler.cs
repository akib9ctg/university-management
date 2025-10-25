using MediatR;
using UniversityManagement.Application.Classes.Interfaces;
using UniversityManagement.Domain.Entities;

namespace UniversityManagement.Application.Classes.Command.CreateClass
{
    public sealed class CreateClassCommandHandler : IRequestHandler<CreateClassCommand, ClassResponse>
    {
        private readonly IClassRepository _classRepository;

        public CreateClassCommandHandler(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task<ClassResponse> Handle(CreateClassCommand request, CancellationToken cancellationToken)
        {
            var createRequest = request.CreateClassRequest ?? throw new ArgumentNullException(nameof(request.CreateClassRequest));
            if (string.IsNullOrWhiteSpace(createRequest.name))
            {
                throw new ArgumentException("Class name is required.", nameof(createRequest.name));
            }

            var trimmedName = createRequest.name.Trim();
            var exists = await _classRepository.ExistsByNameAsync(trimmedName, cancellationToken);

            if (exists)
            {
                throw new InvalidOperationException($"A class named '{trimmedName}' already exists.");
            }

            var newClass = new Class
            {
                Name = trimmedName,
                Description = createRequest.description
            };

            await _classRepository.AddAsync(newClass, cancellationToken);

            return ClassResponse.FromEntity(newClass);
        }
    }
}
