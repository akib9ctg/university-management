using Microsoft.EntityFrameworkCore;
using UniversityManagement.Application.Common.Models;
using UniversityManagement.Application.Students.Queries.GetStudents;
using UniversityManagement.Application.Users.Interfaces;
using UniversityManagement.Domain.Entities;
using UniversityManagement.Domain.Enums;
using UniversityManagement.Infrastructure.Common.Extensions;
using UniversityManagement.Infrastructure.Database.Persistence;

namespace UniversityManagement.Infrastructure.Database.Repository
{
    public sealed class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return Queryable.FirstOrDefaultAsync(user => user.Email == email, cancellationToken);
        }

        public Task<PaginatedResult<User>> GetStudentsPagedAsync(GetStudentsRequest request, CancellationToken cancellationToken)
        {
            request ??= new GetStudentsRequest();

            var query = Queryable.Where(user => user.Role == UserRole.Student);

            if (!string.IsNullOrWhiteSpace(request.FirstName))
            {
                var firstName = request.FirstName.Trim();
                query = query.Where(user => EF.Functions.ILike(user.FirstName, $"%{firstName}%"));
            }

            if (!string.IsNullOrWhiteSpace(request.LastName))
            {
                var lastName = request.LastName.Trim();
                query = query.Where(user => EF.Functions.ILike(user.LastName, $"%{lastName}%"));
            }

            if (!string.IsNullOrWhiteSpace(request.Email))
            {
                var email = request.Email.Trim();
                query = query.Where(user => EF.Functions.ILike(user.Email, $"%{email}%"));
            }

            query = ApplyOrdering(query, request);

            return query.ToPagedResultAsync(request.PageNumber, request.PageSize, cancellationToken);
        }

        private static IQueryable<User> ApplyOrdering(IQueryable<User> query, GetStudentsRequest request)
        {
            var orderBy = string.IsNullOrWhiteSpace(request.OrderBy)
                ? GetStudentsRequest.DefaultOrderBy
                : request.OrderBy.Trim();

            return orderBy.ToLowerInvariant() switch
            {
                "lastname" => request.SortDirection == SortDirection.Desc
                    ? query.OrderByDescending(user => user.LastName)
                    : query.OrderBy(user => user.LastName),
                "email" => request.SortDirection == SortDirection.Desc
                    ? query.OrderByDescending(user => user.Email)
                    : query.OrderBy(user => user.Email),
                "createdat" => request.SortDirection == SortDirection.Desc
                    ? query.OrderByDescending(user => user.CreatedAt)
                    : query.OrderBy(user => user.CreatedAt),
                "modifiedat" => request.SortDirection == SortDirection.Desc
                    ? query.OrderByDescending(user => user.ModifiedAt)
                    : query.OrderBy(user => user.ModifiedAt),
                _ => request.SortDirection == SortDirection.Desc
                    ? query.OrderByDescending(user => user.FirstName)
                    : query.OrderBy(user => user.FirstName),
            };
        }
    }
}
