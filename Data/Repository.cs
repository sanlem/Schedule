using Microsoft.EntityFrameworkCore;
using Schedule.Common.Contracts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.Data
{
    public class Repository<T> : IRepository<T> where T : Entity, new()
    {
        protected readonly DbSet<T> DbSet;
        protected readonly DbContext DbCtx;

        public Repository(DbContext dbContext)
        {
            DbCtx = dbContext;
            DbSet = dbContext.Set<T>();
        }

        public bool Create(T entity)
        {
            if (entity is null)
            {
                return false;
            }

            DbSet.Add(entity);
            return true;
        }

        public async Task<bool> CreateAsync(T entity)
        {
            if (entity is null)
            {
                return false;
            }

            await DbSet.AddAsync(entity);
            return true;
        }

        public bool Delete(T entity)
        {
            if (entity is null || ! ExistsWithId(entity.Id))
            {
                return false;
            }

            DbSet.Remove(entity);
            return true;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            if (entity is null || ! await ExistsWithIdAsync(entity.Id))
            {
                return false;
            }

            DbSet.Remove(entity);
            return true;
        }

        public bool ExistsWithId(int id)
        {
            return DbSet.Any(o => o.Id == id);
        }

        public async Task<bool> ExistsWithIdAsync(int id)
        {
            return await DbSet.AnyAsync(o => o.Id == id);
        }

        public IEnumerable<T> GetAll()
        {
            return DbSet.ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public T GetById(int id)
        {
            return DbSet.Find(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await DbSet.FindAsync(id);
        }

        public bool Update(T entity)
        {
            if (entity is null || !ExistsWithId(entity.Id))
            {
                return false;
            }

            DbSet.Update(entity);
            return true;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            if (entity is null || !await ExistsWithIdAsync(entity.Id))
            {
                return false;
            }

            DbSet.Update(entity);
            return true;
        }
    }
}
