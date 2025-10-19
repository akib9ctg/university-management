using Microsoft.EntityFrameworkCore;
using UniversityManagement.Infrastructure.Database.Persistence;

namespace UniversityManagement.UnitTests
{
    public static class TestDbContextFactory
    {
        public static ApplicationDbContext Create()
        {
            var opts = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;
            return new ApplicationDbContext(opts, null);
        }
    }
}
