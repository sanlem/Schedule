using AutoMapper;
using Schedule.Areas.ServicesManagement.Data;
using Schedule.Areas.ServicesManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.Mappings
{
    public class ModelsMapProfile: Profile
    {
        public ModelsMapProfile()
        {
            CreateMap<Service, ServiceVM>().ReverseMap();
            CreateMap<ServiceCategory, ServiceCategoryVM>().ReverseMap();
        }
    }
}
