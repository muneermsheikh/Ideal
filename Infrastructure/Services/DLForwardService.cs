using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Masters;
using Core.Enumerations;
using Core.Interfaces;
using Core.Specifications;
using Core.Entities.Emails;
using System.Linq;
using Infrastructure.Data;

namespace Infrastructure.Services
{
    public class DLForwardService : IDLForwardService
    {
        private readonly IDLService _dlService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly ATSContext _context;

        public DLForwardService(IDLService dlService, IUnitOfWork unitOfWork, IEmailService emailService, ATSContext context)
        {
            _context = context;
            _emailService = emailService;
            _unitOfWork = unitOfWork;
            _dlService = dlService;
        }

        public async Task<DLForwarded> DLForwardToHRAsync(DateTime dtForwarded,
            IReadOnlyList<IdInt> enquiryIds, int iHRManagerId)
        {
            var dlForwarded = new DLForwarded();
            var _empRepo = _unitOfWork.Repository<Employee>();
            var enqs = new List<Enquiry>();
            var _repoCust = _unitOfWork.Repository<Customer>();
            var _enqRepo = _unitOfWork.Repository<Enquiry>();

            foreach (var enqId in enquiryIds)
            {
                var enq = await _unitOfWork.Repository<Enquiry>()
                    .GetEntityWithSpec(new EnquirySpecs(enqId.Id, "Accepted", false, false));
                if (enq == null) continue;

                var enqItems = await _unitOfWork.Repository<EnquiryItem>().GetEntityListWithSpec(
                    new EnquiryItemsSpecs(enqId.Id, "Accepted"));
                if (enqItems.Count == 0) continue;
                enq.EnquiryItems =(List<EnquiryItem>)enqItems;
                if (enq.Customer == null) enq.Customer = await _repoCust.GetByIdAsync(enq.CustomerId);
                int projectManagerId = enq.ProjectManager == null ? 1 : enq.ProjectManager.Id;

                var mgr = await _empRepo.GetByIdAsync(projectManagerId);
                var emp = await _empRepo.GetByIdAsync(iHRManagerId);

                var sTaskDescription = ComposeAndSendDLForwardMessageToHR(enq, emp, mgr);

                //var fwd = new DLForwardToHR(iHRManagerId, dtForwarded, enqId.Id);
                //var dlFwd = await _unitOfWork.Repository<DLForwardToHR>().AddAsync(fwd);

                //if (dlFwd == null) continue;

                enqs.Add(enq);

                var toDo = new ToDo(projectManagerId, iHRManagerId, dtForwarded, dtForwarded.AddDays(7),
                    sTaskDescription, "HRDeptHeadAssignment", enqId.Id);

                await _unitOfWork.Repository<ToDo>().AddAsync(toDo);

            }
            dlForwarded.Enquiries = enqs;

            return dlForwarded;
        }

        public async Task<IReadOnlyList<EnquiryForwarded>> DLForwardToAssociatesAsync(
            IReadOnlyList<IdInt> customerOfficialIds, int enquiryId,
            IReadOnlyList<IdInt> enqItemIds, string mode, DateTime dtForwarded)
        {
            var enqFwdd = await ForwardTheEnquiry(dtForwarded, enquiryId, enqItemIds, customerOfficialIds, mode);
            return enqFwdd;
        }

        public async Task<int> eleteEnquiryItemForwardedByIdAsync(EnquiryForwarded enquiryForwarded)
        {
            _unitOfWork.Repository<EnquiryForwarded>().Delete(enquiryForwarded);
            return await _unitOfWork.Complete();
        }

        public async Task<int> UpdateEnquiryItemForwardedAsync(EnquiryForwarded enquiryForwarded)
        {
            await _unitOfWork.Repository<EnquiryForwarded>().UpdateAsync(enquiryForwarded);
            return await _unitOfWork.Complete();
        }

        public async Task<IReadOnlyList<EnquiryForwarded>> GetEnquiriesForwardedForAnEnquiry(
            EnquiryForwardedParams enqFwdParams)
        {
            var spec = new EnquiryForwardedSpecs (enqFwdParams);
            return await _unitOfWork.Repository<EnquiryForwarded>().ListWithSpecAsync(spec);
        }

        public async Task<int> DeleteEnquiryItemForwardedByIdAsync(EnquiryForwarded enquiryForwarded)
        {
            _unitOfWork.Repository<EnquiryForwarded>().Delete(enquiryForwarded);
            var result = await _unitOfWork.Complete();
            return result;

        }


