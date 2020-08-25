using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Entities.Admin;
using Core.Entities.EnquiryAggregate;
using Core.Specifications;

namespace Core.Interfaces
{
    public interface IGenericRepository<T> where T: BaseEntity
    {
        Task<bool> RecordExists(ISpecification<T> entity);
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<int> GetNextEnquiryNo();

        Task<T> GetEntityWithSpec(ISpecification<T> spec);
        Task<IReadOnlyList<T>> GetEntityListWithSpec(ISpecification<T> spec);
        
        Task<Customer> GetCustomerFromEmailAsync(string email);     //not really generic, but...
        Task<IReadOnlyList<T>> ListWithSpecAsync(ISpecification<T> spec);
        Task<IReadOnlyList<T>> ListTop500WithSpecAsync(ISpecification<T> spec);
        Task<T> AddAsync(T entity);
        Task<IReadOnlyList<T>> AddListAsync(IReadOnlyList<T> entities);
        
        Task<int> CountWithSpecAsync(ISpecification<T> spec);

        Task<T> UpdateAsync(T entity);
        Task<int> UpdateListAsync(List<T> entities);
        
        Task<int> DeleteAsync(T entity);
        
                
        Task<int> NextSequenceNoWithSpec(ISpecification<T> spec);

        void Add (T entity);
        void Update(T entity);
        void Delete (T entity);
    }
}