using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.Dto;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Processing;
using Core.Enumerations;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class SelDecisionServices : ISelDecisionService


    {
        private readonly ATSContext _context;
        private readonly IGenericRepository<SelDecision> _selRepo;
        private readonly IGenericRepository<CVRef> _cvrefRepo;
        private readonly IGenericRepository<Process> _processRepo;
        
        public SelDecisionServices(ATSContext context, IGenericRepository<SelDecision> selRepo, 
            IGenericRepository<CVRef> cvrefRepo, IGenericRepository<Process> processRepo )
        {
            _processRepo = processRepo;
            _context = context;
            _selRepo = selRepo;
            _cvrefRepo = cvrefRepo;
        }

        public async Task<bool> DeleteSelDecision(SelDecision selDecision)
        {
            var i = await _selRepo.DeleteAsync(selDecision);
            return i == 0 ? true : false;
        }

        public async Task<IReadOnlyList<SelDecision>> GetSelDecisionBetweenDates(DateTime Date1, DateTime Date2, enumSelectionResult result)
        {
            var sel = await _context.SelDecisions
                .Where(x => x.SelectionDate.Date >= Date1.Date && x.SelectionDate.Date <= Date2.Date &&
                x.SelectionResult == result)
                .ToListAsync();

            if (sel == null) throw new Exception("No selection decisions on record with status " +
                Enum.GetName(typeof(enumSelectionResult), result) + " during the dates " + Date1.Date + " and " + Date2.Date);
            return sel;
        }

        public async Task<IReadOnlyList<SelDecision>> GetSelDecisionsWithSpecs(SelDecisionParams selDecisionParams)
        {
            var sel = await _selRepo.GetEntityListWithSpec(new SelDecisionSpecs(selDecisionParams));
            if (sel == null) throw new Exception("Your critieria did not generate any selection data");
            return sel;
        }

        public async Task<IReadOnlyList<SelDecision>> GetSelDecisionsofEnquiryIds(int[] enquiryIds)
        {
            var sel = await _context.SelDecisions.Where(x => enquiryIds.Contains(x.EnquiryId)).ToListAsync();
            if (sel == null) throw new Exception("No selections on record for the chosen Demand letters");
            return sel;
        }

        public async Task<IReadOnlyList<SelDecision>> GetSelDecisionsofEnquiryIdsWithStatus(int[] enquiryIds, enumSelectionResult result)
        {
            var sel = await _context.SelDecisions.Where(x => enquiryIds.Contains(x.EnquiryId)
                && x.SelectionResult == result).ToListAsync();
            if (sel == null) throw new Exception("No selection decisions on record with status " +
                Enum.GetName(typeof(enumSelectionResult), result) + " for the chosen Demand Letter Ids");
            return sel;
        }

        public async Task<IReadOnlyList<SelDecision>> GetSelDecisionsOfEnquiryItemIds(int[] enquiryItemIds)
        {
            var sel = await _context.SelDecisions.Where(x => enquiryItemIds.Contains(x.EnquiryItemId)).ToListAsync();
            if (sel == null) throw new Exception("No selections on record for the chosen Demand letter Categories");
            return sel;
        }

        public async Task<IReadOnlyList<SelDecision>> GetSelDecisionsOfEnquiryItemIdsWithStatus(int[] enquiryItemIds, enumSelectionResult result)
        {
            var sel = await _context.SelDecisions.Where(x => enquiryItemIds.Contains(x.EnquiryItemId)
                && x.SelectionResult == result).ToListAsync();
            if (sel == null) throw new Exception("No selection decisions on record with status " +
                Enum.GetName(typeof(enumSelectionResult), result) + " for the chosen Demand Letter Categories");
            return sel;

        }

        public async Task<IReadOnlyList<SelDecision>> InsertSelDecision(SelDecisionToAddDto selDec)
        {
            var updatedSelDecisions = await CheckSelDecisionDataToAdd(selDec);  //returns List<SelDecision>

            if (updatedSelDecisions == null) return null;

            var sel = await _selRepo.AddListAsync(updatedSelDecisions);
            if (sel == null) throw new Exception("Failed to save the selection decisions");

            return sel;
        }

        //SelDecisionToAddDto contains int[] of CVRefId.  This procedures retrieves candidate and enquiry item details
        //from CVRef and adds to the array to return - List<selDecision>
        private async Task<List<SelDecision>> CheckSelDecisionDataToAdd(SelDecisionToAddDto selDec)
        {

            if (selDec.CVRefIds.Length == 0) throw new Exception("array of CV Ref not provided");

            var selDecList = new List<SelDecision>();

            var errList = new List<clsString>();
            var cvrefList = new List<CVRef>();

            DateTime temp;
            if (!DateTime.TryParse(selDec.SelectionDate.ToString(), out temp))
                throw new Exception("Invalid Selection Date");
            if (selDec.SelectionResult == enumSelectionResult.Referred)
            {
                throw new Exception("no selection decision provided");
            }
            int i = 0;

            var cvr = await _context.SelDecisions.Where(x => selDec.CVRefIds.Contains(x.CVRefID))
                .Select(x => x.CVRefID).ToListAsync();

            if (cvr.Count > 0)
            {
                string ids = "";
                foreach (var c in cvr)
                {
                    ids += ", " + c;
                }
                throw new Exception("CV RefIds " + ids + " already processed for selection decisions");
            }


            try
            {
                foreach (var item in selDec.CVRefIds)
                {
                    ++i;
                    var cvref = await _context.CVRefs
                        .Where(x => x.Id == item && x.RefStatus == enumSelectionResult.Referred).SingleOrDefaultAsync();
                    if (cvref == null)      // record does not exist, bcz either the Id is incorrect, or it is alrady selected/rejected
                    {
                        errList.Add(new clsString("Invalid CVRef Id " + item + " at ordinal " + i + ". It either doesn't exist, or has been selected/rejected"));
                    }
                    else       //the id exists, and status is Referred, so available to select/reject
                    {
                        //SelDecision.ApplicationNo actually is not required, as it can be retrieved from CVRefId, EnquiryItemId
                        //this field is included for convenience
                        //get the applicaiton no from candidateId
                        var appno = await _context.Candidates.Where(x => x.Id == cvref.CandidateId).Select(x => x.ApplicationNo).SingleOrDefaultAsync();
                        //create the object SelDecision to add to the Collection cvrefList.
                        var selDecItem = new SelDecision(item, cvref.EnquiryItemId, cvref.EnquiryId, cvref.CandidateId,
                            appno, selDec.SelectionRef, selDec.SelectionDate, selDec.SelectionResult);
                        selDecList.Add(selDecItem);

                        //change CVRef.Status
                        cvref.RefStatus = selDec.SelectionResult;
                        cvrefList.Add(cvref);
                    }
                }

                //if selected
                enumSelectionResult result = selDec.SelectionResult;

                if (result == enumSelectionResult.Selected)
                {
                    // append a selected record in Process
                    var prcsList = new List<Process>();

                    var processdate = selDec.SelectionDate;
                    var remark = selDec.Remarks;
                    foreach (var id in selDec.CVRefIds)
                    {
                        var qry = await _context.CVRefs.Where(x => x.Id == id)
                            .Select(x => new { x.CandidateId, x.EnquiryItemId, x.EnquiryId }).SingleOrDefaultAsync();

                        prcsList.Add(new Process(id, processdate, enumProcessingStatus.Selected,
                            remark, enumProcessingStatus.OfferAcceptedByCandidate));
                    }
                    await _processRepo.AddListAsync(prcsList);
                }

                string strErr = "";
                if (errList.Count > 0)
                {
                    foreach (var item in errList)
                    {
                        strErr = strErr + Environment.NewLine;
                    }
                    throw new Exception(strErr);
                }

                if (cvrefList.Count > 0) await _cvrefRepo.UpdateListAsync(cvrefList);
                return selDecList;

            }
            catch (ArgumentNullException e)
            {
                //MessageDialog msgDialog = new MessageDialog(w.ToString());
                throw new Exception(e.Message);
            }
        
        }

        private async Task<List<SelDecision>> CheckSelDecisionData(List<SelDecision> selDecs)
        {
            int i = 0;
            var errList = new List<clsString>();
            DateTime temp;
            foreach (var sel in selDecs)
            {
                ++i;
                if (!DateTime.TryParse(sel.SelectionDate.ToString(), out temp))
                    errList.Add(new clsString("Invalid Selection Date at ordinal " + i));

                if (sel.SelectionResult == enumSelectionResult.Referred)
                    errList.Add(new clsString("no selection decision provided at ordinal " + i));

                var cvref = await _context.CVRefs
                    .Where(x => x.Id == sel.CVRefID && x.RefStatus == enumSelectionResult.Referred)
                    .Select(x => new { x.EnquiryItemId, x.CandidateId, x.EnquiryId }).SingleOrDefaultAsync();
                if (cvref == null)
                {
                    errList.Add(new clsString("Invalid CVRef Id " + sel.CVRefID + " at ordinal " + i +
                        ". It either doesn't exist, or has been selected/rejected"));
                    break;
                }
                if (sel.CandidateId != cvref.CandidateId || sel.EnquiryItemId != cvref.EnquiryItemId)
                {
                    errList.Add(new clsString("Invalid CandidateId or EnquiryItemId at ordinal " + i));
                    break;
                }
                if (cvref.EnquiryId != 0 && sel.EnquiryId != 0 && cvref.EnquiryId != sel.EnquiryId) sel.EnquiryId = cvref.EnquiryId;

                var cnd = await _context.Candidates.Where(x => x.Id == cvref.CandidateId).Select(x => x.ApplicationNo).SingleOrDefaultAsync();
                if (sel.ApplicationNo != cnd)
                {
                    if (sel.ApplicationNo == 0)
                    {
                        sel.ApplicationNo = cnd;
                    }
                    else
                    {
                        errList.Add(new clsString("Invalid Application No at ordinal " + i));
                        break;
                    }
                }
            }

            string strErr = "";
            if (errList.Count > 0)
            {
                foreach (var item in errList)
                {
                    strErr = strErr + Environment.NewLine;
                }
                throw new Exception(strErr);
            }

            return selDecs;

        }

        public async Task<int> UpdateSelDecisions(List<SelDecision> selDecisions)
        {
            var updatedSelDecisions = await CheckSelDecisionData(selDecisions);
            if (updatedSelDecisions == null) return 0;

            var sel = await _selRepo.UpdateListAsync(updatedSelDecisions);
            return sel;
        }

        public async Task<SelDecision> UpdateSelDecision(SelDecision selDecision)
        {
            var lst = new List<SelDecision>();
            lst.Add(selDecision);
            var updatedSelDecisions = await CheckSelDecisionData(lst);
            if (updatedSelDecisions == null) return null;
            var sel = lst[0];
            var updatedsel = await _selRepo.UpdateAsync(sel);
            return sel;
        }

        public async Task<IReadOnlyList<CVRef>> GetPendingSelectionsAll()
        {
            var sel = await _context.CVRefs.Where(x => x.RefStatus == enumSelectionResult.Referred)
                .OrderBy(x => x.EnquiryId).OrderBy(x => x.EnquiryItemId).ToListAsync();
            if (sel == null || sel.Count == 0) throw new Exception("No pending selections as on date");

            return sel;
        }

        public async Task<IReadOnlyList<CVRef>> GetPendingSelectionsOfDLs(int[] enquiryIds)
        {
            var sel = await _context.CVRefs.Where(x => enquiryIds.Contains(x.EnquiryId)
                && x.RefStatus == enumSelectionResult.Referred)
                .OrderBy(x => x.DateForwarded)
                .ToListAsync();

            if (sel == null || sel.Count == 0) throw new Exception("The selected DL has no candidates selected");

            return sel;
        }

        public async Task<IReadOnlyList<CVRef>> GetPendingSelectionsOfDLItems(int[] enquiryItemIds)
        {
            var sel = await _context.CVRefs.Where(x => enquiryItemIds.Contains(x.EnquiryItemId)
                && x.RefStatus == enumSelectionResult.Referred)
                .OrderBy(x => x.DateForwarded)
                .ToListAsync();

            if (sel == null || sel.Count == 0) throw new Exception("The selected DL Category has no candidates selected");

            return sel;
        }

    }
}
