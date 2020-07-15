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
        Task<T> GetByIdAsync(int id);
        Task<IReadOnlyList<T>> ListAllAsync();

        Task<T> GetEntityWithSpec(ISpecification<T> spec);
        Task<Customer> GetCustomerFromEmailAsync(string email);     //not really generic, but...
        Task<IReadOnlyList<T>> ListWithSpecAsync(ISpecification<T> spec);
        Task<int> AddAsync(T entity);
        
        Task<int> CountWithSpecAsync(ISpecification<T> spec);

        Task<int> UpdateAsync(T entity);
        Task<int> UpdateListAsync(List<T> entities);
        
        Task<int> NextSequenceNoWithSpec(ISpecification<T> spec);

        void Add (T entity);
        void Update(T entity);
        void Delete (T entity);
    }
}