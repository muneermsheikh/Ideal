using API.Dtos;
using AutoMapper;
using Core.Entities.HR;
using Core.Interfaces;

namespace API.Helpers
{
    public class CVEvalHRExecNameResolver : IValueResolver<CVEvaluation, CVEvaluationDto, string>
    {
        private readonly IEmployeeService _empService;
        public CVEvalHRExecNameResolver(IEmployeeService empService)
        {
            _empService = empService;
        }

        public string Resolve(CVEvaluation source, CVEvaluationDto destination, string destMember, ResolutionContext context)
        {
            if(source.HRExecutiveId==0) return "invalid HR Executive Id";
            var cat = _empService.GetEmployeeName(source.HRExecutiveId);
            if(string.IsNullOrEmpty(cat)) return "Invalid HR Executive Id";
            return cat;
        }
    }
}