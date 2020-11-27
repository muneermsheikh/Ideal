using System.Collections.Generic;
using System;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Admin;
using Core.Entities.Masters;
using Core.Enumerations;
using Core.Interfaces;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDLService _dLService;
        private readonly ATSContext _context;
        private readonly IGenericRepository<Customer> _custRepo;

        public CustomerService(IUnitOfWork unitOfWork, IGenericRepository<Customer> custRepo, IDLService dLService, ATSContext context)
        {
            _custRepo = custRepo;
            _dLService = dLService;
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            var ListIndustryTypes = new List<CustomerIndustryType>();
            var ListOfficials = new List<CustomerOfficial>();
            var nm = "";
            
            foreach (var indType in customer.CustomerIndustryTypes)
            {
                nm = await _context.IndustryTypes.Where(x => x.Id == indType.IndustryTypeId).Select(x => x.Name).SingleOrDefaultAsync();
                if (string.IsNullOrEmpty(nm)) return null;

                ListIndustryTypes.Add(new CustomerIndustryType {
                    IndustryTypeId = indType.IndustryTypeId,
                    Name = nm
                });
            }

            foreach (var off in customer.CustomerOfficials)
            {
                ListOfficials.Add(new CustomerOfficial {
                    Gender = off.Gender,
                    Name = off.Name,
                    Designation = off.Designation,
                    Scope = off.Scope,
                    Phone = off.Phone,
                    Mobile = off.Mobile,
                    Mobile2 = off.Mobile2,
                    Email = off.Email,
                    PersonalMobile = off.PersonalMobile,
                    PersonalEmail = off.PersonalEmail,
                    IsValid = "T",
                    AddedOn = DateTime.Now
                });
            }

            Customer customerToAdd = new Customer{
                CustomerType = customer.CustomerType,
                CustomerName = customer.CustomerName,
                KnownAs = customer.KnownAs,
                IntroducedBy = customer.IntroducedBy,
                Email = customer.Email,
                Phone1 = customer.Phone1,
                Phone2 = customer.Phone2,
                CompanyUrl = customer.CompanyUrl,
                Description = customer.Description,
                Address1 = customer.Address1,
                Address2 = customer.Address2,
                City = customer.City,
                State = customer.State,
                Country = customer.Country,
                CustomerStatus = "Active",
                AddedOn = DateTime.Now,
                CustomerIndustryTypes = ListIndustryTypes,
                CustomerOfficials = ListOfficials
            };

            var cust = await _unitOfWork.Repository<Customer>().AddAsync(customerToAdd);
            // var result = await _unitOfWork.Complete();
            if (cust == null ) return null;
            return cust;
        }

        public async Task<Customer> CustomerByIdAsync(int customerId)
        {
            // var spec = new CustomerSpecs(customerId);
            return await _unitOfWork.Repository<Customer>().GetByIdAsync(customerId);
        }

        public async Task<IReadOnlyList<Customer>> CustomerListAsync(CustomerSpecParams sParams)
        {
            var spec = new CustomerSpecs(sParams);
            return await _unitOfWork.Repository<Customer>().ListWithSpecAsync(spec);
        }

        public async Task<int> DeleteCustomerByIdAsync(int customerId)
        {
            var cust = await _unitOfWork.Repository<Customer>().GetByIdAsync(customerId);
            return await _unitOfWork.Complete();
        }

        public async Task<int> GetCustomerIdFromEmail(CustomerSpecParams custParams)
        {
            var spec = new CustomerSpecs(custParams);
            var cust = await _unitOfWork.Repository<Customer>().GetEntityWithSpec(spec);
            if (cust == null) return 0;
            return cust.Id;
        }

        public async Task<Customer> UpdateCustomer(Customer customer)
        {
            var indTypesInDB = await _context.CustomerIndustryTypes
                .Where(x => x.CustomerId == customer.Id).ToListAsync();

            // identify records in DB that are missing in the customer Model, to delete it from DB
            var indTypeIdsInModel = customer.CustomerIndustryTypes.Select(x => x.Id);
            var indTypesToDelete = new List<CustomerIndustryType>();

            if (indTypeIdsInModel == null)
            {
                indTypesToDelete = await _context.CustomerIndustryTypes
                    .Where(x => x.CustomerId == customer.Id).ToListAsync();
            }
            else
            {
                indTypesToDelete = await _context.CustomerIndustryTypes
                    .Where(x => x.CustomerId == customer.Id && !indTypeIdsInModel.Contains(x.Id)).ToListAsync();
            }

            // delete the identified records from DB
            if (indTypesToDelete != null)
            {
                var deleted = await _unitOfWork.Repository<CustomerIndustryType>()
                    .DeleteListAsync(indTypesToDelete);
            }

            // identify records that exist both in DB and in model, so as to update same
            var indTypeIdsToUpdate = await _context.CustomerIndustryTypes
                .Where(x => indTypeIdsInModel.Contains(x.Id)).Select(x => x.Id).ToListAsync();
            var indTypesFromModelToUpdate = customer.CustomerIndustryTypes.Where(x => indTypeIdsToUpdate.Contains(x.Id)).ToList();

            await _unitOfWork.Repository<CustomerIndustryType>().UpdateListAsync(indTypesFromModelToUpdate);

            // filter out the records to be updated, else these records will be again INSERTED along with the customerEntity when it is updated
            // create another entity excluding records that hv been updated as above.
            var indTypeModelFiltered = customer.CustomerIndustryTypes.Where(x => !indTypeIdsToUpdate.Contains(x.Id)).ToList();
            foreach (var ind in indTypeModelFiltered)
            {
                if (ind.CustomerId == 0 || ind.CustomerId == '0')
                {
                    ind.CustomerId = customer.Id;
                }
            }

            // attach indTypeModelFiltered to customer model
            await _unitOfWork.Repository<CustomerIndustryType>().AddListAsync(indTypeModelFiltered);

            customer.CustomerIndustryTypes = null;

            return await _unitOfWork.Repository<Customer>().UpdateAsync(customer);
        }


        public async Task<Customer> GetCustomerFromEmailAsync(string email)
        {
            return await _unitOfWork.Repository<Customer>().GetCustomerFromEmailAsync(email);
        }

        public async Task<string> CustomerCountryCurrency(int customerId)
        {
            var country = await _context.Customers.Where(x => x.Id == customerId).Select(x => x.Country).SingleOrDefaultAsync();
            string currency="";
            if (!string.IsNullOrEmpty(country))
                {
                    switch (country.ToLower())
                    {
                        case "saudi arabia":
                        case "ksa":
                        case "k.s.a.":
                            currency = "SAR";
                            break;
                        case "uae":
                        case "u.a.e.":
                        case "united arab emirates":
                        case "dubai":
                        case "united arab emirate":
                            currency = "DHS";
                            break;
                        case "qatar":
                        case "state of qatar":
                            currency = "QAR";
                            break;
                        case "oman":
                            currency = "OR";
                            break;
                        case "kuwait":
                            currency ="KD";
                            break;
                        default:
                            break;
                    }
                }
            return currency;
        }
        //officials
        public async Task<IReadOnlyList<CustomerOfficial>> GetCustomerOfficialList(int CustomerId)
        {
            return await _context.CustomerOfficials.Where(x => x.Id == CustomerId && x.IsValid.ToLower() == "t")   
                .OrderBy(x => x.Scope).ToListAsync();
        }
        public async Task<IReadOnlyList<CustomerOfficial>> GetAllOfficialList()
        {
            return await _context.CustomerOfficials.OrderBy(x => x.CustomerId).ThenBy(x => x.Scope).ToListAsync();
        }
        public async Task<IReadOnlyList<CustomerOfficial>> InsertOfficials(List<CustomerOfficial> officials)
        {
            return await _unitOfWork.Repository<CustomerOfficial>().AddListAsync(officials);
        }

        public async Task<int> UpdateOfficials(List<CustomerOfficial> officials)
        {
            return await _unitOfWork.Repository<CustomerOfficial>().UpdateListAsync(officials);
        }

        public async Task<clsString> GetCustomerFromEnquiryItemId(int EnquiryItemId)
        {
            var enqid = await _context.EnquiryItems.Where(x => x.Id == EnquiryItemId)
                .Select(x => x.EnquiryId).SingleOrDefaultAsync();
            var custname = await _context.Enquiries.Where(x => x.Id == enqid)
                .Select(x => x.Customer.CustomerName + " " + x.Customer.City).SingleOrDefaultAsync();
            var r = new clsString();
            r.Name = custname;
            return r;
        }

        // recruitment agencies
        public async Task<IReadOnlyList<Customer>> GetAgencies()
        {
            return await _context.Customers
                .Where(x => x.CustomerType.ToLower() == "associate")
                .OrderBy(x => x.CustomerName)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Customer>> GetCustomerListFlat(string customerType)
        {
            return await _context.Customers.Where(x => x.CustomerType.ToLower() == customerType && 
                x.CustomerStatus.ToLower() == "active").ToListAsync();
        }

        public async Task<string> GetCustomerNameCityCountryFromId(int customerId)
        {
            var nm = await _context.Customers.Where(x => x.Id == customerId)
                .Select(x => new {Name = x.CustomerName, City = x.City, Country = x.Country}).SingleOrDefaultAsync();
            return nm.Name + "|" + nm.City + "|" + nm.Country;
        }
    }
}
