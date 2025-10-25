using Microsoft.EntityFrameworkCore;
using UniversityManagement.Application.Common.Models;
using UniversityManagement.Application.Courses.Interfaces;
using UniversityManagement.Application.Courses.Queries.GetCourses;
using UniversityManagement.Domain.Entities;
using UniversityManagement.Domain.Enums;
using UniversityManagement.Infrastructure.Common.Extensions;
using UniversityManagement.Infrastructure.Database.Persistence;

namespace UniversityManagement.Infrastructure.Database.Repository
{
    public sealed class CourseRepository : Repository<Course>, ICourseRepository
    {
        private ApplicationDbContext _context;

        public CourseRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }

        public async Task<bool> ExistsByNameAsync(string name, CancellationToken cancellationToken)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(name);
            var normalizedName = name.Trim().ToLowerInvariant();

            return await _context.Courses
                .AsNoTracking()
                .AnyAsync(course =>
                    course.Name.ToLower().Equals(normalizedName), cancellationToken);
        }

        public Task<PaginatedResult<Course>> GetPagedAsync(GetCoursesRequest request, CancellationToken cancellationToken)
        {
            request ??= new GetCoursesRequest();

            var query = Queryable;
            query = ApplyFilters(query, request);
            query = ApplyOrdering(query, request);

            return query.ToPagedResultAsync(request.PageNumber, request.PageSize, cancellationToken);
        }

        public async Task<List<User>> GetStudentsByCourseIdAsync(Guid courseId, CancellationToken cancellationToken)
        {
            return await _context.UserCourses
                .AsNoTracking()
                .Where(uc => uc.CourseId == courseId && uc.User.Role == UserRole.Student)
                .Select(uc => uc.User)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Class>> GetClassesByCourseIdAsync(Guid courseId, CancellationToken cancellationToken)
        {
            return await _context.CourseClasses
                .AsNoTracking()
                .Where(cc => cc.CourseId == courseId)
                .Select(cc => cc.Class)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> AddClassToCourseAsync(Guid courseId, Guid classId, CancellationToken cancellationToken)
        {
            var existing = await _context.CourseClasses
                .FirstOrDefaultAsync(cc => cc.CourseId == courseId && cc.ClassId == classId, cancellationToken);

            if (existing is not null)
            {
                if (existing.IsDeleted)
                {
                    existing.IsDeleted = false;
                    existing.ModifiedAt = DateTime.UtcNow;
                    _context.CourseClasses.Update(existing);
                    await _context.SaveChangesAsync(cancellationToken);
                    return true;
                }

                return false;
            }

            var courseClass = new CourseClass
            {
                CourseId = courseId,
                ClassId = classId
            };

            await _context.CourseClasses.AddAsync(courseClass, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
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
