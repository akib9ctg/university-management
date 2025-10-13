using MediatR;
using UniversityManagement.Application.Classes.Interfaces;

namespace UniversityManagement.Application.Classes.Queries.GetClassById
{
    public sealed class GetClassByIdQueryHandler : IRequestHandler<GetClassByIdQuery, ClassResponse>
    {
        private readonly IClassRepository _classRepository;

        public GetClassByIdQueryHandler(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task<ClassResponse> Handle(GetClassByIdQuery request, CancellationToken cancellationToken)
        {
            var existingClass = await _classRepository.GetByIdAsync(request.Id, cancellationToken);

            if (existingClass is null)
            {
                throw new KeyNotFoundException($"Class with Id {request.Id} not found.");
            }

            return new ClassResponse(
                existingClass.Id,
                existingClass.Name,
                existingClass.Description,
                existingClass.CreatedAt,
                existingClass.ModifiedAt
            );
        }
    }
}
