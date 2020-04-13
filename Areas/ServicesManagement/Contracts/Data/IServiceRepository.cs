using Schedule.Areas.ServicesManagement.Data;
using Schedule.Contracts.Data;
using Schedule.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.Areas.ServicesManagement.Contracts.Data
{
    public interface IServiceRepository: IRepository<Service>
    {
        Task<ICollection<Service>> GetServicesByCategory(int categoryId);
    }
}
