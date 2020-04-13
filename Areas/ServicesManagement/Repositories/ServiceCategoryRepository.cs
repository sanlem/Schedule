using Microsoft.EntityFrameworkCore;
using Schedule.Areas.ServicesManagement.Contracts.Data;
using Schedule.Areas.ServicesManagement.Data;
using Schedule.Contracts.Data;
using Schedule.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.Areas.ServicesManagement.Repositories
{
    public class ServiceCategoryRepository : IServiceCategoryRepository
    {
        private readonly ApplicationDbContext _dbContext;
        public ServiceCategoryRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Create(ServiceCategory entity)
        {
            await _dbContext.ServiceCategories.AddAsync(entity);
            return await save();
        }

        public async Task<bool> Delete(ServiceCategory entity)
        {
            _dbContext.ServiceCategories.Remove(entity);
            return await save();
        }

        public async Task<bool> Update(ServiceCategory entity)
        {
            _dbContext.ServiceCategories.Update(entity);
            return await save();
        }

        public async Task<IEnumerable<ServiceCategory>> GetAll()
        {
            var collection = _dbContext.ServiceCategories
                .Include(q => q.ParentCategory)
                .ToListAsync<ServiceCategory>();
            return await collection;
        }

        public async Task<ServiceCategory> GetById(int id)
        {
            var entity = await _dbContext.ServiceCategories
                .Include(q => q.ParentCategory)
                .FirstOrDefaultAsync(q => q.Id == id);
            return entity;
        }

        private async Task<bool> save()
        {
            var changes = await _dbContext.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<bool> ExistsWithId(int id)
        {
            var exists = await _dbContext.ServiceCategories.AnyAsync(q => q.Id == id);
            return exists;
        }
    }
}
