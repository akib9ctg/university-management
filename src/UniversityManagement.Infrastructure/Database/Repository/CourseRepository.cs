using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniversityManagement.Application.Common.Models;
using UniversityManagement.Application.Courses.Interfaces;
using UniversityManagement.Application.Courses.Queries.GetCourses;
using UniversityManagement.Domain.Entities;
using UniversityManagement.Infrastructure.Common.Extensions;
using UniversityManagement.Infrastructure.Database.Persistence;

namespace UniversityManagement.Infrastructure.Database.Repository
{
    public sealed class CourseRepository : Repository<Course>, ICourseRepository
    {
        public CourseRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public Task<PaginatedResult<Course>> GetPagedAsync(GetCoursesRequest request, CancellationToken cancellationToken)
        {
            request ??= new GetCoursesRequest();

            var query = Queryable;
            query = ApplyFilters(query, request);
            query = ApplyOrdering(query, request);

            return query.ToPagedResultAsync(request.PageNumber, request.PageSize, cancellationToken);
        }

        private static IQueryable<Course> ApplyFilters(IQueryable<Course> query, GetCoursesRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var name = request.Name.Trim();
                query = query.Where(course => EF.Functions.ILike(course.Name, $"%{name}%"));
            }

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                var description = request.Description.Trim();
                query = query.Where(course => EF.Functions.ILike(course.Description ?? string.Empty, $"%{description}%"));
            }

            return query;
        }

        private static IQueryable<Course> ApplyOrdering(IQueryable<Course> query, GetCoursesRequest request)
        {
            var orderBy = string.IsNullOrWhiteSpace(request.OrderBy)
                ? GetCoursesRequest.DefaultOrderBy
                : request.OrderBy.Trim();

            return orderBy.ToLowerInvariant() switch
            {
                "description" => request.SortDirection == SortDirection.Desc
                    ? query.OrderByDescending(course => course.Description)
                    : query.OrderBy(course => course.Description),
                "createdat" => request.SortDirection == SortDirection.Desc
                    ? query.OrderByDescending(course => course.CreatedAt)
                    : query.OrderBy(course => course.CreatedAt),
                "modifiedat" => request.SortDirection == SortDirection.Desc
                    ? query.OrderByDescending(course => course.ModifiedAt)
                    : query.OrderBy(course => course.ModifiedAt),
                _ => request.SortDirection == SortDirection.Desc
                    ? query.OrderByDescending(course => course.Name)
                    : query.OrderBy(course => course.Name),
            };
        }
    }
}
