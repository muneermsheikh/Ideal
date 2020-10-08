using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities.Masters;
using Core.Interfaces;
using Infrastructure.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

// this controller is for Users with Authority = AddMasterValues;
// another contorller CategoriesController is used by loggedin users, who are clients or candidates, and
// who have no authority to edit master values
namespace Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ATSContext _context;
        public CategoryService(IUnitOfWork unitOfWork, ATSContext context)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        public async Task<IReadOnlyList<Category>> CategoriesFromCategoryIds(int[] categoryIds)
        {
            return await _context.Categories.Where(x => categoryIds.Contains(x.Id)).OrderBy(x=>x.Id).ToListAsync();
        }

        public string GetCategoryNameWithRefFromEnquiryItemId(int enquiryItemId)
        {
            if (enquiryItemId == 0) return "invalid enquiry item id";

            //var catName = (from e in _context.EnquiryItems join c in _context.Categories
                           //on e.CategoryItemId equals c.Id select c.Name);
       
            /* var catNameWithRef = (from o in _context.Enquiries join e in _context.EnquiryItems 
                on o.Id equals e.EnquiryId join c in _context.Categories on e.CategoryItemId   
                equals c.Id select o.EnquiryNo + "-" + e.SrNo + "-" + c.Name).ToString();

            var catNameWithRef = (from t in db.Track join il in db.InvoiceLine
                        on t.TrackId equals il.TrackId join i in db.Invoice
                        on il.InvoiceId equals i.InvoiceId
                        where t.Name == "Bohemian Rhapsody"
                        select (new
                        {
                            TrackName = t.Name,
                            TrackId = t.TrackId,
                            InvoiceId = i.InvoiceId,
                            InvoiceDate = i.InvoiceDate,
                            Quantity = il.Quantity,
                            UnitPrice = il.UnitPrice
                        })
                    )
            */
            var rs = (from o in _context.Enquiries join e in _context.EnquiryItems
                        on o.Id equals e.EnquiryId join c in _context.Categories 
                        on e.CategoryItemId equals c.Id 
                        where e.Id == enquiryItemId
                        select (new { orderNo = o.EnquiryNo, srNo = e.SrNo, catName=c.Name}))
                        .FirstOrDefault();
                
            return rs.orderNo+"-"+rs.srNo+"-"+rs.catName;
        }
        public string GetCategoryNameFromCategoryId (int categoryId)
        {
            if (categoryId==0) return " invalid category id";
            var cat = _context.Categories.Where(x => x.Id == categoryId)
                .Select(x=>x.Name).FirstOrDefault();
            if (string.IsNullOrEmpty(cat)) return "invalid category id";
            return cat;
        }

        public string getCategoryNameFromCategoryId(int categoryId)
        {
            throw new System.NotImplementedException();
        }

        public async Task<Category> CategoryByIdAsync(int Id)
        {
            return await _unitOfWork.Repository<Category>().GetByIdAsync(Id);
        }

        public async Task<IReadOnlyList<Category>> CategoryListAsync()
        {
            return await _unitOfWork.Repository<Category>().ListAllAsync();
        }

        public async Task<Category> CreateCategoryAsync(string name, int indTypeId, int skillLevelId)
        {
            var cat = new Category(name, indTypeId, skillLevelId);
            return await _unitOfWork.Repository<Category>().AddAsync(cat);
            //var result = await _unitOfWork.Complete();
        }

        public async Task<Category> DeleteCategoryByIdAsync(int Id)
        {
            var cat = await _unitOfWork.Repository<Category>().GetByIdAsync(Id);
            if (cat == null) return null;

            _unitOfWork.Repository<Category>().Delete(cat);
            var result = await _unitOfWork.Complete();
            if (result == 0) return null;
            return cat;
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            // var cat = await _unitOfWork.Repository<Category>().GetByIdAsync(Id);
            _unitOfWork.Repository<Category>().Update(category);
            var result = await _unitOfWork.Complete();
            if (result == 0) return null;
            return category;
        }

        public async Task<bool> CategoryExists(string nm, int indId, int skId)
        {
            var cat = await _context.Categories
                .Where(x => x.Name == nm && x.IndustryTypeId == indId && x.SkillLevelId == skId)
                .SingleOrDefaultAsync();
            
            return (cat == null) ? true : false;
        }
        // industry types

        public async Task<IReadOnlyList<IndustryType>> GetIndustryTypes()
        {
            // return await _unitOfWork.Repository<IndustryType>().ListAllAsync();
            return await _context.IndustryTypes.OrderBy(x => x.Name).ToListAsync();
        }

        public async Task<IndustryType> CreateIndustryType(string name)
        {
            var indType = new IndustryType(name);
            return await _unitOfWork.Repository<IndustryType>().AddAsync(indType);
        }

        public async Task<IndustryType> UpdateIndustryType(IndustryType indType)
        {
            return await _unitOfWork.Repository<IndustryType>().UpdateAsync(indType);
        }

        public async Task<int> DeleteIndustryType(IndustryType indType)
        {
            return await _unitOfWork.Repository<IndustryType>().DeleteAsync(indType);
        }
        public async Task<IndustryType> DeleteIndustryTypeById(int Id)
        {
            var ind = await _unitOfWork.Repository<IndustryType>().GetByIdAsync(Id);
            if (ind == null) return null;

            _unitOfWork.Repository<IndustryType>().Delete(ind);
            var result = await _unitOfWork.Complete();
            if (result == 0) return null;
            return ind;
        }


    // skill Level
        public async Task<SkillLevel> CreateSkillLevel(string name)
        {
            var skillLvl = new SkillLevel(name);
            return await _unitOfWork.Repository<SkillLevel>().AddAsync(skillLvl);
        }

        public async Task<SkillLevel> UpdateSkillLevel(SkillLevel skLevel)
        {
            return await _unitOfWork.Repository<SkillLevel>().UpdateAsync(skLevel);
        }

        public async Task<int> DeleteSkillLevel(SkillLevel skLevel)
        {
            return await _unitOfWork.Repository<SkillLevel>().DeleteAsync(skLevel);
        }

        public async Task<IReadOnlyList<SkillLevel>> GetSkillLevels()
        {
            return await _context.SkillLevels.OrderBy(x => x.Name).ToListAsync();
        }


        public async Task<SkillLevel> DeleteSkillLevelById(int Id)
        {
            var skl = await _unitOfWork.Repository<SkillLevel>().GetByIdAsync(Id);
            if (skl == null) return null;

            _unitOfWork.Repository<SkillLevel>().Delete(skl);
            var result = await _unitOfWork.Complete();
            if (result == 0) return null;
            return skl;
        }
    }
}
