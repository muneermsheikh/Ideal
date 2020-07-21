using API.Dtos;
using AutoMapper;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Admin;
using Core.Entities.Masters;
using Core.Entities.HR;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Category, CategoryToReturnDto>()
                .ForMember(x => x.IndustryType, o => o.MapFrom(s => s.IndustryType.Name))
                .ForMember(x => x.SkillLevel, o => o.MapFrom(s => s.SkillLevel.Name));
                // .ForMember(x => x.imageUrl, o => o.MapFrom<MachineryImageResolver>());
            
            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<BasketItem, EnquiryItem>();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<AddressDto, SiteAddress>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<Enquiry, EnquiryToReturnDto>();
                /* .ForMember(x => x.EnquiryStatus, o => o.MapFrom(s => s.enumEnquiryStatus.Value))
                .ForMember(x => x.ProjectManager, o => o.MapFrom(s => s.ProjectManager.Employee.Name))
                .ForMember(x => x.HRExecutive , o => o.MapFrom(s => s.CustomerOfficial.Name))
                .ForMember(x => x.LogisticsExecutive, o => o.MapFrom(s => s.CustomerOfficial.Name))
                .ForMember(x => x.AccountExecutive, o => o.MapFrom(s => s.CustomerOfficial.Name));
                */ 
            CreateMap<EnquiryItem, EnquiryItemDto>();
            CreateMap<Customer, CustomerDto>();
            CreateMap<ToDo, TaskToReturnDto>();
            CreateMap<TaskItem, TaskItemDto>();
            CreateMap<HRSkillClaim, HRSkillClaimsDto>();
            CreateMap<AssessmentQ, AssessmentItem>();
            CreateMap<Employee, EmployeeDto>()
                .ForMember(x => x.EmployeeName, o => o.MapFrom(s => s.Person.FullName))
                .ForMember(x => x.KnownAs, o => o.MapFrom(s => s.Person.KnownAs));
        }
    }
}