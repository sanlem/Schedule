using Schedule.Common.Contracts.Data;
using Schedule.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.Common.Contracts
{
    public interface ICRUDService<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
    }
}
