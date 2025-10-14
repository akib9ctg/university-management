using UniversityManagement.Application.Common.Models;
using UniversityManagement.Domain.Entities;

namespace UniversityManagement.Application.Students.Queries.GetStudents
{
    public sealed record GetStudentsRequest
    {
        public const string DefaultOrderBy = nameof(User.FirstName);

        public int PageNumber { get; init; } = PaginationDefaults.DefaultPageNumber;
        public int PageSize { get; init; } = PaginationDefaults.DefaultPageSize;
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? Email { get; init; }
        public string OrderBy { get; init; } = DefaultOrderBy;
        public SortDirection SortDirection { get; init; } = SortDirection.Asc;
    }
}
