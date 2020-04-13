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
    public class ServiceRepository : IServiceRepository
    {
        private readonly ApplicationDbContext _dbContext;
        
        public ServiceRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> Create(Service entity)
        {
            await _dbContext.Services.AddAsync(entity);
            return await save();
        }

        public async Task<bool> Delete(Service entity)
        {
            _dbContext.Services.Remove(entity);
            return await save();
        }

        public async Task<bool> Update(Service entity)
        {
            _dbContext.Services.Update(entity);
            return await save();
        }

        public async Task<IEnumerable<Service>> GetAll()
        {
            var collection = _dbContext.Services
                .Include(q => q.Category)
                .ToListAsync<Service>();
            return await collection;
        }

        public async Task<Service> GetById(int id)
        {
            var entity = await _dbContext.Services
                .Include(q => q.Category)
                .FirstOrDefaultAsync(q => q.Id == id);
            return entity;
        }

        private async Task<bool> save()
        {
            var changes = await _dbContext.SaveChangesAsync();
            return changes > 0;
        }

        public async Task<ICollection<Service>> GetServicesByCategory(int categoryId)
        {
            var services = await _dbContext.Services
                .Where(q => q.CategoryId == categoryId)
                .ToListAsync();
            return services;
        }

        public async Task<bool> ExistsWithId(int id)
        {
            var exists = await _dbContext.Services.AnyAsync(q => q.Id == id);
            return exists;
        }

       
    }
}