        /// privates
        private async Task<IReadOnlyList<EnquiryForwarded>> ForwardTheEnquiry(
            DateTime dtForwarded, int enqId, IReadOnlyList<IdInt> enqItemIds,
            IReadOnlyList<IdInt> officialIds, string forwardedByMode)
        {
            if (officialIds == null) return null;

            var officials = await _unitOfWork.Repository<CustomerOfficial>().
                GetEntityListWithSpec(new CustomerOfficialsSpecs(officialIds, "t"));

            var forwardedList = new List<EnquiryForwarded>();

            Enquiry enq = await _unitOfWork.Repository<Enquiry>()
                .GetEntityWithSpec(new EnquirySpecs(enqId, "Accepted", false, false));

            var enqForwardedItemsList = new List<EnquiryItemForwarded>();   //to write to child table
                                                                            //EnquiryItemForwarded, for each parent Id of EnquiryForwarded (EnquiryForwrds in db)
            var enqFwdItemToAdd = new EnquiryItemForwarded();

            var enqItems = await _unitOfWork.Repository<EnquiryItem>().GetEntityListWithSpec(
                    new EnquiryItemsSpecs(0, false));

            if (enqItemIds == null || enqItemIds.Count == 0)   // get items from db
            {
                enqItems = await _unitOfWork.Repository<EnquiryItem>()
                    .GetEntityListWithSpec(new EnquiryItemsSpecs(
                        enqId, "Accepted"));
                if (enqItems == null || enqItems.Count == 0) return null;

                foreach (var item in enqItems)
                {
                    //                    enqFwdItemToAdd.EnquiryItemId=item.Id;
                    enqForwardedItemsList.Add(
                        new EnquiryItemForwarded { EnquiryItemId = item.Id });
                }
            }
            else
            {
                // get an entity based upon enqId and selected enquiry item Ids.  Any Ids that
                // do not match the EnquiryId or are not contract reviewed.Accepted
                // will be excluded
                enqItems = await _unitOfWork.Repository<EnquiryItem>()
                .GetEntityListWithSpec(new EnquiryItemsSpecs(enqItemIds,
                    enqId, "Accepted", false));
                if (enqItems == null || enqItems.Count == 0) return null;

                foreach (var i in enqItemIds)
                {
                    enqFwdItemToAdd.EnquiryItemId = i.Id;
                    enqForwardedItemsList.Add(enqFwdItemToAdd);
                }
            }

            foreach (var off in officials)
            {
                var add = forwardedByMode == "mail" ? off.Email : forwardedByMode == "sms" ? off.Mobile : off.Mobile2;

                var fwd = new EnquiryForwarded(dtForwarded, off.CustomerId, off.Id,
                    enqId, forwardedByMode, add, enqForwardedItemsList);

                //var forwarded = await _unitOfWork.Repository<EnquiryForwarded>().UpdateAsync(fwd);
                var forwarded = await _unitOfWork.Repository<EnquiryForwarded>().AddAsync(fwd);

                if (forwarded == null) continue;
                forwardedList.Add(forwarded);
            }

            // ComposeDLForwardMessage needs List<EnquiryItem> entity to retrieve field values
            var cust = await _unitOfWork.Repository<Customer>().GetByIdAsync(enq.CustomerId);
            if (cust == null) return null;
            var emp = await _unitOfWork.Repository<Employee>().GetByIdAsync(enq.ProjectManagerId);
            if (emp == null) return null;

            ComposeAndSendDLForwardMessageToAssociates(enq, enqItems, cust, emp, officials);

            return forwardedList;
        }


        private string ComposeAndSendDLForwardMessageToHR(Enquiry enq,
            Employee emp, Employee mgr)
        {
            var enqNumber = enq.EnquiryNo;
            var enqDate = enq.EnquiryDate.Date;
            var CustomerName = enq.Customer.CustomerName;
            var CustomerCity = enq.Customer.City;

            var assignedToId = emp.Id;
            var assignedToNameAndDesignation =
                emp.Gender == "M" ? "Mr." : "Ms." + emp.FullName + ", " +
                Environment.NewLine + emp.Designation;
            var assignedToemailId = emp.Email;
            var assignedToMobile = emp.Mobile;

            var ownerId = mgr.Id;
            var ownerNameAndDesignation =
                mgr.Gender == "M" ? "Mr." : "Ms." + mgr.FullName + ", " + Environment.NewLine + mgr.Designation;
            var ownerEmailId = mgr.Email;
            var ownerMobile = mgr.Mobile;

            var requirementTable = GetEnquiryItemTable(enq.EnquiryItems);

            var sTaskDescription = "To:" + Environment.NewLine + assignedToNameAndDesignation +
                Environment.NewLine + "email: " + assignedToemailId + ", Mobile:" + assignedToMobile +
                Environment.NewLine + Environment.NewLine +
                "Following task has been assigned to you.  Please organize submission of " +
                "compliant CVs within the time allowed.  Job Description and remuneration terms are available " +
                "online" + Environment.NewLine + Environment.NewLine +
                "Order No.:" + enqNumber + " dated " + enqDate + Environment.NewLine +
                "Customer: " + CustomerName + " Employment City: " + CustomerCity +
                Environment.NewLine + "Regards/" + Environment.NewLine + Environment.NewLine +
                ownerNameAndDesignation + Environment.NewLine + "Mobile: " + ownerMobile +
                "The demand letter items are as follows:" + Environment.NewLine + Environment.NewLine +
                requirementTable;

            return sTaskDescription;
        }

