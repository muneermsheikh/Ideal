using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dto;
using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Identity;
using Core.Entities.Processing;
using Core.Enumerations;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class ProcessController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IProcessServices _procservice;

        private readonly ATSContext _context;
        private readonly IMapper _mapper;
        public ProcessController(UserManager<AppUser> userManager, IProcessServices procservice,
             ATSContext context, IMapper mapper)
        {
            _procservice = procservice;
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<IReadOnlyList<ProcessAddedDto>>> AddProcessTransaction([FromBody]ProcessTransactionsAddDto processAdd)
        {
            var prcs =  await _procservice.AddProcessTransactions(processAdd.TransactionDate, 
                processAdd.ProcessingStatus, processAdd.Remarks, processAdd.CVRefIds, processAdd.travelToAddDto);
            if (prcs==null) return NotFound(new ApiResponse(404, "No processing records found"));

            var data = _mapper.Map<IReadOnlyList<Process>, IReadOnlyList<ProcessAddedDto>>(prcs);
            
            return Ok(data);
        }


        [HttpPut("transaction")]
        public async Task<ActionResult<Process>> UpdateProcessTransaction(Process process)
        {
            var prcs = await _procservice.UpdateProcessTransaction(process);
            if (prcs == null) return BadRequest(new ApiResponse(400, "Bad Request"));
            return prcs;
        }

        [HttpPut("transactions")]
        public async Task<ActionResult<int>> UpdateProcessTransactions(List<Process> Processes)
        {
            var prcs = await _procservice.UpdateProcessTransactions(Processes);
            if (prcs == 0) return BadRequest(new ApiResponse(400, "Bad Request"));
            return prcs;
            //return Ok(_mapper.Map<IReadOnlyList<Process>,IReadOnlyList<ProcessDto>>(prcs));
        }

        [HttpDelete]
        public async Task<ActionResult<int>> DeleteProcessTransaction(Process process)
        {
            var deleted = await _procservice.DeleteProcessTransaction(process);
            if (deleted==0) return BadRequest(new ApiResponse(400, "Bad Request - failed to delete the object"));
            return deleted;
        }

        [HttpGet("candidate")]
        public async Task<ActionResult<HistoryDto>> GetCandidateHistory(int candidateId)
        {
            var h = await _procservice.GetCandidateHistory(candidateId);
            if (h==null) return NotFound(new ApiResponse(404, "No deployment history for the selected candidate"));
            
            var hist = MapCVRefToCandidateHistory(h);

            return Ok(hist);
        }

        [HttpGet("enqs")]
        public async Task<ActionResult<List<DLHistoryDto>>> GetDLHistory([FromBody] int[] enquiryids)
        {
            var enqs = await _procservice.GetDLProcessDetails(enquiryids);
            if (enqs==null || enqs.Count ==0) return NotFound(new ApiResponse(404, "No process data available for selected enquiry Ids"));
            
            //map Enquiry to DLHistoryDto
            var dlhistorydto = new List<DLHistoryDto>();
            foreach(var en in enqs)
            {
                var hist = await MapCVRefToDLHistory(en);
                dlhistorydto.Add(hist);
            }

            return dlhistorydto;
        }

//private
        private async Task<DLHistoryDto> MapCVRefToDLHistory(Enquiry enq)
        {
            
            // DL -> DL Category -> Candidates Referred -> Process details
            // use navigation properties - Enquiry -> EnquiryItems -> CVRef -> Process
            
            var Lst = new DLHistoryDto();
            Lst.EnquiryNo = enq.EnquiryNo.ToString();
            Lst.EnquiryDated = enq.EnquiryDate.Date.ToString();
            var items = enq.EnquiryItems.Select(x => new {x.SrNo, x.CategoryItemId, x.Quantity, 
                x.MaxCVsToSend, x.CVsReferred})
                .OrderBy(x => x.SrNo).ToList();
            var DLHistCat = new List<DLCategory>();
            foreach(var c in items)
            {
                var navigationRef = c.CVsReferred;
                var candidateRefList = new List<CandidateRef>();
                foreach(var item in navigationRef)
                {
                    var processHist = new List<HistoryProcess>();
                    foreach(var prc in item.ProcessTransactions)
                    {
                        //4th, i.e. last, level of process transactions
                        processHist.Add(new HistoryProcess(prc.ProcessingDate.ToString(), 
                            Enum.GetName(typeof(enumProcessingStatus), prc.Status), 
                            Enum.GetName(typeof(enumProcessingStatus), prc.NextProcessingId)));
                    }
                    
                    //3rd level of candidate rferrals
                    string candDetails="";
                    var cnd = item.Candidate;
                    if (cnd==null)
                    {
                        var cn = await _context.Candidates.Where(x => x.Id == item.CandidateId)
                            .Select( x => new {x.ApplicationNo, x.FullName, x.PassportNo}).SingleOrDefaultAsync();
                        if (cn !=null)
                        {
                            candDetails = cn.ApplicationNo + "-" + cn.FullName + ", PP No."+ cn.PassportNo;
                        }
                        else
                        {
                            candDetails = "failed to retrieve details";
                        }
                    }
                    else {
                        candDetails=cnd.ApplicationNo + "-" + cnd.FullName + ", PP No." + cnd.PassportNo;
                    }
                    candidateRefList.Add(new CandidateRef(candDetails, "", processHist));
                }
                //2nd level of DL categories
                var category  = await _context.Categories.Where(x => x.Id == c.CategoryItemId)
                    .Select(x => c.SrNo + "-" + x.Name).SingleOrDefaultAsync();
                DLHistCat.Add(new DLCategory(category, c.Quantity, candidateRefList));
            }

            Lst.DLCategories=DLHistCat;
            return Lst;
        }

        private async Task<HistoryDto> MapCVRefToCandidateHistory(List<CVRef> cvrefs)
        {
            var Lst = new HistoryDto();
            int cid =  cvrefs.Select(x => x.CandidateId).FirstOrDefault();

            var cand = await _context.Candidates.Where(x => x.Id == cid).FirstOrDefaultAsync();
            Lst.CandidateName = cand.ApplicationNo + "-" + cand.FullName + ", PP No. " + cand.PassportNo;

            var refs = new List<HistoryReferred>();
            foreach(var c in cvrefs)
            {
                var enqitem = await 
                (   from enq in _context.Enquiries
                    join item in _context.EnquiryItems on enq.Id equals item.EnquiryId
                    join cat in _context.Categories on item.CategoryItemId equals cat.Id
                    where item.Id == c.EnquiryItemId 
                    orderby enq.Id, item.SrNo
                    select new 
                    {
                        customername=enq.Customer.CustomerName, 
                        categoryref = enq.EnquiryNo + "-" + item.SrNo + "-" + cat.Name
                    }
                ).FirstOrDefaultAsync();

                var LstProcess = new List<HistoryProcess>();
                foreach(var p in c.ProcessTransactions)
                {
                    LstProcess.Add(new HistoryProcess(p.ProcessingDate.ToString(), 
                        Enum.GetName(typeof(enumProcessingStatus), p.Status),
                        Enum.GetName(typeof(enumProcessingStatus), p.NextProcessingId)));
                }

                var r = new HistoryReferred(enqitem.categoryref, enqitem.customername, 
                    c.DateForwarded.ToString(), LstProcess);
                refs.Add(r);

            }
            Lst.ReferredList=refs;
            return Lst;
        }
/*
        [HttpGet("enquiries")]
        public ActionResult<List<RequirementPendingDto>> GetDLProcessDetails(int[] enquiryIds)
        {
            var dtls =  _procservice.GetDLProcessDetails(enquiryIds);
            if (dtls==null) return NotFound(new ApiResponse(404, "No processing transactions on record for the selected DLs"));
            return Ok(dtls);
        }
*/
        [HttpGet("categories")]
        public async Task<ActionResult<List<HistoryReferred>>> GetDLCategoryProcessDetails(int[] enquiryItemIds)
        {
            var dtls = await _procservice.GetDLCategoryProcessDetails(enquiryItemIds);
            
            if (dtls==null) return NotFound(new ApiResponse(404, "No processing transactions on record for the selected DL Categories"));
            
            var prcs = _mapper.Map<IReadOnlyList<EnquiryItem>, IReadOnlyList<HistoryReferred>>(dtls);
            return Ok(prcs);
        }


    }
}