using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Schedule.Areas.ServicesManagement.Data;
using Schedule.Areas.ServicesManagement.Models;
using Schedule.Common.Contracts;
using Schedule.Common.Controllers;

namespace Schedule.Areas.ServicesManagement.Controllers
{
    [Area("ServicesManagement")]
    public class ServiceCategoryController :
        BaseCrudController<ServiceCategory, ServiceCategoryVM>
    {
        public ServiceCategoryController(ICRUDService<ServiceCategory> crudService, IMapper mapper,
            ILogger<ServiceCategoryController> logger, IStringLocalizer<ServiceCategoryController> localizer) 
            : base(crudService, mapper, logger, localizer)
        {
        }

        protected override async Task<IEnumerable<ServiceCategoryVM>> GetModelForListView()
        {
            var dbModels = await _crudService.GetAllAsync();
            var vm = _mapper.Map<IEnumerable<ServiceCategoryVM>>(dbModels);
            return vm;
        }
    }
}