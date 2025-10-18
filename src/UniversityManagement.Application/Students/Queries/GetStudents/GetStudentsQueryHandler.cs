using MediatR;
using UniversityManagement.Application.Common.Models;
using UniversityManagement.Application.Users.Interfaces;

namespace UniversityManagement.Application.Students.Queries.GetStudents
{
    public sealed class GetStudentsQueryHandler : IRequestHandler<GetStudentsQuery, PaginatedResult<StudentResponse>>
    {
        private readonly IUserRepository _userRepository;

        public GetStudentsQueryHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<PaginatedResult<StudentResponse>> Handle(GetStudentsQuery request, CancellationToken cancellationToken)
        {
            var filter = request.Request ?? new GetStudentsRequest();

            var pagedStudents = await _userRepository.GetStudentsPagedAsync(filter, cancellationToken);

            var items = pagedStudents.Items
                .Select(StudentResponse.FromEntity)
                .ToList();

            return new PaginatedResult<StudentResponse>(
                items,
                pagedStudents.PageNumber,
                pagedStudents.PageSize,
                pagedStudents.TotalCount,
                pagedStudents.TotalPages);
        }
    }
}
