using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Enumerations;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class EmolumentService : IEmolumentService
    {
        private readonly IGenericRepository<Emolument> _emolumentRepo;
        private readonly ATSContext _context;
        public EmolumentService(IGenericRepository<Emolument> emolumentRepo, ATSContext context)
        {
            _context = context;
            _emolumentRepo = emolumentRepo;
        }

        public async Task<IReadOnlyList<Emolument>> AddEmoluments(List<Emolument> emoluments)
        {
            if (await EmolumentsExist(emoluments)) throw new System.Exception ("emoluemnts for some or all of the entities already exist");
            if (!await ConfirmCVRefIdExists(emoluments)) throw new System.Exception("Bad Request - invalid Selection references");

            var emolList = new List<Emolument>();
            foreach(var item in emoluments)
            {
                emolList.Add(new Emolument(item.CVRefId, item.SalaryCurrency, item.BasicSalary, item.WeeklyWorkHours, 
                    item.ContractPeriodInMonths, item.Housing, item.HousingAllowance, item.Food, 
                    item.FoodAllowance, item.Transport, item.TransportAllowance, item.OtherAllowance, 
                    item.OtherAllowanceAmount, item.AirportOfBoarding, item.AirportOfDestination, 
                    item.OfferAcceptedByCandidate, item.OfferAcceptedOn));
            }
            var ret = await _emolumentRepo.AddListAsync(emolList);
            if (ret==null) throw new System.Exception("Failed to insert the remuneration records");

            return ret;
        }


        public async Task<int> DeleteEmolument(Emolument emolument)
        {
            return await _emolumentRepo.DeleteAsync(emolument);
        }

        public async Task<Emolument> GetEmolumentOfCandidate(int candidateId, int enquiryItemId)
        {
            var cvrefid = await _context.CVRefs
                .Where(x=>x.CandidateId == candidateId && x.EnquiryItemId==enquiryItemId)
                .Select(x=>x.Id).SingleOrDefaultAsync();
            if (cvrefid==0) return null;

            var emolument = await _context.Emoluments.Where(x => x.CVRefId == cvrefid).SingleOrDefaultAsync();
            return emolument;

        }

        public async Task<IReadOnlyList<Emolument>> GetEmolumentsOfEnquiryCategory(int enquiryItemId)
        {
            var enqitemids = await _context.CVRefs
                .Where(x => x.EnquiryItemId == enquiryItemId)
                .Select(x => x.EnquiryItemId)
                .ToListAsync();
            if (enqitemids==null || enqitemids.Count==0) return null;

            var emols = await _context.Emoluments
                .Where(x => enqitemids.Contains(x.CVRefId))
                .OrderBy(x => x.CVRefId).ToListAsync();

            return emols;
        }

        public async Task<int> UpdateEmoluments(List<Emolument> emoluments)
        {
             if (!await EmolumentsExist(emoluments)) 
                throw new System.Exception ("some or all of the emolument records are not on record");
            
            return await _emolumentRepo.UpdateListAsync(emoluments);
        }

        public async Task<Emolument> UpdateEmolument(Emolument emolument)
        {
             var Lst = new List<Emolument>();
             Lst.Add(emolument);
             if (!await EmolumentsExist(Lst)) 
                throw new System.Exception ("No such record exists");
            
            return await _emolumentRepo.UpdateAsync(emolument);
        }

    //privates
        private async Task<bool> EmolumentsExist(List<Emolument> emoluments)
        {
            foreach(var item in emoluments)
            {
                var id = await _context.Emoluments.Where(x=>x.CVRefId==item.CVRefId).Select(x=>x.Id).SingleOrDefaultAsync();
                if (id!=0) return true;
            }
            return false;
        }
        private async Task<bool> ConfirmCVRefIdExists(List<Emolument> emoluments)
        {
            foreach (var item in emoluments)
            {
                var b = await _context.CVRefs.Where(x=>x.Id==item.CVRefId && x.RefStatus == enumSelectionResult.Selected)
                    .Select(x=>x.Id).SingleOrDefaultAsync();
                if(b==0) return false;
            }
            return true;
        }

    }
}