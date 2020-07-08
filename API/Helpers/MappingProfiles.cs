using API.Dtos;
using AutoMapper;
using Core.Entities.Identity;
using Core.Entities.Masters;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Category, CategoryToReturnDto>()
                .ForMember(x => x.IndustryType, o => o.MapFrom(s => s.IndustryType.Name))
                .ForMember(x => x.SkillLevel, o => o.MapFrom(s => s.SkillLevel.Name))
                .ForMember(x => x.imageUrl, o => o.MapFrom<MachineryImageResolver>());
            
            CreateMap<Address, AddressDto>().ReverseMap();
        }
    }
}