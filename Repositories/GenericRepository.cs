using PayRoleSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Dapper;
using Npgsql;
using PayRoleSystem.Http;
using AutoMapper;
using PayRoleSystem.Models.PayRoleSystem.Models;
using PayRoleSystem.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Query;

namespace PayRoleSystem.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;
        private readonly IMapper _mapper;

        public GenericRepository(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _dbSet = _context.Set<T>();
            _mapper = mapper;
        }
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            // Only retrieve records where IsDeleted is false (i.e., active records)
            return await _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false).ToListAsync();
        }

        // Repository method (generic)
        public async Task<IEnumerable<T>> GetAllsAsync(Expression<Func<T, bool>> predicate = null)
        {
            var query = _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false);

            if (predicate != null)
                query = query.Where(predicate);

            return await query.ToListAsync();
        }


        public async Task<IEnumerable<T>> GetAllsForAsync(Expression<Func<T, bool>>? predicate = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if (predicate != null)
                query = query.Where(predicate);

            return await query.AsNoTracking().ToListAsync();
        }


        public async Task<IEnumerable<T>> GetAllsForInculdeAsync(
        Expression<Func<T, bool>> predicate = null,
        Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null)
        {
            IQueryable<T> query = _dbSet.Where(e => EF.Property<bool>(e, "IsDeleted") == false);

            if (predicate != null)
                query = query.Where(predicate);

            if (include != null)
                query = include(query);

            return await query.ToListAsync();
        }


        public async Task<T> GetByIdAsync(object id)
        {
            return await _dbSet
                .AsNoTracking()
                .FirstOrDefaultAsync(e => EF.Property<object>(e, "Id") == id && EF.Property<bool>(e, "IsDeleted") == false);
        }
        public async Task<List<T>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.Set<T>()
                .Where(e => ids.Contains((int)e.GetType().GetProperty("Id").GetValue(e)))
                .ToListAsync();
        }
        public async Task DeleteByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteByIdd(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            if (entity != null)
            {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public IQueryable<Sale> GetQueryable()
        {
            return _context.Set<Sale>();
        }
        public async Task<T> GetByGlobalIdAsync(Guid globalId)
        {
            return await _dbSet
                .Where(e => EF.Property<Guid>(e, "GlobalId") == globalId &&
                            EF.Property<bool>(e, "IsDeleted") == false)
                .FirstOrDefaultAsync();
        }
        public async Task<T> AddAsync(T entity)
            {
            await _dbSet.AddAsync(entity);
            //await _context.SaveChangesAsync();
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception and provide detailed error response
                Console.WriteLine(ex.Message);
                throw new Exception("Error saving changes to the database.");
            }



            return entity;
        }
        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }
         
        public async Task UpdateAsync(T entity)
        {
            try
            {
                Console.WriteLine("UpdateAsync method invoked");
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    Console.WriteLine("Entity is detached; attaching");
                    _dbSet.Attach(entity);
                }
                _context.Entry(entity).State = EntityState.Modified;
                Console.WriteLine("Entity marked as modified");
                await _context.SaveChangesAsync();
                Console.WriteLine("Changes saved");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in UpdateAsync: {ex.Message}");
                throw;
            }
        }


        //public void Update(T entity)
        //{
        //    if (_context.Entry(entity).State == EntityState.Detached)
        //        _dbSet.Attach(entity);
        //    _context.Entry(entity).State = EntityState.Modified;
        //    // Do NOT call SaveChangesAsync here
        //}

        public void Update(T entity)
        {
            if (_context.Entry(entity).State == EntityState.Detached)
                _dbSet.Attach(entity);

            _context.Entry(entity).State = EntityState.Modified;
            // ❌ Don't call SaveChangesAsync here
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                Update(entity);
            }
        }

        public async Task UpdateRangeAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().UpdateRange(entities);
            await _context.SaveChangesAsync();
        }
        public async Task SoftDeleteAsync(Guid globalId)
        {
            var entity = await GetByGlobalIdAsync(globalId);
            if (entity == null)
            {
                throw new ArgumentException("Entity not found");
            }

            var isDeletedProperty = entity.GetType().GetProperty("IsDeleted");
            var isActiveProperty = entity.GetType().GetProperty("IsActive");

            if (isDeletedProperty != null)
            {
                isDeletedProperty.SetValue(entity, true);
            }

            if (isActiveProperty != null)
            {
                isActiveProperty.SetValue(entity, false);
            }

            await _context.SaveChangesAsync();
        }
         
        public async Task PermanentDeleteAsync(Guid globalId)
        {
            var entity = await GetByGlobalIdAsync(globalId);
            if (entity == null)
            {
                throw new ArgumentException("Entity not found");
            }

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }

        // Permanent Delete by Id
        public async Task PermanentDeleteAsyncById(int id, string columnName)
        {
            try
            {
                var entities = await _dbSet
                    .Where(e => EF.Property<int>(e, columnName) == id)
                    .ToListAsync();

                if (entities.Any())
                {
                    _dbSet.RemoveRange(entities);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during delete: {ex.Message}");
                throw;
            }
        }
         
        // Generic delete method in the repository
        public async Task DeleteEntitiesAsync<TEntity>(int id, string columnName) where TEntity : class
        {
            var entities = await _dbSet
                .Where(e => EF.Property<int>(e, columnName) == id)
                .ToListAsync();

            if (entities.Any())
            {
                _dbSet.RemoveRange(entities);
                await _context.SaveChangesAsync();  // Ensure changes are committed
            }
        }

        //public async Task DeleteById(int Id)
        //{
        //    try
        //    {
        //        var entity = await GetByIdAsync(Id);
        //        if (entity == null)
        //        {
        //            throw new ArgumentException("Entity not found");
        //        }

        //        _dbSet.Remove(entity);
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        throw new Exception($"Database update failed: {ex.InnerException?.Message}", ex);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"An error occurred while deleting: {ex.Message}", ex);
        //    }
        //}
        public async Task DeleteById(int Id)
        {
            var entity = await GetByIdAsync(Id);
            if (entity == null)
            {
                throw new ArgumentException("Entity not found");
            }

            entity.GetType().GetProperty("IsDeleted")?.SetValue(entity, true);
            entity.GetType().GetProperty("IsActive")?.SetValue(entity, false);

            _context.Update(entity);
            await _context.SaveChangesAsync();
        }

        public Task HardDeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return Task.CompletedTask;
        }


        public async Task DeleteAsync(T entity)
        {
            _context.Entry(entity).State = EntityState.Deleted;

            await _context.SaveChangesAsync();
        }
        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            foreach (var entity in entities)
            {
                _context.Entry(entity).State = EntityState.Deleted;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DeleteRange(IEnumerable<T> entities)
        {
            // **Check if entities are tracked**
            foreach (var entity in entities)
            {
                var trackedEntity = _context.Set<T>().Local.FirstOrDefault(e => e == entity);
                if (trackedEntity != null)
                {
                    _context.Entry(trackedEntity).State = EntityState.Detached; // Detach it
                }
            }

            // **Remove All Entities & Save in One Transaction**
            _context.Set<T>().RemoveRange(entities);
            var deletedCount = await _context.SaveChangesAsync();

            Console.WriteLine($"Total Deleted Records: {deletedCount}");
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.Where(predicate).ToListAsync();
        }
        public async Task PermanentDeleteRangeAsync(IEnumerable<T> entities)
        {
            if (entities == null || !entities.Any())
            {
                return;
            }

            _context.Set<T>().RemoveRange(entities);
            await _context.SaveChangesAsync();

        }

        // Get Paginated Results
        public async Task<PageResult<T>> GetPaginatedAsync(IPageRequest pageRequest)
        {
            var offset = (pageRequest.Page - 1) * pageRequest.PageSize;
            var query = $"SELECT * FROM {typeof(T).Name}s OFFSET @Offset LIMIT @PageSize";
            var countQuery = $"SELECT COUNT(*) FROM {typeof(T).Name}s";

            using (var connection = new NpgsqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                var result = await connection.QueryAsync<T>(query, new { Offset = offset, PageSize = pageRequest.PageSize });

                var rowCount = await connection.QuerySingleAsync<int>(countQuery);

                var pageCount = (int)Math.Ceiling((double)rowCount / pageRequest.PageSize);

                return new PageResult<T>
                {
                    CurrentPage = pageRequest.Page,
                    PageSize = pageRequest.PageSize,
                    RowCount = rowCount,
                    PageCount = pageCount,
                    Result = result
                };
            }
        }




        // Search
        public async Task<ResponseModel<PageResult<T>>> SimpleSearchEntities(string searchTerm, int page, int pageSize)
        {
            // Ensure valid page and pageSize
            page = page > 0 ? page : 1;
            pageSize = pageSize > 0 ? pageSize : 10;

            var query = _dbSet.AsQueryable();

            // Add condition to exclude deleted records
            var isDeletedProperty = typeof(T).GetProperty("IsDeleted");
            if (isDeletedProperty != null)
            {
                // Add condition to exclude records with IsDeleted = true
                var parameter = Expression.Parameter(typeof(T), "e");
                var isDeletedAccess = Expression.Property(parameter, isDeletedProperty);
                var falseValue = Expression.Constant(false);

                var isNotDeletedCondition = Expression.Equal(isDeletedAccess, falseValue);
                var lambda = Expression.Lambda<Func<T, bool>>(isNotDeletedCondition, parameter);

                query = query.Where(lambda);
            }

            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Use reflection to get all string properties of the entity
                var stringProperties = typeof(T).GetProperties()
                    .Where(p => p.PropertyType == typeof(string)) // Include all string properties
                    .ToList();

                // Start building the search expression
                var parameter = Expression.Parameter(typeof(T), "e");
                Expression combinedSearchCondition = null;

                foreach (var prop in stringProperties)
                {
                    // Build the condition for each string property
                    var propertyAccess = Expression.Property(parameter, prop);
                    var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);

                    // Convert both the property value and the search term to lowercase
                    var propertyToLower = Expression.Call(propertyAccess, toLowerMethod);
                    var searchTermToLower = Expression.Constant(searchTerm.ToLower());

                    // Compare the lowercase property value with the lowercase search term
                    var equalsExpression = Expression.Equal(propertyToLower, searchTermToLower);

                    // Combine each condition with OR (i.e., search across multiple properties)
                    combinedSearchCondition = combinedSearchCondition == null
                        ? equalsExpression
                        : Expression.OrElse(combinedSearchCondition, equalsExpression);
                }

                // If any conditions were added, apply the combined search condition to the query
                if (combinedSearchCondition != null)
                {
                    var lambda = Expression.Lambda<Func<T, bool>>(combinedSearchCondition, parameter);
                    query = query.Where(lambda);
                }
            }

            // Order the items in descending order by CreatedOn (or any other property like Id)
            var createdOnProperty = typeof(T).GetProperty("CreatedOn"); // Change this property as needed
            if (createdOnProperty != null)
            {
                var parameter = Expression.Parameter(typeof(T), "e");
                var propertyAccess = Expression.Property(parameter, createdOnProperty);
                var lambda = Expression.Lambda<Func<T, DateTime>>(propertyAccess, parameter);
                query = query.OrderByDescending(lambda);
            }

            // Get the total count of records
            var totalRecords = await query.CountAsync();

            // Apply pagination
            var paginatedQuery = query.Skip((page - 1) * pageSize).Take(pageSize);

            // Get the list of entities
            var entityList = await paginatedQuery.ToListAsync();

            // If no records are found, return a response with an empty list
            if (!entityList.Any())
            {
                return new ResponseModel<PageResult<T>>
                {
                    Result = new PageResult<T>
                    {
                        CurrentPage = page,
                        PageSize = pageSize,
                        RowCount = totalRecords,
                        PageCount = 0,
                        Result = new List<T>()
                    },
                    Message = "No records found.",
                    HttpStatusCode = 200
                };
            }

            // Calculate the total number of pages
            var pageCount = (int)Math.Ceiling((double)totalRecords / pageSize);

            // Return the paginated response
            return new ResponseModel<PageResult<T>>
            {
                Result = new PageResult<T>
                {
                    CurrentPage = page,
                    PageSize = pageSize,
                    RowCount = totalRecords,
                    PageCount = pageCount,
                    Result = entityList
                },
                Message = "Records retrieved successfully.",
                HttpStatusCode = 200
            };
        }
    }
}
