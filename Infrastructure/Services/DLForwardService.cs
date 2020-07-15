using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.Dtos;
using Core.Entities.EnquiryAggregate;
using Core.Enumerations;
using Core.Interfaces;
using Core.Specifications;

namespace Infrastructure.Services
{
    public class DLForwardService : IDLForwardService
    {
        private readonly IDLService _dlService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDLForwardService _dlForwardService;
        public DLForwardService(IDLService dlService, IUnitOfWork unitOfWork, IDLForwardService dlForwardService)
        {
            _dlForwardService = dlForwardService;
            _unitOfWork = unitOfWork;
            _dlService = dlService;
        }

        public Task<IReadOnlyList<EnquiryForwarded>> CreateEnquiryForwardForSelectedEnquiryItemsAsync(
            IReadOnlyList<CustOfficialToForwardDto> officialsDto, IReadOnlyList<EnquiryItem> enquiryItems, 
            string mode, DateTimeOffset dtForwarded)
        {
            var forwardedList = EnquiryForwards(dtForwarded, enquiryItems, officialsDto, mode);
            return forwardedList;
        }

        public Task<IReadOnlyList<EnquiryForwarded>> CreateEnquiryForwardForEnquiryIdAsync(
            IReadOnlyList<CustOfficialToForwardDto> officialsDto, int enquiryId, string mode, DateTimeOffset dtForwarded)
        {
            IReadOnlyList<EnquiryItem> items = (IReadOnlyList<EnquiryItem>)_dlService
                .GetDLItemsAsync(enquiryId, enumItemReviewStatus.Accepted);
            
            var forwardedList = EnquiryForwards(dtForwarded, items, officialsDto, mode);
            return forwardedList;
        }


        public async Task<int> DeleteEnquiryItemForwardedByIdAsync(EnquiryForwarded enquiryForwarded)
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
            EnqForwardSpecParams enqFwdParams)
        {
            var spec = new EnqForwardedWithFilterSpec(enqFwdParams);
            return await _unitOfWork.Repository<EnquiryForwarded>().ListWithSpecAsync(spec);
        }

        public async Task<int> DeleteEnquiryItemsForwardedById(EnquiryForwarded enquiryForwarded)
        {
            _unitOfWork.Repository<EnquiryForwarded>().Delete(enquiryForwarded);
            var result = await _unitOfWork.Complete();
            return result;
        }


        private async Task<IReadOnlyList<EnquiryForwarded>> EnquiryForwards(
            DateTimeOffset dtForwarded, IReadOnlyList<EnquiryItem> items,
            IReadOnlyList<CustOfficialToForwardDto> customerOfficials,
            string forwardedByMode)
        {

            var fwdList = new List<EnquiryForwarded>();

            foreach (var off in customerOfficials)
            {
                foreach (var item in items)
                {
                    var add = forwardedByMode == "mail" ? off.email : forwardedByMode == "sms" ? off.Mobile : off.Mobile2;

                    var fwd = new EnquiryForwarded(dtForwarded, off.CustomerId, off.CustomerOfficialId, item.Id,
                        item.EnquiryId, forwardedByMode, add);

                    fwdList.Add(fwd);
                }
            }

            await _unitOfWork.Repository<EnquiryForwarded>().UpdateListAsync(fwdList);
            var result = await _unitOfWork.Complete();      // database trigger will compose  and optionally send messages
            if (result == 0) return null;

            return fwdList;
        }
    }
}