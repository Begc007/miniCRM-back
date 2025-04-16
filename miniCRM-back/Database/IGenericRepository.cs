using miniCRM_back.Configs;
using miniCRM_back.Models;
using System.Linq.Expressions;

namespace miniCRM_back.Database {
    public interface IGenericRepository<TEntity>
        where TEntity : BaseEntity{
            Task<TEntity> CreateAsync(TEntity entity);
            Task<TEntity> UpdateAsync(TEntity TEntity);
            Task DeleteAsync(int id);
            Task<TEntity?> GetByIdAsync(int id);
            Task<TEntity?> GetByIdWithIncludesAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties);
            Task<int> CountAsync();
            Task<int> CountWithCustomQueryAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryBuilder);
            Task<IEnumerable<TEntity>> GetAllAsync(PaginationParams paginationParams);
            Task<(IEnumerable<TEntity> data, int totalCount)> GetWithCustomQueryAsync(PaginationParams paginationParams, Func<IQueryable<TEntity>, IQueryable<TEntity>> queryBuilder);
            Task<IEnumerable<TEntity>> GetAllWithIncludesAsync(PaginationParams paginationParams, params Expression<Func<TEntity, object>>[] includeProperties);
    }
}
