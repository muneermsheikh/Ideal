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
using Core.Entities.Processing;

namespace API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

    // assessment
            CreateMap<Assessment, AssessmentDto>()
                .ForMember(d=>d.CandidateName, o=>o.MapFrom<AssessmentCandidateNameResolver>())
                .ForMember(d=>d.CategoryNameAndRef, o=>o.MapFrom<AssessmentCategoryResolver>())
                .ForMember(d=>d.CustomerNameAndCity, o=>o.MapFrom<AssessmentCustomerResolver>())
                .ForMember(d=>d.ApplicationNo, o=>o.MapFrom<AssessmentApplicationNoResolver>())
                .ForMember(d=>d.Result, o => o.MapFrom(s => 
                    Enum.GetName(typeof(enumAssessmentResult), s.Result)));
            CreateMap<AssessmentItem, AssessmentItemDto>();
                    
            CreateMap<AssessmentToAddDto, Assessment>()
                .ForMember(d => d.CustomerNameAndCity, o => o.MapFrom<AssessmentDtoCustomerNameCityResolver>())
                .ForMember(d => d.Enquiryitem, o => o.MapFrom<AssessmentDtoEnquiryItemResolver>())
                .ForMember(d => d.CategoryNameAndRef, o => o.MapFrom<AssessmentDtoCategoryNameRefResolver>()).ReverseMap();
            CreateMap<AssessmentItemToAddDto, AssessmentItem>();

            CreateMap<AssessmentQ, AssessmentQItemDto>();   //items
            //CreateMap<AssessmentQ, AssessmentQDto>();   //items
            CreateMap<AssessmentQBankToAddDto, AssessmentQBank>().ReverseMap();
            CreateMap<AssessmentQBank, AssessmentQBankToAddDto>()
                .ForMember(d => d.CategoryRef, o => o.MapFrom<AssessmentQBankToAddDtoCategoryNameResolver>());
            
//category
            CreateMap<Category, CategoryToReturnDto>()
                .ForMember(x => x.IndustryType, o => o.MapFrom(s => s.IndustryType.Name))
                .ForMember(x => x.SkillLevel, o => o.MapFrom(s => s.SkillLevel.Name));
            
            CreateMap<Core.Entities.Identity.Address, AddressDto>().ReverseMap();
            CreateMap<BasketItem, EnquiryItem>();
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();
            CreateMap<CustomerAddress, CustomerAddressDto>();

