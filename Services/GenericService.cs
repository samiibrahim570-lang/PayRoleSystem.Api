using PayRoleSystem.Http;
using PayRoleSystem.Repositories.Interfaces;
using PayRoleSystem.Services.Interfaces;
using System.Linq.Expressions;
 
namespace PayRoleSystem.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly IGenericRepository<T> _repository;

        public GenericService(IGenericRepository<T> repository)
        {
            _repository = repository;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public virtual async Task<T> GetByIdAsync(object id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public virtual async Task<T> AddAsync(T entity)
        {
            return await _repository.AddAsync(entity);
        }

        public virtual async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            return await _repository.AddRangeAsync(entities);
        }

        public virtual async Task UpdateAsync(T entity)
        {
            await _repository.UpdateAsync(entity);
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            await _repository.UpdateRangeAsync(entities);
        }

        public virtual async Task DeleteByIdAsync(int id)
        {
            await _repository.DeleteByIdAsync(id);
        }

        public virtual async Task DeleteAsync(T entity)
        {
            await _repository.DeleteAsync(entity);
        }

        public virtual async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _repository.FindAsync(predicate);
        }

        public virtual async Task<PageResult<T>> GetPaginatedAsync(IPageRequest pageRequest)
        {
            return await _repository.GetPaginatedAsync(pageRequest);
        }
    }

}