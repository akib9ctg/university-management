using System;
using System.Collections.Generic;
using UniversityManagement.Application.Classes.Queries.GetClasses;
using UniversityManagement.Application.Common.Models;
using UniversityManagement.Domain.Entities;
using UniversityManagement.Infrastructure.Common.Interfaces;

namespace UniversityManagement.Application.Classes.Interfaces
{
    public interface IClassRepository : IRepository<Class>
    {
        Task<PaginatedResult<Class>> GetPagedAsync(GetClassesRequest request, CancellationToken cancellationToken);
        Task<List<User>> GetStudentsByClassIdAsync(Guid classId, CancellationToken cancellationToken);
        Task<List<Course>> GetCoursesByClassIdAsync(Guid classId, CancellationToken cancellationToken);
    }
}
