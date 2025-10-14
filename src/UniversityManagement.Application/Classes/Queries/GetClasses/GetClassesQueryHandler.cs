using System.Linq;
using MediatR;
using UniversityManagement.Application.Classes.Interfaces;
using UniversityManagement.Application.Common.Models;

namespace UniversityManagement.Application.Classes.Queries.GetClasses
{
    public sealed class GetClassesQueryHandler : IRequestHandler<GetClassesQuery, PaginatedResult<ClassResponse>>
    {
        private readonly IClassRepository _classRepository;

        public GetClassesQueryHandler(IClassRepository classRepository)
        {
            _classRepository = classRepository;
        }

        public async Task<PaginatedResult<ClassResponse>> Handle(GetClassesQuery request, CancellationToken cancellationToken)
        {
            var filter = request.Request ?? new GetClassesRequest();
            var pagedClasses = await _classRepository.GetPagedAsync(filter, cancellationToken);

            var items = pagedClasses.Items
                .Select(@class => new ClassResponse(
                    @class.Id,
                    @class.Name,
                    @class.Description,
                    @class.CreatedAt,
                    @class.ModifiedAt))
                .ToList();

            return new PaginatedResult<ClassResponse>(
                items,
                pagedClasses.PageNumber,
                pagedClasses.PageSize,
                pagedClasses.TotalCount,
                pagedClasses.TotalPages);
        }
    }
}
