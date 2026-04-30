using PayRoleSystem.Http;
using PayRoleSystem.Models.PayRoleSystem.Models;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace PayRoleSystem.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task DeleteRange(IEnumerable<T> entities);
       
        IQueryable<Sale> GetQueryable();
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllsAsync(Expression<Func<T, bool>> predicate = null);
        Task<IEnumerable<T>> GetAllsForInculdeAsync(
    Expression<Func<T, bool>> predicate = null,
    Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null);
        Task<T> GetByIdAsync(object id);
        Task<List<T>> GetByIdsAsync(IEnumerable<int> ids);
        Task<T> GetByGlobalIdAsync(Guid globalId);
        Task<T> AddAsync(T entity);
        Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateRangeAsync(IEnumerable<T> entities);
        Task DeleteAsync(T entity);
        Task<IEnumerable<T>> GetAllsForAsync(Expression<Func<T, bool>>? predicate = null);
        Task DeleteRangeAsync(IEnumerable<T> entities);
        Task SoftDeleteAsync(Guid globalId);
        Task PermanentDeleteAsync(Guid globalId);
        Task PermanentDeleteAsyncById(int Id,string columnName);
        Task DeleteById(int Id);
        Task DeleteByIdAsync(int globalId);
        Task DeleteByIdd(int id);

        //Task DeleteByIdAsync<TEntity, TId>(TId id);

        Task PermanentDeleteRangeAsync(IEnumerable<T> entities);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<PageResult<T>> GetPaginatedAsync(IPageRequest pageRequest);
        //Task<ResponseModel<PageResult<T>>> SimpleSearchEntities(string searchTerm, int page, int pageSize);
        Task<ResponseModel<PageResult<T>>> SimpleSearchEntities(string searchTerm, int page, int pageSize);

        Task HardDeleteAsync(T entity);
    }


}
