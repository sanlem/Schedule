using Microsoft.Extensions.Logging;
using Schedule.Common.Contracts;
using Schedule.Common.Contracts.Data;
using Schedule.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.Common.Services
{
    public class CRUDService<TEntity>: ICRUDService<TEntity> where TEntity: Entity, new()
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger _logger;
        public CRUDService(IUnitOfWork uow, ILogger<CRUDService<TEntity>> logger)
        {
            if (uow is null)
            {
                throw new ArgumentNullException("uow can not be null!");
            }

            _uow = uow;
            _logger = logger;
        }

        public bool Create(TEntity entity)
        {
            _uow.GetRepository<TEntity>().Create(entity);
            return _uow.SaveChanges() > 0;
        }

        public async Task<bool> CreateAsync(TEntity entity)
        {
            await _uow.GetRepository<TEntity>().CreateAsync(entity);
            return await _uow.SaveChangesAsync() > 0;
        }

        public bool Delete(TEntity entity)
        {
            var addedToCtx = _uow.GetRepository<TEntity>().Delete(entity);
            if (!addedToCtx)
            {
                return false;
            }

            return _uow.SaveChanges() > 0;
        }

        public async Task<bool> DeleteAsync(TEntity entity)
        {
            var addedToCtx = await _uow.GetRepository<TEntity>().DeleteAsync(entity);
            if (!addedToCtx)
            {
                return false;
            }

            return await _uow.SaveChangesAsync() > 0;
        }

        public bool Update(TEntity entity)
        {
            var addedToCtx = _uow.GetRepository<TEntity>().Update(entity);
            if (!addedToCtx)
            {
                return false;
            }

            return _uow.SaveChanges() > 0;
        }

        public async Task<bool> UpdateAsync(TEntity entity)
        {
            var addedToCtx = await _uow.GetRepository<TEntity>().UpdateAsync(entity);
            if (!addedToCtx)
            {
                return false;
            }

            return await _uow.SaveChangesAsync() > 0;
        }

        public bool ExistsWithId(int id)
        {
            return _uow.GetRepository<TEntity>().ExistsWithId(id);
        }

        public async Task<bool> ExistsWithIdAsync(int id)
        {
            return await _uow.GetRepository<TEntity>().ExistsWithIdAsync(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _uow.GetRepository<TEntity>().GetAll();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _uow.GetRepository<TEntity>().GetAllAsync();
        }

        public TEntity GetById(int id)
        {
            return _uow.GetRepository<TEntity>().GetById(id);
        }

        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _uow.GetRepository<TEntity>().GetByIdAsync(id);
        }
    }
}
