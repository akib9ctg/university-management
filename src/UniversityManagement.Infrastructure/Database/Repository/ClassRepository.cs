using UniversityManagement.Application.Classes.Interfaces;
using UniversityManagement.Domain.Entities;
using UniversityManagement.Infrastructure.Database.Persistence;

namespace UniversityManagement.Infrastructure.Database.Repository
{
    public sealed class ClassRepository : Repository<Class>, IClassRepository
    {
        private readonly ApplicationDbContext _context;

        public ClassRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _context = dbContext;
        }
    }
}
