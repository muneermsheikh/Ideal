using Core.Entities.Masters;

namespace Core.Entities.HR
{
    public class CandidateCategory: BaseEntity
    {
        public CandidateCategory()
        {
        }

        public CandidateCategory(int candidateId, int categoryId)
        {
            CandId = candidateId;
            CatId = categoryId;
        }

        public int CandId  {get; set; }
        public int CatId {get; set; }
        //public Candidate Candidate {get; set; }
        //public Category Category {get; set; }
    }
}