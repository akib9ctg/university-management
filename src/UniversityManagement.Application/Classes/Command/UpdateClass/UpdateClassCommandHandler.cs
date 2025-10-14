using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UniversityManagement.Application.Classes.Interfaces;

namespace UniversityManagement.Application.Classes.Command.UpdateClass
{
    public sealed class UpdateClassCommandHandler : IRequestHandler<UpdateClassCommand, ClassResponse>
    {
        private readonly IClassRepository _classRepository;

        public UpdateClassCommandHandler(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task<ClassResponse> Handle(UpdateClassCommand request, CancellationToken cancellationToken)
        {
            var existingClass = await _classRepository.GetByIdAsync(request.UpdateClassRequest.Id, cancellationToken);

            if (existingClass is null)
            {
                throw new KeyNotFoundException($"Class with Id {request.UpdateClassRequest.Id} not found.");
            }

            existingClass.Name = request.UpdateClassRequest.Name;
            existingClass.Description = request.UpdateClassRequest.Description;

            await _classRepository.UpdateAsync(existingClass, cancellationToken);

            return ClassResponse.FromEntity(existingClass);
        }
    }
}
