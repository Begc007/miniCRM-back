using miniCRM_back.Configs;
using System.Linq.Expressions;

namespace miniCRM_back.Services {
    public interface IBaseService<TEntity, TDto, TCreateDto>
        where TEntity : class
        where TDto : class
        where TCreateDto : class {
        Task<Result<TDto>> CreateAsync(TCreateDto createDto);
        Task<Result<TDto>> UpdateAsync(TDto updateDto);
        Task DeleteAsync(int id);
        Task<PagedResult<IEnumerable<TDto>>> GetAllAsync(PaginationParams paginationParams);
        Task<PagedResult<IEnumerable<TDto>>> GetAllWithIncludesAsync(PaginationParams paginationParams, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<Result<TDto>> GetByIdAsync(int id);
        Task<int> GetCountAsync();
    }
}
