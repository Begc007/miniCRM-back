using AutoMapper;
using miniCRM_back.Configs;
using miniCRM_back.Database;
using miniCRM_back.DTOs;
using miniCRM_back.Models;
using System.Linq.Expressions;

namespace miniCRM_back.Services {
    public class BaseService<TEntity, TDto, TCreateDto, TUpdateDto> : IBaseService<TEntity, TDto, TCreateDto, TUpdateDto>
        where TEntity : BaseEntity
        where TDto : class
        where TCreateDto : class
        where TUpdateDto: class {
        protected readonly IGenericRepository<TEntity> repository;
        protected readonly ILogger<BaseService<TEntity, TDto, TCreateDto, TUpdateDto>> logger;
        protected readonly IMapper mapper;

        public BaseService(IGenericRepository<TEntity> repository, ILogger<BaseService<TEntity, TDto, TCreateDto, TUpdateDto>> logger, IMapper mapper) {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
        }

        public virtual async Task<Result<TDto>> CreateAsync(TCreateDto createDto) {
            var entity = mapper.Map<TEntity>(createDto);
            var created = await repository.CreateAsync(entity);
            var dto = mapper.Map<TDto>(created);
            return Result<TDto>.Success(dto);
        }

        public virtual async Task DeleteAsync(int id) {
            await repository.DeleteAsync(id);
        }

        public virtual async Task<PagedResult<IEnumerable<TDto>>> GetAllAsync(PaginationParams paginationParams) {
            try {
                var totalCount = await repository.CountAsync();

                var items = mapper.Map<IEnumerable<TDto>>(await repository.GetAllAsync(paginationParams));
                PaginationMetadata paginationMetadata = GetPaginationMetadata(paginationParams, totalCount);

                return PagedResult<IEnumerable<TDto>>.Success(items, paginationMetadata);
            } catch (Exception ex) {
                return PagedResult<IEnumerable<TDto>>.Failure("InternalServerError", ex.Message);
            }
        }

        public PaginationMetadata GetPaginationMetadata(PaginationParams paginationParams, int totalCount) {
            return new PaginationMetadata {
                CurrentPage = paginationParams.PageNumber,
                PageSize = paginationParams.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)paginationParams.PageSize)
            };
        }

        public Task<PagedResult<IEnumerable<TDto>>> GetAllWithIncludesAsync(PaginationParams paginationParams, params Expression<Func<TEntity, object>>[] includeProperties) {
            throw new NotImplementedException();
        }

        public virtual async Task<Result<TDto>> GetByIdAsync(int id) {
            try {
                var entity = await repository.GetByIdAsync(id);
                return entity is null
                    ? Result<TDto>.Failure("NotFound", "Entity not found")
                    : Result<TDto>.Success(mapper.Map<TDto>(entity));
            } catch (Exception ex) {
                return Result<TDto>.Failure("InternalServerError", ex.Message);
            }
        }

        public virtual Task<int> GetCountAsync() {
            throw new NotImplementedException();
        }

        public virtual async Task<Result<TDto>> UpdateAsync(TUpdateDto updateDto) {
            try {
                var entity = mapper.Map<TEntity>(updateDto);
                var updated = await repository.UpdateAsync(entity);
                return Result<TDto>.Success(mapper.Map<TDto>(updated));
            } catch (Exception ex) {
                return Result<TDto>.Failure("InternalServerError", ex.Message);
            }
        }

    }
}