//enquiry
            CreateMap<Enquiry, EnquiryToReturnDto>()
                .ForMember(x => x.EnquiryStatus, o => o.MapFrom(
                    s => Enum.GetName(typeof(enumEnquiryStatus), s.EnquiryStatus)));
                 
            CreateMap<EnquiryItem, EnquiryItemToReturnDto>()
                .ForMember(d => d.AssessingSup, o => o.MapFrom<EnquiryItemSupNameResolver>())
                .ForMember(d => d.AssessingHRM, o => o.MapFrom<EnquiryItemHRMNameResolver>())
                .ForMember(d => d.Category, o => o.MapFrom<EnquiryItemCategoryResolver>());
            CreateMap<EnquiryItemToReturnDto, EnquiryItem>();
            CreateMap<EnquiryItem, EnquiryItemToEditDto>().ReverseMap();
            
            CreateMap<ContractReviewItem, ReviewItemDto>()
                .ForMember(d => d.CustomerName, o => o.MapFrom<ReviewItemCustomerNameResolver>())
                .ForMember(d => d.CategoryName, o => o.MapFrom<ReviewItemCategoryNameResolver>())
                .ForMember(d => d.EnquiryNoAndDate, o => o.MapFrom<ReviewItemEnquiryNoDateResolver>())
                .ForMember(d => d.StatusString, o => o.MapFrom(s => Enum.GetName(typeof(enumItemReviewStatus), s.Status)))
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
            CreateMap<CustomerOfficial, CustomerOfficialDto>()
                .ForMember(d => d.CustomerName, o => o.MapFrom<CustomerOfficialCustomerNameResolver>());
            
            CreateMap<AddressDto, SiteAddress>();
            CreateMap<CustomerAddress, CustomerAddressDto>();
            CreateMap<CustomerAddress, Address>();
            CreateMap<CustomerOfficial, CustomerOfficialDto>();
            CreateMap<ToDo, TaskToReturnDto>();
            CreateMap<TaskItem, TaskItemDto>();
            CreateMap<HRSkillClaim, HRSkillClaimsDto>();
            //CreateMap<AssessmentQ, AssessmentItem>();
            CreateMap<Employee, EmployeeToReturnDto>();
            CreateMap<IndustryType, IndustryToReturnDto>();
            CreateMap<SkillLevel, IndustryToReturnDto>();

            CreateMap<EmailModel, EmailDto>();

    //candidate
            CreateMap<Category, CategoryNameDto>();

            CreateMap<Candidate, CandidateDto>()
                .ForMember(d => d.Age, o => o.MapFrom<CandidateDtoAgeResolver>())            
                .ForMember(d => d.City, o => o.MapFrom<CandidateDtoCityResolver>())
                .ForMember(d => d.CandidateStatus, 
                    o => o.MapFrom(s => Enum.GetName(typeof(enumCandidateStatus), s.CandidateStatus)));
                    
            CreateMap<CandidateCategory, CategoryNameDto>()
                .ForMember(d => d.Name, o => o.MapFrom<CategoryNameResolver>());
               
                
            CreateMap<CandidateTempToAddDto, Candidate>();

    //candidatecategory
            CreateMap<CandidateCategory, CandidateCategoryDto>()
                .ForMember(d => d.CandidateName, o=>o.MapFrom<CandidateCategoryCandidateNameResolver>())
                .ForMember(d => d.CategoryName, o => o.MapFrom<CandidateCategoryCategoryNameResolver>());
            
    //cvref
            CreateMap<CVRef, CVRefHdrDto>()
                .ForMember(d => d.customername, o => o.MapFrom<CVRefCustomerNameResolver>())
                .ForMember(d => d.customercity, o => o.MapFrom<CVRefCustomerCityResolver>());
            CreateMap<CVRef, CVRefItemDto>()
                .ForMember(d => d.candidatename, o => o.MapFrom<CVRefCandidateNameResolver>())
                .ForMember(d => d.categoryref, o => o.MapFrom<CVRefCategoryRefResolver>())
                .ForMember(d => d.ppno, o => o.MapFrom<CVRefPPNoResolver>());
            
            CreateMap<CVRef, CVRefDto>()
                .ForMember(d => d.CandidateNameWithAppNo, o => o.MapFrom<CVRefToCVRefDtoCandidateNameResolver>())
                .ForMember(d => d.CategoryRef, o => o.MapFrom<CVRefToCVRefDtoCategoryRefResolver>())
                .ForMember(d => d.CustomerNameAndCity, o => o.MapFrom<CVRefToCVRefDtoCustomerNameResolver>())
                .ForMember(d => d.DateForwarded, o => o.MapFrom(s => s.DateForwarded.Date));
            CreateMap<Process, ProcessDto>();
            

            CreateMap<CVRef, CandidateHistoryDto>()
                .ForMember(d => d.CandidateName, o => o.MapFrom<CVRefToHistoryCandidateNameResolver>())
                .ForMember(d => d.CategoryRef, o => o.MapFrom<CVRefToHistoryCategoryRefResolver>())
                .ForMember(d => d.CustomerName, o => o.MapFrom<CVRefToHistoryCustomerNameResolver>())
                .ForMember(d => d.DateReferred, o => o.MapFrom(s => s.DateForwarded.Date.ToString()))
                .ForMember(d => d.CurrentStatus, o => o.MapFrom(s => s.DateForwarded.Date));
                
            CreateMap<Process, HistoryItemDto>();
     
            CreateMap<EnquiryItem, CVRefDto>()
                .ForMember(d => d.CategoryRef, o => o.MapFrom<EnquiryItemToCVRefDtoCategoryRefResolver>())
                .ForMember(d => d.CustomerNameAndCity, o => o.MapFrom<EnquiryItemToCVRefDtoCustomerNameResolver>());

    //cv fwd
            CreateMap<CVForward, CVForwardDto>()
                .ForMember(d => d.CustomerName, o => o.MapFrom<CVForwardDtoCustomerNameResolver>())
                .ForMember(d => d.OfficialName, o => o.MapFrom<CVForwardOfficialNameResolver>());

            CreateMap<CVForwardItem, CVForwardItemDto>()
                .ForMember(d => d.ApplicationNo, o => o.MapFrom<CVForwardItemDtoAppNoResolver>())
                .ForMember(d => d.CandidateName, o => o.MapFrom<CVForwardItemDtoCandidateNameResolver>())
                .ForMember(d => d.CategoryRef, o => o.MapFrom<CVForwardItemDtoCategoryRefResolver>());

    //process
            CreateMap<Process, ProcessAddedDto>()
                .ForMember(d => d.CandidateName, o => o.MapFrom<ProcessToDtoAppNoResolver>())
                .ForMember(d => d.CustomerName, o => o.MapFrom<ProcessToDtoCompanyNameResolver>())
                .ForMember(d => d.CategoryRef, o => o.MapFrom<ProcessToDtoCandidateNameResolver>())
                .ForMember(d => d.DateOfStatus, o => o.MapFrom(o => o.ProcessingDate.ToString()))
                .ForMember(d => d.StatusInserted, o => o.MapFrom(s => Enum.GetName(typeof(enumProcessingStatus), s.Status)))
                .ForMember(d => d.NextStatusName, o => o.MapFrom(s => Enum.GetName(typeof(enumProcessingStatus), s.NextProcessingId)));
            
            CreateMap<CVRef, ProcessDto>()
                .ForMember(d => d.ApplicationNo, o => o.MapFrom<CVRefToProcessDtoAppNoResolver>())
                .ForMember(d => d.PPNo, o => o.MapFrom<CVRefToProcessDtoPPNoResolver>())
                .ForMember(d => d.CandidateName, o => o.MapFrom<CVRefToProcessDtoCandidateNameResolver>());
            
            CreateMap<Process, ProcessReferredDto>()
                .ForMember(d => d.CustomerName , o => o.MapFrom<ProcessToReferredDtoCustomerNameResolver>())
                .ForMember(d => d.CategoryRef, o => o.MapFrom<ProcessToReferredDtoCategoryRefResolver>())
                .ForMember(d => d.DateReferred, o => o.MapFrom<ProcessToReferredDtoDateReferredResolver>())
                .ForMember(d => d.RefStatusDate, o => o.MapFrom<ProcessToReferredDtoRefStatusDateResolver>())
                .ForMember(d => d.CurrentStatus, o => o.MapFrom<ProcessToReferredDtoCurrentStatusResolver>());
            
            CreateMap<Process, ProcessItemDto>()
                .ForMember(d => d.Status, o => o.MapFrom(s => Enum.GetName(typeof(enumProcessingStatus), s.Status)));           
    
       }
    }
}