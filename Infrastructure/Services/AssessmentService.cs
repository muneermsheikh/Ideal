using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Entities.HR;
using Core.Entities.Masters;
using Core.Enumerations;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;

namespace Infrastructure.Services
{
    public class AssessmentService : IAssessmentService
    {
        private readonly ITaskService _taskService;
        private readonly IGenericRepository<Assessment> _assessRepo;
        private readonly ATSContext _context;
        public AssessmentService(IGenericRepository<Assessment> assessRepo, ATSContext context, ITaskService taskService)
        {
            _context = context;
            _assessRepo = assessRepo;
            _taskService = taskService;
        }

        public async Task<Assessment> AddAssessment(Assessment assessment)
        {
            return await _assessRepo.AddAsync(assessment);
        }

        public async Task<int> DeleteAssessment(Assessment assessment)
        {
            return await _assessRepo.DeleteAsync(assessment);
        }

        public async Task<IReadOnlyList<Assessment>> GetAssessmentBySpec(AssessmentParam assessmentParam)
        {
            return await _assessRepo.GetEntityListWithSpec(new AssessmentSpec(assessmentParam));
        }
    }
}