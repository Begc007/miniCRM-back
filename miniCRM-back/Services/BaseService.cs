using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using miniCRM_back.Configs;
using miniCRM_back.Database;
using miniCRM_back.Models;
using System.Linq.Expressions;

namespace miniCRM_back.Services {
    public class BaseService<TEntity, TDto, TCreateDto> : IBaseService<TEntity, TDto, TCreateDto>
        where TEntity : BaseEntity
        where TDto : class
        where TCreateDto : class {
        protected readonly IGenericRepository<TEntity> repository;
        protected readonly ILogger<BaseService<TEntity, TDto, TCreateDto>> logger;
        protected readonly IMapper mapper;

        public BaseService(IMapper mapper) {
            this.mapper = mapper;
        }
        public Task<Result<TDto>> CreateAsync(TCreateDto createDto) {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id) {
            throw new NotImplementedException();
        }

        public Task<PagedResult<IEnumerable<TDto>>> GetAllAsync(PaginationParams paginationParams) {
            throw new NotImplementedException();
        }

        public Task<PagedResult<IEnumerable<TDto>>> GetAllWithIncludesAsync(PaginationParams paginationParams, params Expression<Func<TEntity, object>>[] includeProperties) {
            throw new NotImplementedException();
        }

        public Task<Result<TDto>> GetByIdAsync(int id) {
            throw new NotImplementedException();
        }

        public Task<int> GetCountAsync() {
            throw new NotImplementedException();
        }

        public Task<Result<TDto>> UpdateAsync(TDto updateDto) {
            throw new NotImplementedException();
        }
    }
}
