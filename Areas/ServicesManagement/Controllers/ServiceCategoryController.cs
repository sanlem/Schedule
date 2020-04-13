using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Schedule.Areas.ServicesManagement.Contracts.Data;
using Schedule.Areas.ServicesManagement.Data;
using Schedule.Areas.ServicesManagement.Models;
using Schedule.Areas.ServicesManagement.Repositories;
using Schedule.Base;

namespace Schedule.Areas.ServicesManagement.Controllers
{
    [Area("ServicesManagement")]
    public class ServiceCategoryController :
        BaseCrudController<ServiceCategory, ServiceCategoryVM, 
            IServiceCategoryRepository>
    {
        public ServiceCategoryController(IServiceCategoryRepository repository, IMapper mapper,
            ILogger<ServiceCategoryController> logger, IStringLocalizer<ServiceCategoryController> localizer) 
            : base(repository, mapper, logger, localizer)
        {
        }

        protected override async Task<IEnumerable<ServiceCategoryVM>> getModelForListView()
        {
            var dbModels = await _repository.GetAll();
            var vm = _mapper.Map<IEnumerable<ServiceCategoryVM>>(dbModels);
            return vm;
        }
    }
}