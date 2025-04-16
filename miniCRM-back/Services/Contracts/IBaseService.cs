using miniCRM_back.Configs;
using miniCRM_back.DTOs;
using System.Linq.Expressions;

namespace miniCRM_back.Services.Contracts {
    public interface IBaseService<TEntity, TDto, TCreateDto, TUpdateDto>
        where TEntity : class
        where TDto : class
        where TCreateDto : class
        where TUpdateDto: class {
        Task<Result<TDto>> CreateAsync(TCreateDto createDto);
        Task<Result<TDto>> UpdateAsync(TUpdateDto updateDto);
        Task DeleteAsync(int id);
        Task<PagedResult<IEnumerable<TDto>>> GetAllAsync(PaginationParams paginationParams);
        Task<PagedResult<IEnumerable<TDto>>> GetAllWithIncludesAsync(PaginationParams paginationParams, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<Result<TDto>> GetByIdAsync(int id);
        Task<int> GetCountAsync();
        PaginationMetadata GetPaginationMetadata(PaginationParams paginationParams, int totalCount);
        
    }
}
