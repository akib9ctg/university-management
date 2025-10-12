using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversityManagement.Domain.Common;
using UniversityManagement.Infrastructure.Common.Interfaces;

namespace UniversityManagement.Infrastructure.Database.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly DbContext _dbContext;
        public Repository(DbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await _dbContext.AddAsync(entity);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            entity.IsDeleted = true;
            entity.ModifiedAt = DateTime.UtcNow;  
            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _dbContext.Set<TEntity>()
                                .AsNoTracking()
                                .Where(e => !e.IsDeleted)
                                .ToListAsync(cancellationToken);
        }

        public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _dbContext.Set<TEntity>()
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public async Task UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            entity.ModifiedAt = DateTime.UtcNow;
            _dbContext.Entry(entity).State = EntityState.Modified;
            
            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
