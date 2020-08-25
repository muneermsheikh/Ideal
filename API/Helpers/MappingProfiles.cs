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
            
            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<BasketItem, EnquiryItem>();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<CustomerAddress, CustomerAddressDto>();

            CreateMap<Enquiry, EnquiryToReturnDto>()
                .ForMember(x => x.EnquiryStatus, o => o.MapFrom(
                    s => Enum.GetName(typeof(enumEnquiryStatus), s.EnquiryStatus)));
                 
            CreateMap<EnquiryItem, EnquiryItemToReturnDto>()
                .ForMember(d => d.AssessingSup, o => o.MapFrom<EnquiryItemSupNameResolver>())
                .ForMember(d => d.AssessingHRM, o => o.MapFrom<EnquiryItemHRMNameResolver>());
            CreateMap<EnquiryItemToReturnDto, EnquiryItem>();
            CreateMap<EnquiryItem, EnquiryItemToEditDto>().ReverseMap();
            
            CreateMap<ContractReviewItem, ReviewItemDto>()
                .ForMember(d => d.CustomerName, o => o.MapFrom<ReviewItemCustomerNameResolver>())
                .ForMember(d => d.CategoryName, o => o.MapFrom<ReviewItemCategoryNameResolver>())
                .ForMember(d => d.EnquiryNoAndDate, o => o.MapFrom<ReviewItemEnquiryNoDateResolver>())
                .ForMember(d => d.Status, o => o.MapFrom(s => Enum.GetName(typeof(enumItemReviewStatus), s.Status)))
                .ForMember(d => d.ReviewedByName, o => o.MapFrom<ReviewItemReviewedByResolver>());

        // for CV Evaluation
            CreateMap<CVEvaluation, CVEvaluationDto>()
                .ForMember(d => d.Category, o => o.MapFrom<CVEvalCategoryResolver>())
                .ForMember(d => d.FullName, o => o.MapFrom<CVEvalCandidateNameResolver>())
                .ForMember(d => d.HRExecutive, o => o.MapFrom<CVEvalHRExecNameResolver>())
                .ForMember(d => d.HRManager, o => o.MapFrom<CVEvalHRManagerNameResolver>())
                .ForMember(d => d.HRSupervisor, o => o.MapFrom<CVEvalHRSupNameResolver>())
                .ForMember(d => d.HRMReviewResult, o => o.MapFrom(s => Enum.GetName(typeof(enumItemReviewStatus), s.HRMReviewResult)))
                .ForMember(d => d.HRSupReviewResult, o => o.MapFrom(s => Enum.GetName(typeof(enumItemReviewStatus), s.HRSupReviewResult)));

        // for DLForwardedToHRDto
            CreateMap<Customer, CustomerToReturnInBriefDto>();
            CreateMap<EnquiryItem, EnquiryForwardedItemInBriefDto>();
                  CreateMap<DLForwardToHR, EnquiryForwardedInBriefDto>()
                .ForMember(d => d.Customer, o => o.MapFrom<DLFwdToHRCustomerResolver>());
            CreateMap<DLForwardToHR, DLForwardedToHRDto>();
            
            CreateMap<EnquiryForwarded, EnquiryForwardedInBriefDto>()
                .ForMember(d => d.Customer, o => o.MapFrom<EnquiryForwardedCustomerResolver>());
            CreateMap<EnquiryItemForwarded, EnquiryForwardedItemInBriefDto>();
            CreateMap<Customer, CustomerInBriefDto>();
            CreateMap<EnquiryForwarded, EnquiryForwardedDto>()
                .ForMember(d => d.ForwardedToAssociate, o => o.MapFrom<EnquiryForwardedAssociateResolver>());
    
    //JOB DESC           
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
            CreateMap<IndustryType, IndustryToReturnDto>();
            CreateMap<SkillLevel, IndustryToReturnDto>();

            CreateMap<EmailModel, EmailDto>();

    // assessment
            CreateMap<AssessmentToAddDto, Assessment>()
                .ForMember(d => d.CustomerNameAndCity, o => o.MapFrom<AssessmentDtoCustomerNameCityResolver>())
                .ForMember(d => d.Enquiryitem, o => o.MapFrom<AssessmentDtoEnquiryItemResolver>())
                .ForMember(d => d.CategoryNameAndRef, o => o.MapFrom<AssessmentDtoCategoryNameRefResolver>()).ReverseMap();
            CreateMap<AssessmentItemToAddDto, AssessmentItem>();

    //candidate
            CreateMap<Category, CategoryNameDto>();
            CreateMap<Candidate, CandidateDto>()
                .ForMember(d => d.Age, o => o.MapFrom<CandidateDtoAgeResolver>())            
                .ForMember(d => d.City, o => o.MapFrom<CandidateDtoCityResolver>())
                .ForMember(d => d.CandidateStatus, 
                    o => o.MapFrom(s => Enum.GetName(typeof(enumCandidateStatus), s.CandidateStatus)));
            CreateMap<CandidateCategory, CategoryNameDto>()
                //.ForMember(d => d.Name, o => o.MapFrom<CategoryNameResolver>());
                .ForMember(d => d.Name, o => o.MapFrom<CategoryNameResolver>());
                //.ConstructUsing(ct => Map<CandidateCategory>(ct.CatId))
                //.ForAllMembers(opt => opt.Ignore());
                
                
            CreateMap<CandidateTempToAddDto, Candidate>();
       }
    }
}