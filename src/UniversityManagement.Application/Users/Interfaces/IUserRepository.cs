using UniversityManagement.Application.Common.Models;
using UniversityManagement.Application.Students.Queries.GetStudents;
using UniversityManagement.Domain.Entities;
using UniversityManagement.Infrastructure.Common.Interfaces;

namespace UniversityManagement.Application.Users.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken);
        Task<PaginatedResult<User>> GetStudentsPagedAsync(GetStudentsRequest request, CancellationToken cancellationToken);
    }
}
