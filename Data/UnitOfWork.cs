using Microsoft.EntityFrameworkCore;
using Schedule.Common.Contracts.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.Data
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private bool _disposed = false;
        private readonly DbContext _dbContext;
        private Dictionary<Type, object> _repositoryDict = new Dictionary<Type, object>(); 

        public UnitOfWork(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<T> GetRepository<T>() where T : Entity, new()
        {
            var targetType = typeof(T);
            if (_repositoryDict.ContainsKey(targetType))
            {
                return _repositoryDict[targetType] as IRepository<T>;
            }

            var repo = new Repository<T>(_dbContext);
            _repositoryDict.Add(targetType, repo);
            return repo;
        }

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

