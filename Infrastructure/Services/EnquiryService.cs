using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.EnquiryAggregate;
using Core.Entities.Masters;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class EnquiryService : IEnquiryService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _unitOfWork;

        public EnquiryService(IUnitOfWork unitOfWork,
                IBasketRepository basketRepo)
        {
            _unitOfWork = unitOfWork;
            _basketRepo = basketRepo;
        }

        public async Task<Enquiry> CreateEnquiryAsync(string buyerEmail, string basketId, Address shippingAddress)
        {
            // get basket from basketRepo  -- this basket comes from Client, 
            // and brings in categoryId and quantity, nothing else is trusted by API

            var basket = await _basketRepo.GetBasketAsync(basketId);

            var enquiryItems = new List<EnquiryItem>();

            foreach (var item in basket.Items)
            {
                // not trusting basket from the client, categoryName reset to actual value from database
                var catItem = await _unitOfWork.Repository<Category>().GetByIdAsync(item.CategoryId);
                var catItemOrdered = new CategoryItemOrdered(catItem.Id, catItem.Name);
                var enqItem = new EnquiryItem(catItemOrdered, item.Quantity, item.ECNR,
                    item.ExpDesiredInYrsMin, item.ExpDesiredInYrsMax,
                    item.JobDescInBrief, item.JobDescAttachment, item.SalaryRangeMin,
                    item.SalaryRangeMax, item.ContractPeriodInMonths, item.Food,
                    item.Housing, item.Transport, item.DateRequiredBy);

                enquiryItems.Add(enqItem);
            }

            // get deliveryMethod

            // create Enquiry
            var enquiry = new Enquiry(enquiryItems, buyerEmail);
            _unitOfWork.Repository<Enquiry>().Add(enquiry);
            
            // save to Db
            var result = await _unitOfWork.Complete();      // all of changes in the transactons are applied
            if (result <= 0) return null;

            // delete basket
            await _basketRepo.DeleteBasketAsync(basketId);
            // return enquiry


            return enquiry;

        }

        public async Task<Enquiry> GetEnquiryById(int Id, string buyerEmail)
        {
            var spec = new EnquiriesWithItemsAndOrderingSpecs(Id, buyerEmail);
            return await _unitOfWork.Repository<Enquiry>().GetEntityWithSpec(spec);

        }

        public async Task<IReadOnlyList<Enquiry>> GetUserEnquiriesAsync(string buyerEmail)
        {
            var spec = new EnquiriesWithItemsAndOrderingSpecs(buyerEmail);
            return await _unitOfWork.Repository<Enquiry>().ListAsync(spec);
        }
    }
}