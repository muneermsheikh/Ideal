using System.Collections.Generic;
using System.Threading.Tasks;
using API.Dtos;
using API.Errors;
using API.Extensions;
using API.Helpers;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Entities.HR;
using Core.Entities.Identity;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class InternalHRController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;
        private readonly IGenericRepository<HRSkillClaim> _skillRepo;
        private readonly IInternalHRService _internalHrService;
        private readonly IAssessmentService _cvEvalService;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHRService _hrService;

        public InternalHRController(
            IUnitOfWork unitOfWork,
            UserManager<AppUser> userManager,
            IGenericRepository<HRSkillClaim> skillRepo,
            IMapper mapper,
            IInternalHRService internalHrService,
            IAssessmentService cvEvalService,
            IHRService hrService
        )
        {
            _hrService = hrService;
            _cvEvalService = cvEvalService;
            _internalHrService = internalHrService;
            _skillRepo = skillRepo;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }


    }
}
