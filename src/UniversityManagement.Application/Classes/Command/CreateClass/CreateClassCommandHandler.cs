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
            var newClass = new Class
            {
                Name = request.CreateClassRequest.name,
                Description = request.CreateClassRequest.description,
            };

            await _classRepository.AddAsync(newClass, cancellationToken);

            return ClassResponse.FromEntity(newClass);
        }
    }
}
