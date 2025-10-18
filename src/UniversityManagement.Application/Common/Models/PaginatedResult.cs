namespace UniversityManagement.Application.Common.Models
{
    public sealed record PaginatedResult<T>
    (
        List<T> Items,
        int PageNumber,
        int PageSize,
        int TotalCount,
        int TotalPages
    );
}
