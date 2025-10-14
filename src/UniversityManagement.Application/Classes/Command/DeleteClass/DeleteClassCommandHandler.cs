using MediatR;
using UniversityManagement.Application.Classes.Interfaces;

namespace UniversityManagement.Application.Classes.Command.DeleteClass
{
    public sealed class DeleteClassCommandHandler : IRequestHandler<DeleteClassCommand, bool>
    {
        private readonly IClassRepository _classRepository;

        public DeleteClassCommandHandler(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task<bool> Handle(DeleteClassCommand request, CancellationToken cancellationToken)
        {
            var existingClass = await _classRepository.GetByIdAsync(request.Id, cancellationToken);

            if (existingClass is null)
            {
                throw new KeyNotFoundException($"Class with Id {request.Id} not found.");
            }

            await _classRepository.DeleteAsync(existingClass, cancellationToken);

            return true;
        }
    }
}
