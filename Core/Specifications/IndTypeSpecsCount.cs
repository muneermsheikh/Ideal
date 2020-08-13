using Core.Entities.Masters;

namespace Core.Specifications
{
    public class IndTypeSpecsCount: BaseSpecification<IndustryType>
    {
        public IndTypeSpecsCount(IndTypeSpecParams indParams)
            : base(x => 
                (string.IsNullOrEmpty(indParams.Search) || x.Name.ToLower().Contains(indParams.Search)) &&
                (!indParams.Id.HasValue || x.Id == indParams.Id) 
            )
        {
        }
        

    public IndTypeSpecsCount(int id) : base(x => x.Id == id)
        {
        }
    }
}