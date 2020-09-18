using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;

namespace Core.Interfaces
{
    public interface IEmolumentService
    {
         Task<IReadOnlyList<Emolument>> AddEmoluments(List<Emolument> emoluments);
         Task<int> UpdateEmoluments(List<Emolument> emoluments);
         Task<Emolument> UpdateEmolument (Emolument emolument);
         Task<int> DeleteEmolument(Emolument emolument);
         Task<Emolument> GetEmolumentOfCandidate(int candidateId, int enquiryItemId);
         Task<IReadOnlyList<Emolument>> GetEmolumentsOfEnquiryCategory (int enquiryItemId);
    }
}