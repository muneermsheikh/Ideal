using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Processing;
using Core.Enumerations;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ProcessServices : IProcessServices
    {
        private readonly ATSContext _context;
        private readonly IGenericRepository<Process> _processRepo;
        private readonly IGenericRepository<Travel> _travelRepo;
        public ProcessServices(IGenericRepository<Process> processRepo, IGenericRepository<Travel> travelRepo, ATSContext context)
        {
            _travelRepo = travelRepo;
            _processRepo = processRepo;
            _context = context;
        }

        public async Task<List<CVRef>> GetCandidateHistory(int candidateId)
        {

            var hist = await _context.CVRefs.Where(x => x.CandidateId == candidateId)
                .Include(x => x.ProcessTransactions).ToListAsync();
            if (hist == null || hist.Count == 0)
                throw new Exception("Candidate does not have any processing transactions");

            return hist;
        }

        public async Task<int> DeleteProcessTransaction(Process process)
        {
            return await _processRepo.DeleteAsync(process);
        }

        public async Task<List<Enquiry>> GetDLProcessDetails(int[] enquiryIds)
        {


            var enqs = await _context.Enquiries
                .Include(x => x.EnquiryItems)
                .ThenInclude(x => x.CVsReferred)
                .ThenInclude(x => x.ProcessTransactions)
                .Where(x => enquiryIds.Contains(x.Id))
                .OrderBy(x => x.EnquiryNo)
                .ToListAsync();

            if (enqs == null || enqs.Count == 0) throw new Exception("no refererrals avaiable for selected enquiry Ids");

            return enqs;
        }


        public async Task<List<EnquiryItem>> GetDLCategoryProcessDetails(int[] enquiryitemIds)
        {
            var Lst = new List<CVRef>();

            //return processList;

            var processList = await _context.EnquiryItems
                .Include(x => x.CVsReferred)
                .ThenInclude(x => x.ProcessTransactions)
                .Where(x => enquiryitemIds.Contains(x.Id))
                .ToListAsync();

            if (processList == null) throw new Exception("No processing transactions on record for selected DL Categories");

            return processList;

        }

        public async Task<Process> UpdateProcessTransaction(Process processing)
        {
            var lst = new List<Process>();
            lst.Add(processing);
            var verified = ProcessTransactionsVerifiedForUpdate(lst).FirstOrDefault();
            if (verified == null) return null;

            verified = await _processRepo.UpdateAsync(verified);
            return verified;
        }

        public async Task<int> UpdateProcessTransactions(List<Process> processings)
        {
            var verified = ProcessTransactionsVerifiedForUpdate(processings);
            if (verified == null) return 0;

            var ret = await _processRepo.UpdateListAsync(verified);
            return ret;
        }

        public async Task<IReadOnlyList<Process>> AddProcessTransactions(DateTime TransactionDate,
            enumProcessingStatus status, string remarks, int[] cvrefIds, TravelToAddDto travel)
        {
            var processList = await ProcessTransasctionsVerifiedForAdd(TransactionDate, 
                status, remarks, cvrefIds, travel);

            if (processList == null) return null;

            var prcs = await _processRepo.AddListAsync(processList);
            if (status == enumProcessingStatus.TravelBooked && travel != null) 
            {
                var trvls = new List<Travel>();
                foreach(var item in prcs)
                {
                    trvls.Add(new Travel(item.Id, item.CVRefId, travel.BoardingAirport,
                        travel.DestinationAirport, travel.Airline, travel.FlightNo, 
                        travel.ETD, travel.ETA, travel.BookedOn, travel.PNR));
                }
                await _travelRepo.AddListAsync(trvls);
            }
            
            return prcs;
        }

        //privates       
        private List<Process> ProcessTransactionsVerifiedForUpdate(List<Process> processes)
        {
            //UPDATES only for 1 cvrefid at a time
            var dist = processes.Select(x => x.CVRefId).Distinct().AsEnumerable();
            if (dist.Count() > 1) throw new Exception("Only one set of candidate transactions for a requirement can be undertaken at a time");
            int cvrefid = dist.FirstOrDefault();

            if (!ProcessTransactionsAreNotMissing(processes)) throw new Exception("Process transactions are not in order");

            var sorted = getSortedData(processes, "TransationDate", "asc");
            //check if status are in sequence
            int previousstatus = -1;
            foreach (var item in sorted)
            {
                if (previousstatus > (int)item.Status) throw new Exception("transaction dated " + item.ProcessingDate + " is out of sequence");
                previousstatus = (int)item.Status;
            }

            return processes;
        }

        // implement exceptions for emigration clearances for ecnr cnadidates
        private bool ProcessTransactionsAreNotMissing(List<Process> processes)
        {
            //get sequence from table
            List<int> thisList = processes.OrderBy(x => x.Status).Select(x => (int)x.Status).ToList();
            int f = processes.OrderBy(x => x.Status).Select(x => (int)x.Status).FirstOrDefault();
            int l = processes.OrderBy(x => x.Status).Select(x => (int)x.Status).Last();
            var thisRange = Enumerable.Range(f, l - f + 1).ToList();
            var missingSeq = thisList.Except(thisRange).ToList();

            return (missingSeq.Count == 0);
        }
        private ICollection<Process> getSortedData(ICollection<Process> collection, string property, string direction)
        {
            switch (direction.Trim())
            {
                case "asc":
                    collection = ((from n in collection
                                   orderby
                                   n.GetType().GetProperty(property).GetValue(n, null)
                                   select n).ToList<Process>()) as ICollection<Process>;
                    break;
                case "desc":
                    collection = ((from n in collection
                                   orderby
                                   n.GetType().GetProperty(property).GetValue(n, null)
                                   descending
                                   select n).ToList<Process>()) as ICollection<Process>;
                    break;
            }
            return collection;
        }

        //checks for each cvrefid if the new added transDate is later than previous transaction date
        //and also whether the new status is not earlier than previous status
        private async Task<List<Process>> ProcessTransasctionsVerifiedForAdd(DateTime transDate,
            enumProcessingStatus status, string remarks, int[] cvrefids, TravelToAddDto travel)
        {

            //var errList = new List<clsString>();
            var selProcessList = new List<Process>();

            DateTime temp;

            if (!DateTime.TryParse(transDate.ToString(), out temp))
                //errList.Add(new clsString("Invalid Date - " + transDate));
                throw new Exception("invalid date = " + transDate + " in ProcessTransactionsVerifiedForAdd");

            if (status == enumProcessingStatus.Selected)
                //errList.Add(new clsString("Selected status cannot be inserted manually - once a candidate is selected, this status is automaticlaly added to the system"));
                throw new Exception("selected status cannot be inserted manually - it is to be inserted by the system when selections are registered - Source:ProcessTransactionsVerifiedForAdd");

            if (status == enumProcessingStatus.TravelBooked)
            {
                if (travel == null)
                { throw new Exception("travel details not provided for travel booked status"); }
                else if (     
                    string.IsNullOrEmpty(travel.BoardingAirport) ||
                    string.IsNullOrEmpty(travel.DestinationAirport) ||
                    string.IsNullOrEmpty(travel.Airline) || string.IsNullOrEmpty(travel.FlightNo) ||
                    string.IsNullOrEmpty(travel.PNR))
                {
                    throw new Exception("For status of ticket booked, all travel details must be provided");
                }

                DateTime tempOut;
                if (!DateTime.TryParse(travel.ETD.ToString(), out tempOut) ||
                    !DateTime.TryParse(travel.ETA.ToString(), out tempOut))
                {
                    throw new Exception("invalid dates - ETA or ETD");
                }
            }
            foreach (var id in cvrefids)
            {
                var currentdata = await _context.Processes.Where(x => x.CVRefId == id)
                    .Select(x => new { x.Status, x.NextProcessingId, x.ProcessingDate })
                    .OrderByDescending(x => x.ProcessingDate).FirstOrDefaultAsync();
                if (currentdata == null) throw new Exception("For transaction process candidates need to be selected first - TransactionsVerifiedForAdd");

                if (currentdata.NextProcessingId == 0)
                {
                    throw new Exception("With last transaction of " +
                    Enum.GetName(typeof(enumProcessingStatus), currentdata.Status) +
                    ", process for this candidate is concluded - no further process avaialable");
                }
                else if (currentdata.NextProcessingId != status)
                {
                    //errList.Add(new clsString("next expected processing status is " +
                    //  Enum.GetName(typeof(enumProcessingStatus), currentdata.NextProcessingId)));
                    throw new Exception("next expected processing status is " +
                        Enum.GetName(typeof(enumProcessingStatus), currentdata.NextProcessingId) + " in ProcessTranactionsVerifiedForAdd");
                }
                else if (transDate <= currentdata.ProcessingDate)
                {
                    //errList.Add(new clsString("the transaction date " + transDate + " provided is earlier or equal to " +
                    //  "the last transaction date of " + currentdata.ProcessingDate));
                    throw new Exception("the transaction date " + transDate + " provided is earlier or equal to " +
                        "the last transaction date of " + currentdata.ProcessingDate + " in ProcessTransactionsVerifiedForAdd");
                }
            }

            foreach (var id in cvrefids)
            {
                var nextstatusid = GetNextStatus(id, status);
                selProcessList.Add(new Process(id, transDate, status, remarks, nextstatusid));
            }
            return selProcessList;
        }

        private enumProcessingStatus GetNextStatus(int cvrefid, enumProcessingStatus currentStatus)
        {
            enumProcessingStatus nextstatus;

            if (currentStatus == enumProcessingStatus.VisaReceivedEndorsed)
            {
                //check if candidate is ECNR
                var candid = _context.CVRefs.Where(x => x.Id == cvrefid).Select(x => x.CandidateId).SingleOrDefault();

                var ecnr = _context.Candidates.Where(x => x.Id == candid).Select(x => x.ECNR).SingleOrDefault();
                nextstatus = ecnr ? enumProcessingStatus.TravelBooked : enumProcessingStatus.EmigrationDocumentsSubmitted;
            }
            else
            {
                nextstatus = _context.ProcessStatuses.Where(x => x.StatusId == currentStatus).Select(x => x.NextStatusId).FirstOrDefault();
            }

            return nextstatus;
        }

    }
}