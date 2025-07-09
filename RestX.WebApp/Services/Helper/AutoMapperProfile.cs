using AutoMapper;
using RestX.WebApp.Models;
using RestX.WebApp.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestX.WebApp.Services.Helper
{
    internal class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Dish, DishViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : ""))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.File != null ? src.File.Url : ""));
            CreateMap<DishRequest, Dish>().ReverseMap();
            // CreateMap<Source, Destination>();
            // CreateMap<Destination, Source>();
            // CreateMap<Source, Destination>().ReverseMap();
        }
    }
}
