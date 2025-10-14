using System;
using System.Collections.Generic;
using System.Linq;
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
        private ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
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

        public async Task EnrollStudentInClassAsync(Guid studentId, Guid classId, Guid? assignedByUserId, CancellationToken cancellationToken)
        {
            var courseClass = await _context.CourseClasses
                .AsNoTracking()
                .FirstOrDefaultAsync(cc => cc.ClassId == classId, cancellationToken);

            if (courseClass is null)
            {
                throw new InvalidOperationException($"Class with Id {classId} is not associated with any course.");
            }

            await EnsureUserCourseAsync(studentId, courseClass.CourseId, assignedByUserId, cancellationToken);

            var isAlreadyEnrolled = await _context.UserCourseClasses
                .AnyAsync(ucc => ucc.UserId == studentId && ucc.ClassId == classId, cancellationToken);

            if (!isAlreadyEnrolled)
            {
                var userCourseClass = new UserCourseClass
                {
                    UserId = studentId,
                    ClassId = classId,
                    CourseId = courseClass.CourseId,
                    AssignedByUserId = assignedByUserId,
                    AssignedAt = DateTime.UtcNow
                };

                await _context.UserCourseClasses.AddAsync(userCourseClass, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task EnrollStudentInCourseAsync(Guid studentId, Guid courseId, Guid? assignedByUserId, CancellationToken cancellationToken)
        {
            await EnsureUserCourseAsync(studentId, courseId, assignedByUserId, cancellationToken);

            var classIds = await _context.CourseClasses
                .Where(cc => cc.CourseId == courseId)
                .Select(cc => cc.ClassId)
                .ToListAsync(cancellationToken);

            if (classIds.Count == 0)
            {
                await _context.SaveChangesAsync(cancellationToken);
                return;
            }

            var existingClassIds = await _context.UserCourseClasses
                .Where(ucc => ucc.UserId == studentId && classIds.Contains(ucc.ClassId))
                .Select(ucc => ucc.ClassId)
                .ToListAsync(cancellationToken);

            var missingClassIds = new HashSet<Guid>(classIds);
            missingClassIds.ExceptWith(existingClassIds);

            if (missingClassIds.Count > 0)
            {
                var assignments = missingClassIds.Select(classId => new UserCourseClass
                {
                    UserId = studentId,
                    CourseId = courseId,
                    ClassId = classId,
                    AssignedByUserId = assignedByUserId,
                    AssignedAt = DateTime.UtcNow
                });

                await _context.UserCourseClasses.AddRangeAsync(assignments, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task EnsureUserCourseAsync(Guid studentId, Guid courseId, Guid? assignedByUserId, CancellationToken cancellationToken)
        {
            var userCourse = await _context.UserCourses
                .FirstOrDefaultAsync(uc => uc.UserId == studentId && uc.CourseId == courseId, cancellationToken);

            if (userCourse is null)
            {
                userCourse = new UserCourse
                {
                    UserId = studentId,
                    CourseId = courseId,
                    AssignedByUserId = assignedByUserId
                };

                await _context.UserCourses.AddAsync(userCourse, cancellationToken);
            }
            else if (assignedByUserId.HasValue && userCourse.AssignedByUserId is null)
            {
                userCourse.AssignedByUserId = assignedByUserId;
                _context.UserCourses.Update(userCourse);
            }
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
