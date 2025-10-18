using UniversityManagement.Application.Common.Models;
using UniversityManagement.Application.Courses.Queries.GetCourses;
using UniversityManagement.Domain.Entities;
using UniversityManagement.Infrastructure.Common.Interfaces;

namespace UniversityManagement.Application.Courses.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        Task<PaginatedResult<Course>> GetPagedAsync(GetCoursesRequest request, CancellationToken cancellationToken);
        Task<List<User>> GetStudentsByCourseIdAsync(Guid courseId, CancellationToken cancellationToken);
        Task<List<Class>> GetClassesByCourseIdAsync(Guid courseId, CancellationToken cancellationToken);
        Task<bool> AddClassToCourseAsync(Guid courseId, Guid classId, CancellationToken cancellationToken);
    }
}