        private int ComposeAndSendDLForwardMessageToAssociates(Enquiry enq,
            IReadOnlyList<EnquiryItem> enqItems, Customer cust, Employee ProjectManager,
            IReadOnlyList<CustomerOfficial> custOfficials)
        {
            var ListMessages = new List<string>();      // to store messages for each associate

            var enqNumber = enq.EnquiryNo;
            var enqDate = enq.EnquiryDate.Date;
            var CustomerName = cust.CustomerName;
            var CustomerCity = cust.City;
            var ownerId = ProjectManager.Id;
            var ownerNameAndDesignation = ProjectManager.Gender == "M" ? "Mr." : "Ms." +
                ProjectManager.FullName + ", " + Environment.NewLine + ProjectManager.Designation;
            var ownerEmailId = ProjectManager.Email;
            var ownerMobile = ProjectManager.Mobile;

            var requirementTable = GetEnquiryItemTable(enqItems);

            foreach (var off in custOfficials)
            {
                var assignedToEmailId = new List<string>();
                var assignedToId = off.Id;
                var assignedToNameAndDesignation =
                    off.Gender == "M" ? "Mr." : "Ms." + off.Name + ", " +
                    Environment.NewLine + off.Designation;
                assignedToEmailId.Add(off.Email);
                var assignedToMobile = off.Mobile;

                var sTaskDescription = "To:" + Environment.NewLine + assignedToNameAndDesignation +
                    Environment.NewLine + "email: " + assignedToEmailId + ", Mobile:" + assignedToMobile +
                    Environment.NewLine + Environment.NewLine +
                    "If you know of candidates that are interested in overseas employment and match the following " +
                    "criteria, please forward their CVs to us.  For interested candidate's reference, their job  " +
                    "Job Description is available <b>here</b> and remuneration terms are available <b>here</b>a" +
                    "online" + Environment.NewLine + Environment.NewLine +
                    "While applying, please ask candidates to refer to Order No.:" + enqNumber + " dated " + enqDate + Environment.NewLine +
                    "Customer: " + CustomerName + " Employment City: " + CustomerCity +
                     Environment.NewLine +
                    Environment.NewLine + "Regards/" + Environment.NewLine + Environment.NewLine +
                    ownerNameAndDesignation + Environment.NewLine + "Mobile: " + ownerMobile +
                    "The demand letter items are as follows:" + Environment.NewLine + Environment.NewLine +
                    Environment.NewLine + "category description" + Environment.NewLine +
                    requirementTable;

                ListMessages.Add(sTaskDescription);
                var emailCC = new List<string>();
                var emailBCC = new List<string>();
                var email = new EmailModel(assignedToNameAndDesignation, assignedToEmailId,
                    emailCC, emailBCC, "Invitation for your friends to take part for overseas employment opportunity" +
                    " in " + CustomerCity, sTaskDescription);
                //_emailService.SendEmail(email);
            }

            return ListMessages.Count;
        }

        private string GetEnquiryItemTable(IReadOnlyList<EnquiryItem> enqItems)
        {

            var st = "<table>" +
                        "<tr>" +
                            "<th>Sr.No.</th>" +
                            "<th>Category</th>" +
                            "<th>Quantity</th>" +
                            "<th>Job Description</th>" +
                            "<th>Remuneration</th>" +
                        "</tr>";
            foreach (var item in enqItems)
            {
                st = st + "<tr>" +
                    "<td>" + item.SrNo + "</td>" +
                    "<td>" + item.CategoryName + "</td>" +
                    "<td>" + item.Quantity + "</td>" +
                    "<td> click for job desc</td>" +
                    "<td> click for remuneration</td>" +
                    "</tr>";
            }
            st = st + "</table>";
            return st;
        }

    }
}