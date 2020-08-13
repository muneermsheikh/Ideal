using API.Dtos;
using AutoMapper;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Admin;
using Core.Entities.Masters;
using Core.Entities.HR;
using Core.Enumerations;
using Core.Entities.Identity;
using System;
using Core.Entities.Emails;
using System.Collections.Generic;

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
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<CustomerAddress, CustomerAddressDto>();

            CreateMap<Enquiry, EnquiryToReturnDto>()
                .ForMember(x => x.EnquiryStatus, o => o.MapFrom(
                    s => Enum.GetName(typeof(enumEnquiryStatus), s.EnquiryStatus)));
               // .ForMember(x => x.ProjectManager, o => o.MapFrom(s => s.ProjectManager.KnownAs))
               // .ForMember(x => x.HRExecutive , o => o.MapFrom(s => s.HRExecutive.Name))
               // .ForMember(x => x.LogisticsExecutive, o => o.MapFrom(s => s.LogisticsExecutive.Name))
               // .ForMember(x => x.AccountExecutive, o => o.MapFrom(s => s.AccountExecutive.Name))
               // .ForMember(x => x.CustomerName, o => o.MapFrom(s => s.Customer.CustomerName));
                 
            CreateMap<EnquiryItem, EnquiryItemToReturnDto>();
            CreateMap<EnquiryItemToReturnDto, EnquiryItem>();
            CreateMap<EnquiryItem, EnquiryItemToEditDto>().ReverseMap();
            
            CreateMap<ContractReviewItem, ReviewItemDto>()
                .ForMember(d => d.CustomerName, o => o.MapFrom<ReviewItemCustomerNameResolver>())
                .ForMember(d => d.CategoryName, o => o.MapFrom<ReviewItemCategoryNameResolver>())
                .ForMember(d => d.EnquiryNoAndDate, o => o.MapFrom<ReviewItemEnquiryNoDateResolver>())
                .ForMember(d => d.Status, o => o.MapFrom(s => Enum.GetName(typeof(enumItemReviewStatus), s.Status)))
                .ForMember(d => d.ReviewedByName, o => o.MapFrom<ReviewItemReviewedByResolver>());

        // for DLForwardedToHRDto
            CreateMap<Customer, CustomerToReturnInBriefDto>();
            CreateMap<EnquiryItem, EnquiryItemInBriefDto>();
            //CreateMap<IReadOnlyList<Enquiry>, IReadOnlyList<EnquiryInBriefDto>>();
            CreateMap<Enquiry, EnquiryInBriefDto>();

            CreateMap<DLForwarded, DLForwardedToHRDto>();

            CreateMap<JobDesc, JobDescDto>();
            CreateMap<Remuneration, RemunerationDto>()
                .ForMember(x => x.Food, o => o.MapFrom(s => Enum.GetName(typeof(enumProvision), s.Food)))
                .ForMember(x => x.Housing, o => o.MapFrom(s => Enum.GetName(typeof(enumProvision), s.Housing)))
                .ForMember(x => x.Transport, o => o.MapFrom(s => Enum.GetName(typeof(enumProvision), s.Transport)));
            
            CreateMap<EnquiryItem, EnquiryItemAssignmentDto>()
                .ForMember(d => d.HRExecName, o => o.MapFrom(s => s.AssessingHRExec.KnownAs))
                .ForMember(d => d.HRSupName, o => o.MapFrom(s => s.AssessingSup.KnownAs))
                .ForMember(d => d.HRMName, o => o.MapFrom(s => s.AssessingHRM.KnownAs));
            
            CreateMap<Customer, CustomerToReturnDto>();
            CreateMap<Customer, CustomerDto>();
                //.ForMember(d => d.CustomerType, o => o.MapFrom(s => s.enumCustomerType.Value))
                //.ForMember(x => x.CustomerStatus, o => o.MapFrom(s => s.enumCustomerStatus.Value));
            
            CreateMap<CustomerOfficial, CustOfficialToReturnDto>();
            
            CreateMap<AddressDto, SiteAddress>();
            CreateMap<CustomerAddress, CustomerAddressDto>();
            CreateMap<CustomerAddress, Address>();
            CreateMap<CustomerOfficial, CustomerOfficialDto>();
            CreateMap<ToDo, TaskToReturnDto>();
            CreateMap<TaskItem, TaskItemDto>();
            CreateMap<HRSkillClaim, HRSkillClaimsDto>();
            CreateMap<AssessmentQ, AssessmentItem>();
            CreateMap<Employee, EmployeeToReturnDto>();
                // .ForMember(x => x.EmployeeName, o => o.MapFrom(s => s.Person.FullName))
                // .ForMember(x => x.KnownAs, o => o.MapFrom(s => s.Person.KnownAs));
            CreateMap<IndustryType, IndustryToReturnDto>();
            CreateMap<SkillLevel, IndustryToReturnDto>();

            CreateMap<EmailModel, EmailDto>();
        }
    }
}