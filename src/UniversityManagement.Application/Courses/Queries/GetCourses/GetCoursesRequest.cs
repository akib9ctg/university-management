using UniversityManagement.Application.Common.Models;
using UniversityManagement.Domain.Entities;

namespace UniversityManagement.Application.Courses.Queries.GetCourses
{
    public sealed record GetCoursesRequest
    {
        public const string DefaultOrderBy = nameof(Course.Name);

        public int PageNumber { get; init; } = PaginationDefaults.DefaultPageNumber;
        public int PageSize { get; init; } = PaginationDefaults.DefaultPageSize;
        public string? Name { get; init; }
        public string? Description { get; init; }
        public string OrderBy { get; init; } = DefaultOrderBy;
        public SortDirection SortDirection { get; init; } = SortDirection.Asc;
    }
}
