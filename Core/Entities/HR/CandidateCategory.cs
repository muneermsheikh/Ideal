using Core.Entities.Masters;

namespace Core.Entities.HR
{
    public class CandidateCategory: BaseEntity
    {
        public CandidateCategory()
        {
        }

        public CandidateCategory(int candidateId, int categoryId, string catName)
        {
            CandidateId = candidateId;
            CategoryId = categoryId;
            Name = catName;
        }

        public int CandidateId  {get; set; }
        public int CategoryId {get; set; }
        public string Name {get; set;}
        //public Candidate Candidate {get; set; }
        //public Category Category {get; set; }
    }
}