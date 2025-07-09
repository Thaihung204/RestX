using AutoMapper;
using RestX.WebApp.Models;
using RestX.WebApp.Models.ViewModels;
using System.Linq;

namespace RestX.WebApp.Services.Mappings
{
    public class StaffMappingProfile : Profile
    {
        public StaffMappingProfile()
        {
           
            CreateMap<Staff, StaffViewModel>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Accounts.FirstOrDefault().Username));

       
            CreateMap<StaffRequest, Staff>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) 
                .ForMember(dest => dest.OwnerId, opt => opt.Ignore()); 
          
            CreateMap<StaffRequest, Account>()
                .ForMember(dest => dest.Password, opt => opt.Ignore());
        }
    }
}