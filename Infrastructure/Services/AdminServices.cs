using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Interfaces;
using Infrastructure.Data;
using System.Linq;
using Core.Enumerations;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Services
{
    public class AdminServices : IAdminServices
    {
        private readonly ATSContext _context;
        public AdminServices(ATSContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<CVForward>> CVForwardDetailsOfDLs(int[] enquiryIds)
        {
            var fwdids = await _context.CVForwardItems.Where(x => enquiryIds.Contains(x.EnquiryId))
                .Select(x=>x.CVForwardId).Distinct().ToListAsync();
            var cvfwdList = new List<CVForward>();
            foreach(var item in fwdids)
            {
                var cvfwd=await _context.CVForwards.Where(x => x.Id==item).FirstOrDefaultAsync();
                if (cvfwd !=null)
                {
                    cvfwdList.Add(cvfwd);
                }
            }
            foreach(var item in cvfwdList)
            {
                var items = await _context.CVForwardItems.Where(x=>x.CVForwardId==item.Id).OrderBy(x=>x.SrNo).ToListAsync();
                item.CVForwardItems=items;
            }
            return cvfwdList;
        }

        public async Task<IReadOnlyList<RequirementPendingDto>> PendingRequirements()
        {
            
            var query = await (from e in _context.Enquiries
                join i in _context.EnquiryItems on e.Id equals i.EnquiryId 
                join cat in _context.Categories on i.CategoryItemId equals cat.Id
                join r in _context.CVRefs on i.Id equals r.EnquiryItemId 

                where i.EnquiryStatus != enumEnquiryStatus.Concluded
                
                group new {i.Id, r.RefStatus}
                by new 
                    {
                        e.Customer.CustomerName, e.EnquiryNo, e.EnquiryDate, 
                        i.SrNo, i.CompleteBy, i.MaxCVsToSend,  cat.Name, i.Quantity, 
                    } into g 
                orderby g.Key.EnquiryNo, g.Key.SrNo
                select new
                {
                    //cvrefid= g.Key.Id,
                    //currentstatus = g.Key.EnquiryStatus,
                    maxcvs = g.Key.MaxCVsToSend,
                    enquirynumber = g.Key.EnquiryNo,
                    enquirydated = g.Key.EnquiryDate.Date.ToString("dd MMMM yyyy"),
                    customername = g.Key.CustomerName,
                    srno = g.Key.SrNo,
                    categoryname = g.Key.Name,
                    completeby = g.Key.CompleteBy.Value.ToString("dd MMMM yyyy"),
                    quantity = g.Key.Quantity,
                    referred = g.Count(),
                    selected = g.Sum(x => x.RefStatus == enumSelectionResult.Selected ? 1: 0), 
                    rejected = g.Sum(x => x.RefStatus == enumSelectionResult.RejectedFakeProfile || 
                        x.RefStatus == enumSelectionResult.RejectedHighExpectations || x.RefStatus == enumSelectionResult.RejectedInsufficientExp || 
                        x.RefStatus == enumSelectionResult.RejectedNoReason || x.RefStatus == enumSelectionResult.RejectedNotQualified ||
                        x.RefStatus == enumSelectionResult.RejectedOverAge || x.RefStatus == enumSelectionResult.RejectedRequiremetFulfilled ||
                        x.RefStatus == enumSelectionResult.RejectedSuspiciousProfile ? 1 : 0), 
                    deployed = g.Sum(x => x.RefStatus == enumSelectionResult.Deployed ? 1 : 0)
                }).ToListAsync();
            
            var Lst = new List<RequirementPendingDto>();
            foreach(var item in query)
            {
                var underreview=item.referred - item.selected - item.rejected;
                var balcvstosend = ((item.maxcvs / item.quantity) * (item.quantity - item.selected)) - underreview;
                if (balcvstosend < 0) balcvstosend = 0;
                Lst.Add(new RequirementPendingDto(item.enquirynumber, item.enquirydated, item.customername, 
                    string.Concat(item.srno, "-", item.categoryname), item.quantity, item.referred, item.selected, item.rejected, 
                    underreview, item.deployed, balcvstosend, item.completeby, ""));
            }
            return Lst;
           
        }
        public async Task<IReadOnlyList<RequirementPendingDto>> PendingRequirements(int[] enquiryIds)
        {
            
            var query = await (from e in _context.Enquiries
                join i in _context.EnquiryItems on e.Id equals i.EnquiryId 
                join cat in _context.Categories on i.CategoryItemId equals cat.Id
                join r in _context.CVRefs on i.Id equals r.EnquiryItemId 

                where enquiryIds.Contains(e.Id) && i.EnquiryStatus != enumEnquiryStatus.Concluded
                
                group new {i.Id, r.RefStatus}
                by new 
                    {
                        e.Customer.CustomerName, e.EnquiryNo, e.EnquiryDate, 
                        i.SrNo, i.CompleteBy, i.MaxCVsToSend,  cat.Name, i.Quantity, 
                    } into g 
                orderby g.Key.EnquiryNo, g.Key.SrNo
                select new
                {
                    //cvrefid= grp.Key.Id,
                    //currentstatus = grp.Key.EnquiryStatus,
                    maxcvs = g.Key.MaxCVsToSend,
                    enquirynumber = g.Key.EnquiryNo,
                    enquirydated = g.Key.EnquiryDate.ToString("dd MMMM yyyy"),
                    customername = g.Key.CustomerName,
                    srno = g.Key.SrNo,
                    categoryname = g.Key.Name,
                    completeby = g.Key.CompleteBy.Value.ToString("dd MMMM yyyy"),
                    quantity = g.Key.Quantity,
                    referred = g.Count(),
                    selected = g.Sum(x => x.RefStatus == enumSelectionResult.Selected ? 1: 0), 
                    rejected = g.Sum(x => x.RefStatus == enumSelectionResult.RejectedFakeProfile || 
                        x.RefStatus == enumSelectionResult.RejectedHighExpectations || x.RefStatus == enumSelectionResult.RejectedInsufficientExp || 
                        x.RefStatus == enumSelectionResult.RejectedNoReason || x.RefStatus == enumSelectionResult.RejectedNotQualified ||
                        x.RefStatus == enumSelectionResult.RejectedOverAge || x.RefStatus == enumSelectionResult.RejectedRequiremetFulfilled ||
                        x.RefStatus == enumSelectionResult.RejectedSuspiciousProfile ? 1 : 0), 
                    deployed = g.Sum(x => x.RefStatus == enumSelectionResult.Deployed ? 1 : 0)
                }).ToListAsync();
            
            var Lst = new List<RequirementPendingDto>();

            foreach(var item in query)
            {
                var underreview = item.referred - item.selected - item.rejected;
                var balcvstosend = ((item.maxcvs / item.quantity) * (item.quantity - item.selected)) - underreview;
                if (balcvstosend < 0) balcvstosend = 0;
                Lst.Add(new RequirementPendingDto(item.enquirynumber, item.enquirydated, item.customername, 
                    string.Concat(item.srno, "-", item.categoryname), item.quantity, item.referred, item.selected, item.rejected, 
                    underreview, item.deployed, balcvstosend, item.completeby, ""));
            }
            return Lst;
           
        }
    }
}
