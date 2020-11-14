using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.HR;
using Core.Entities.Masters;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class CandidateService : ICandidateService
    {
        private readonly IGenericRepository<Candidate> _candidateRepo;
        private readonly ATSContext _context;
        private readonly ICandidateCategoryService _candidateCategoryService;
        private readonly IGenericRepository<CandidateCategory> _candCatRepo;
        public CandidateService(IGenericRepository<Candidate> candidateRepo, 
                                IGenericRepository<CandidateCategory> candCatRepo,   
                                ICandidateCategoryService candidateCategoryService, 
                                ATSContext context)
        {
            _candidateCategoryService = candidateCategoryService;
            _context = context;
            _candidateRepo = candidateRepo;
            _candCatRepo = candCatRepo;
        }

        public Task<bool> DeleteCandidate(Candidate candidate)
        {
            throw new System.NotImplementedException();
        }

        public Task<Candidate> GetCandidate(CandidateParams candidateParams)
        {
            throw new System.NotImplementedException();
        }

        public Task<Candidate> GetCandidateByApplicationNo(int ApplicationNo)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Candidate> GetCandidateById(int candidateId)
        {
            var cv = await _context.Candidates.AsNoTracking().Where
                (x => x.Id == candidateId)
                .FirstOrDefaultAsync();
            return cv;
        }

        public async Task<Candidate> GetCandidateBySpec(CandidateParams candidateParams)
        {
            var cand = await _candidateRepo.GetEntityWithSpec(
                new CandidateSpecs(candidateParams));
            if (candidateParams.includeCategories && cand.CandidateCategories==null)
                cand.CandidateCategories = await _candidateCategoryService.GetCandidateCategoryType(cand.Id);
            return cand;
        }

        public async Task<IReadOnlyList<Candidate>> GetCandidatesBySpecs(CandidateParams candidateParams)
        {
            var cands = await _candidateRepo.GetEntityListWithSpec(
                new CandidateSpecs(candidateParams));
            if (candidateParams.includeCategories)
            { 
                foreach(var cand in cands)
                {
                    if (cand.CandidateCategories==null || cand.CandidateCategories.Count==0)
                    cand.CandidateCategories = await _candidateCategoryService.GetCandidateCategoryType(cand.Id);
                }
            }
            return cands;
        }

        public async Task<Candidate> RegisterCandidate(Candidate candidate)
        {
            
            if (!await ValidateCandidateToAdd(candidate)) return null;
            if (candidate.ApplicationNo == 0)
            {
                candidate.ApplicationNo = await GetNextApplicationNo();
            }

            var candidatecategories = candidate.CandidateCategories;
            var candidatecatsNew=new List<CandidateCategory>();
            
            candidate.CandidateCategories = null;       //values coied to candidatecatsNew, to be saved separately below
            var candidateAdded = await _candidateRepo.AddAsync(candidate);
            
            if (candidatecategories != null)
            {
                foreach (var cand in candidatecategories)
                {   
                    // ** use cache here, instead of accessing DB
                    string catName = await _context.Categories
                        .Where(x => x.Id == cand.CategoryId).Select(x => x.Name).SingleOrDefaultAsync();
                    candidatecatsNew.Add(new CandidateCategory(candidateAdded.Id, cand.CategoryId, catName));
                }
                var candidatecatsAdded = await _candidateCategoryService.AddCandidateCategories(candidatecatsNew);
            }
            
            return candidateAdded;
        }

        public async Task<IReadOnlyList<Candidate>> RegisterCandidates(IReadOnlyList<Candidate> candidates)
        {
            return await _candidateRepo.AddListAsync(candidates);
        }

        public async Task<Candidate> UpdateCandidate(Candidate cv)
        {
          
            var CandCats = await _context.CandidateCategories.Where(x => x.CandidateId == cv.Id).ToListAsync();
            if (CandCats != null) {
                await _candCatRepo.DeleteListAsync(CandCats);
            }

            await _candidateRepo.UpdateAsync(cv);

            // finally, update all entities
            await _context.SaveChangesAsync();

            return await _context.Candidates.Where(x => x.Id == cv.Id)
                .Include(x => x.CandidateCategories).SingleOrDefaultAsync();
            /*
            var cats = cv.CandidateCategories;
            if (cats == null || cats.Count ==0) {
                return null;
            }
            foreach(var c in cats)
            {
                if (c.CandId == 0) {
                    c.CandId = cv.Id;
                if (string.IsNullOrEmpty(c.Name)) {
                    c.Name = await _context.Categories.Where(x => x.Id == c.CatId).Select(x => x.Name).SingleOrDefaultAsync();
                }
                }
            }

            return await _candidateRepo.UpdateAsync(cv);
        */
        }

        public async Task<bool> ValidateCandidateToAdd(Candidate candidate)
        {
            if (string.IsNullOrEmpty(candidate.PassportNo) && string.IsNullOrEmpty(candidate.AadharNo))
                throw new Exception("Either PP number or Aadhar Card No must be provided"); 

            /*
            if (candidate.CandidateCategories==null) throw new Exception ("candidate must have atleast one category");
            if (candidate.CandidateCategories.Count == 0) 
                throw new Exception("atleast one category must be provided");
        */
            int appNo = await CandidateAppNoOrPPNoOrAadharNoOrEmailExist(candidate);
            if (appNo !=0) throw new Exception("Application No. " + appNo + 
                " exists that contains same PP No, AadharNo or eMailID");
            return true;
        }

        public async Task<Candidate> PPNumberExists(string ppnumber)
        {
            var app = await _context.Candidates
                .Where(x => x.PassportNo == ppnumber)
                .FirstOrDefaultAsync();
            
            return app;
        }

        public async Task<Candidate> AadharNumberExists(string aadharNo)
        {
            var app = await _context.Candidates.Where
                (x => x.AadharNo == aadharNo)
                .FirstOrDefaultAsync();
            return app;
        }

        
        public async Task<Candidate> CandidateAppNoOrPPNoOrAadharNoOrEmailExist(int appno, string ppno, string aadharno, string email)
        {
            var cv = await _context.Candidates.AsNoTracking()
                .Include(x => x.CandidateCategories)
                .Where
                (x => x.ApplicationNo == appno && appno != 0 ||
                (!string.IsNullOrEmpty(ppno) && x.PassportNo == ppno) ||
                (!string.IsNullOrEmpty(aadharno) && x.AadharNo == aadharno) ||
                (!string.IsNullOrEmpty(email) && x.Email == email))
                .FirstOrDefaultAsync();
            return cv;
        }

        public async Task<int> CandidateAppNoOrPPNoOrAadharNoOrEmailExist(Candidate candidate)
        {

            var appNo = await _context.Candidates.AsNoTracking().Where
                (x => x.ApplicationNo == candidate.ApplicationNo &&
                    candidate.ApplicationNo != 0 ||
                (!string.IsNullOrEmpty(candidate.PassportNo) && x.PassportNo == candidate.PassportNo) ||
                (!string.IsNullOrEmpty(candidate.AadharNo) && x.AadharNo == candidate.AadharNo) ||
                (!string.IsNullOrEmpty(candidate.Email) && x.Email == candidate.Email))
                .Select(x=>x.ApplicationNo).FirstOrDefaultAsync();
            return appNo;
        }

        public async Task<int> GetNextApplicationNo()
        {
            return await _context.Candidates.MaxAsync(x => x.ApplicationNo) + 1;
        }

        public string GetCandidateName (int candidateId)
        {
            var candName = _context.Candidates.Where(x=>x.Id==candidateId).Select(x=>x.FullName).FirstOrDefault();
            if(string.IsNullOrEmpty(candName)) return "invalid candidateId";
            return candName;
        }

        public async Task<int> UpdateCandidateCategories(List<CandidateCategory> candCats)
        {
            return await _candidateCategoryService.UpdateCandidateCategories(candCats);
        }

        public Task<IReadOnlyList<Candidate>> RegisterCandidate(IReadOnlyList<Candidate> candidate)
        {
            throw new NotImplementedException();
        }

    //sources
        public async Task<IReadOnlyList<Source>> GetSources()
        {
            return await _context.Sources.OrderBy(x => x.Name).ToListAsync();
        }

    // candCat with Prof
        public async Task<List<Category>> GetCandCatsWithProf()
        {
            return await _candidateCategoryService.GetCandididateCatsWithProf();
        }
    }
}
