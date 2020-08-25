using API.Dtos;
using AutoMapper;
using Core.Entities.HR;
using Core.Interfaces;

namespace API.Helpers
{
    public class CVEvalHRManagerNameResolver : IValueResolver<CVEvaluation, CVEvaluationDto, string>
    {
        private readonly IEmployeeService _empService;
        public CVEvalHRManagerNameResolver(IEmployeeService empService)
        {
            _empService = empService;
        }

        public string Resolve(CVEvaluation source, CVEvaluationDto destination, string destMember, ResolutionContext context)
        {
            if(source.HRManagerId==0) return "invalid HR Manager Id";
            var cat = _empService.GetEmployeeName((int)source.HRManagerId);
            if(string.IsNullOrEmpty(cat)) return "Invalid HR Manager Id";
            return cat;
        }
    }
}