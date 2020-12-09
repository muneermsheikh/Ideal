using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly ATSContext _context;
        public GenericRepository(ATSContext context)
        {
            _context = context;
        }


        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<int> GetNextEnquiryNo()
        {
            var TopEnqNo = await _context.Enquiries.OrderByDescending(x => x.EnquiryNo)
                .Select(x => x.EnquiryNo).Take(1).FirstOrDefaultAsync();
            return TopEnqNo;
        }
                
        public async Task<IReadOnlyList<T>> ListWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<IReadOnlyList<T>> ListTop500WithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).Take(500).ToListAsync();
        }
        
        public async Task<T> AddAsync(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Added;
            var result = await _context.SaveChangesAsync();
            if (result != 0) return entity;
            return null;
        }

        public async Task<IReadOnlyList<T>> AddListAsync(IReadOnlyList<T> entities)
        {
            foreach(var t in entities)
            {
                _context.Set<T>().Attach(t);
                _context.Entry(t).State = EntityState.Added;
            }
            var result = await _context.SaveChangesAsync();
            if (result != 0) return entities;
            return null;
        }

        public async Task<T> GetEntityWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> GetEntityListWithSpec(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).ToListAsync();
        }

        public async Task<int> DeleteAsync(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Deleted;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteListAsync(List<T> entities)
        {
            foreach(var item in entities)
            {
                _context.Set<T>().Attach(item);
                _context.Entry(item).State=EntityState.Deleted;
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }
        
        public async Task<T> UpdateWithNoTrackingAsync(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> UpdateListAsync(List<T> entities)
        {
            foreach(var item in entities)
            {
                _context.Set<T>().Attach(item);
                _context.Entry(item).State = EntityState.Modified;
            }
            return await _context.SaveChangesAsync();
        }
        public async Task<int> CountWithSpecAsync(ISpecification<T> spec)
        {
            return await ApplySpecification(spec).CountAsync();
        }

        public async Task<int> NextSequenceNoWithSpec(ISpecification<T> spec)
        {
            var x = await ApplySpecification(spec).MaxAsync();
            int i = Convert.ToInt32(x) +1;
            return i;
        }
        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }

  
        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void Update(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<Customer> GetCustomerFromEmailAsync(string email)
        {
            return await _context.Customers.FirstOrDefaultAsync(x => x.Email == email);

        }

        public async Task<bool> RecordExists(ISpecification<T> entity)
        {
            return await _context.Set<T>().AnyAsync();
        }
    }
}