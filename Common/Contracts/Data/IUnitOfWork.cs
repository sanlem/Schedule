using Schedule.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.Common.Contracts.Data
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T : Entity, new();
        Task<int> SaveChangesAsync();
        int SaveChanges();
    }
}
