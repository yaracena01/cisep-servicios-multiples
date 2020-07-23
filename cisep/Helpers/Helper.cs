using AutoMapper;
using cisep.Models;
using cisep.ViewModel;
using cisep.ViewModels.ServicesViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisep.Helpers
{
    public class Helper : Profile
    {
        public Helper()
        {
            CreateMap<Services, ServicesViewModel>().ReverseMap();
            CreateMap<ServicesViewModelCreate, Services>();
        }
    }
}
