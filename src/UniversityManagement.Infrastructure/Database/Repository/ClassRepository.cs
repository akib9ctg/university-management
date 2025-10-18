using Microsoft.EntityFrameworkCore;
using UniversityManagement.Application.Classes.Interfaces;
using UniversityManagement.Application.Classes.Queries.GetClasses;
using UniversityManagement.Application.Common.Models;
using UniversityManagement.Domain.Entities;
using UniversityManagement.Domain.Enums;
using UniversityManagement.Infrastructure.Common.Extensions;
using UniversityManagement.Infrastructure.Database.Persistence;

namespace UniversityManagement.Infrastructure.Database.Repository
{
    public sealed class ClassRepository : Repository<Class>, IClassRepository
    {
        private ApplicationDbContext _context;

        public ClassRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public Task<PaginatedResult<Class>> GetPagedAsync(GetClassesRequest request, CancellationToken cancellationToken)
        {
            request ??= new GetClassesRequest();

            var query = Queryable;
            query = ApplyFilters(query, request);
            query = ApplyOrdering(query, request);

            return query.ToPagedResultAsync(request.PageNumber, request.PageSize, cancellationToken);
        }

        public async Task<List<User>> GetStudentsByClassIdAsync(Guid classId, CancellationToken cancellationToken)
        {
            return await _context.UserCourseClasses
                .AsNoTracking()
                .Where(ucc => ucc.ClassId == classId && !ucc.IsDeleted && !ucc.User.IsDeleted && ucc.User.Role == UserRole.Student)
                .Select(ucc => ucc.User)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Course>> GetCoursesByClassIdAsync(Guid classId, CancellationToken cancellationToken)
        {
            return await _context.CourseClasses
                .AsNoTracking()
                .Where(cc => cc.ClassId == classId && !cc.IsDeleted && !cc.Course.IsDeleted)
                .Select(cc => cc.Course)
                .ToListAsync(cancellationToken);
        }

        private static IQueryable<Class> ApplyFilters(IQueryable<Class> query, GetClassesRequest request)
        {
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var name = request.Name.Trim();
                query = query.Where(@class => EF.Functions.ILike(@class.Name, $"%{name}%"));
            }

            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                var description = request.Description.Trim();
                query = query.Where(@class => EF.Functions.ILike(@class.Description ?? string.Empty, $"%{description}%"));
            }

            return query;
        }

        private static IQueryable<Class> ApplyOrdering(IQueryable<Class> query, GetClassesRequest request)
        {
            var orderBy = string.IsNullOrWhiteSpace(request.OrderBy)
                ? GetClassesRequest.DefaultOrderBy
                : request.OrderBy.Trim();

            return orderBy.ToLowerInvariant() switch
            {
                "description" => request.SortDirection == SortDirection.Desc
                    ? query.OrderByDescending(@class => @class.Description)
                    : query.OrderBy(@class => @class.Description),
                "createdat" => request.SortDirection == SortDirection.Desc
                    ? query.OrderByDescending(@class => @class.CreatedAt)
                    : query.OrderBy(@class => @class.CreatedAt),
                "modifiedat" => request.SortDirection == SortDirection.Desc
                    ? query.OrderByDescending(@class => @class.ModifiedAt)
                    : query.OrderBy(@class => @class.ModifiedAt),
                _ => request.SortDirection == SortDirection.Desc
                    ? query.OrderByDescending(@class => @class.Name)
                    : query.OrderBy(@class => @class.Name),
            };
        }
    }
}
