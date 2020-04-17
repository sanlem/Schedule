using Schedule.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.Common.Contracts.Data
{
    public interface IRepository<T> where T: Entity, new()
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<bool> ExistsWithIdAsync(int id);

        IEnumerable<T> GetAll();
        T GetById(int id);
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        bool ExistsWithId(int id);
    }
}
