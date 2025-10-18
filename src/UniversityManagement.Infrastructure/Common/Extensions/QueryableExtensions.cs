using Microsoft.EntityFrameworkCore;
using UniversityManagement.Application.Common.Models;

namespace UniversityManagement.Infrastructure.Common.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PaginatedResult<TSource>> ToPagedResultAsync<TSource>(
            this IQueryable<TSource> source,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var normalizedPageNumber = PaginationDefaults.NormalizePageNumber(pageNumber);
            var normalizedPageSize = PaginationDefaults.NormalizePageSize(pageSize);

            var totalCount = await source.CountAsync(cancellationToken);

            if (totalCount == 0)
            {
                return new PaginatedResult<TSource>(
                    new List<TSource>(),
                    normalizedPageNumber,
                    normalizedPageSize,
                    0,
                    0);
            }

            var items = await source
                .Skip((normalizedPageNumber - 1) * normalizedPageSize)
                .Take(normalizedPageSize)
                .ToListAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (double)normalizedPageSize);

            return new PaginatedResult<TSource>(
                items,
                normalizedPageNumber,
                normalizedPageSize,
                totalCount,
                totalPages);
        }
    }
}
