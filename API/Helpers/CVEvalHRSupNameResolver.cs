using API.Dtos;
using AutoMapper;
using Core.Entities.HR;
using Core.Interfaces;

namespace API.Helpers
{
    public class CVEvalHRSupNameResolver : IValueResolver<CVEvaluation, CVEvaluationDto, string>
    {
        private readonly IEmployeeService _empService;
        public CVEvalHRSupNameResolver(IEmployeeService empService)
        {
            _empService = empService;
        }

        public string Resolve(CVEvaluation source, CVEvaluationDto destination, string destMember, ResolutionContext context)
        {
            if(source.HRSupervisorId==0) return "invalid HR Supervisor Id";
            var cat = _empService.GetEmployeeName((int)source.HRSupervisorId);
            if(string.IsNullOrEmpty(cat)) return "Invalid HR Supervisor Id";
            return cat;
        }
    }
}