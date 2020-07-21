using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.Dtos;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Masters;
using Core.Enumerations;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class EnquiryService : IEnquiryService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Enquiry> _enqRepository;
        private readonly ATSContext _context;
        private readonly ICustomerService _customerService;
        private readonly IDLService _dlService;

        public EnquiryService(IUnitOfWork unitOfWork,
                IBasketRepository basketRepo, ATSContext context,
                ICustomerService customerService, IDLService dlService)
        {
            _dlService = dlService;
            _customerService = customerService;
            _context = context;
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
        }

    //Enq and Enq items
        public async Task<Enquiry> CreateEnquiryAsync(string buyerEmail, string basketId, SiteAddress shippingAddress)
        {
            // get basket from basketRepo  -- this basket comes from Client, 
            // and brings in categoryId and quantity, nothing else is trusted by API

            var basket = await _basketRepo.GetBasketAsync(basketId);
            if (basket == null) return null;

            //  Get CustomerId from buyerEmail
            int customerId = GetCustomerIdFromLoggedInemail(buyerEmail, shippingAddress).Result;
            if (customerId == 0) return null;       // never happens

            var enq = new Enquiry(customerId, buyerEmail);
            var enquiryItems = new List<EnquiryItem>();

            // int newOrderNo = async _unitOfWork.Repository<Enquiry>.Select(p => p.Age).Cast<int?>().Max() ?? 0;
            foreach (var item in basket.Items)
            {
                // not trusting basket from the client, categoryName reset to actual value from database
                var catItem = await _unitOfWork.Repository<Category>().GetByIdAsync(item.CategoryId);
                var catItemOrdered = new CategoryItemOrdered(catItem.Id, catItem.Name);

                var enqItem = new EnquiryItem(catItemOrdered, item.Quantity, item.ECNR,
                    item.ExpDesiredInYrsMin, item.ExpDesiredInYrsMax,
                    item.JobDescInBrief, item.JobDescAttachment, item.SalaryRangeMin,
                    item.SalaryRangeMax, item.ContractPeriodInMonths, item.Food,
                    item.Housing, item.Transport, item.CompleteBy);

                enquiryItems.Add(enqItem);
            }

            // get deliveryMethod

            // create Enquiry
            var enquiry = new Enquiry(enquiryItems, buyerEmail);
            _unitOfWork.Repository<Enquiry>().Add(enquiry);

            // save to Db
            var result = await _unitOfWork.Complete();      // all of changes in the transactons are applied
            if (result <= 0) return null;

            // add JobDesc and Remuneration for each enquiryItem

            // delete basket
            await _basketRepo.DeleteBasketAsync(basketId);
            // return enquiry

            return enquiry;

        }

        public async Task<Enquiry> GetEnquiryById(int Id, string buyerEmail)
        {
            var spec = new EnquirySpecs(Id, buyerEmail);
            var enq = await _unitOfWork.Repository<Enquiry>().GetEntityWithSpec(spec);
            // var result = _unitOfWork.Complete();
            return (enq);
        }

        public async Task<Enquiry> GetEnquiryByIdAsync(int enquiryId)
        {
            return await _unitOfWork.Repository<Enquiry>().GetByIdAsync(enquiryId);
        }

        public async Task<IReadOnlyList<Enquiry>> GetUserEnquiriesAsync(string buyerEmail)
        {
            var spec = new EnquirySpecs(buyerEmail);
            var enqs = await _unitOfWork.Repository<Enquiry>().ListWithSpecAsync(spec);
            // await _unitOfWork.Complete();
            return enqs;
        }

        public async Task<int> GetEnquiryItemsCountNotReviewed(int enquiryId)
        {
            var spec = new EnquiryItemsSpecs(enquiryId, (int)enumItemReviewStatus.NotReviewed);
            return await _unitOfWork.Repository<EnquiryItem>().CountWithSpecAsync(spec);
        }

        public async Task<EnquiryItem> GetEnquiryItemByIdAsync(int enquiryItemId)
        {
            return await _unitOfWork.Repository<EnquiryItem>().GetByIdAsync(enquiryItemId);
        }

        public async Task<bool> UpdateEnquiryReadyToReview(Enquiry enq)
        {
            enq.ReadyToReview = true;

            var x = await _unitOfWork.Repository<Enquiry>().UpdateAsync(enq);

            if (x == null) return false;

            // create task for the Project Manager

            var todo = new ToDo(
                enq.ProjectManager.Id, enq.ProjectManager.Id, enq.ReviewedOn,
                enq.ReviewedOn.AddDays(4), "Following task assigned to you",
                enumTaskType.HRDeptHeadAssignment, enq.Id, 0
            );

            _unitOfWork.Repository<ToDo>().Add(todo);
            var result = await _unitOfWork.Complete();   //this will also trigger email message from database prvider   
            return (result > 0);
        }


    // job desc
        public async Task<JobDesc> GetJobDescriptionBySpecAsync(int enquiryItemId)
        {
            var spec = new JobDescSpec(enquiryItemId);
            var jd = await _unitOfWork.Repository<JobDesc>().GetEntityWithSpec(spec);

            if (jd == null)
            {
                var enqItem = await _unitOfWork.Repository<EnquiryItem>().GetByIdAsync(enquiryItemId);
                int enquiryId = enqItem.EnquiryId;
                jd = new JobDesc(enquiryItemId, enquiryId);
                await _context.SaveChangesAsync();
            }
            return jd;
        }

        public async Task<JobDesc> UpdateJDAsync(JobDesc jobDesc)
        {
            return await _unitOfWork.Repository<JobDesc>().UpdateAsync(jobDesc);
        }


    // remuneration
        public async Task<Remuneration> GetRemunerationBySpecEnquiryItemIdAsync(int enquiryItemId)
        {
            var spec = new RemunerationSpecs(enquiryItemId);
            var remun = await _unitOfWork.Repository<Remuneration>().GetEntityWithSpec(spec);

            if (remun == null)
            {
                var enqItem = await _unitOfWork.Repository<Remuneration>().GetByIdAsync(enquiryItemId);
                int enquiryId = enqItem.EnquiryId;
                remun = new Remuneration(enquiryItemId, enquiryId);
                await _context.SaveChangesAsync();
            }
            return remun;
        }

        public async Task<Remuneration> UpdateRemunerationAsync(Remuneration remuneration)
        {
            return await _unitOfWork.Repository<Remuneration>().UpdateAsync(remuneration);
        }


    // contract review
        public async Task<ContractReviewItem> GetContractReviewItemAsync(int enquiryItemId)
        {
            var spec = new ContractReviewItemSpec(enquiryItemId);
            var rvw = await _unitOfWork.Repository<ContractReviewItem>().GetEntityWithSpec(spec);

            if (rvw == null)
            {
                var enqItem = await _unitOfWork.Repository<EnquiryItem>().GetByIdAsync(enquiryItemId);
                int enquiryId = enqItem.EnquiryId;
                
                rvw = new ContractReviewItem(enquiryItemId, enquiryId);
                await _context.SaveChangesAsync();
            }
            return rvw;
        }

        public async Task<ContractReviewItem> UpdateContractReviewItemAsync(ContractReviewItem contractReviewItem)
        {
            return await _unitOfWork.Repository<ContractReviewItem>().UpdateAsync(contractReviewItem);
        }


    //PRIVATE METHODS
        // if a customer with email exists, get the entity and return its Id
        // else, insert the entity and gets its Id
        private async Task<int> GetCustomerIdFromLoggedInemail(string email, SiteAddress siteAddress)
        {
            var cust = await _unitOfWork.Repository<Customer>().GetCustomerFromEmailAsync(email);
            if (cust == null)
            {
                // insert customer
                string sCustomerName = siteAddress.FirstName;
                string sCity = siteAddress.City;

                cust = new Customer(enumCustomerType.Customer, sCustomerName, sCustomerName, sCity, email);
                await _unitOfWork.Repository<Customer>().AddAsync(cust);
            }
            return cust.Id;
        }

    }
}