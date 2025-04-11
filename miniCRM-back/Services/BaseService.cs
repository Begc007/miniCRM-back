﻿using AutoMapper;
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

        public BaseService(IGenericRepository<TEntity> repository, ILogger<BaseService<TEntity, TDto, TCreateDto>> logger, IMapper mapper) {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
        }
        public Task<Result<TDto>> CreateAsync(TCreateDto createDto) {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id) {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<IEnumerable<TDto>>> GetAllAsync(PaginationParams paginationParams) {
            try {
                var totalCount = await repository.CountAsync();

                var items = mapper.Map<IEnumerable<TDto>>(await repository.GetAllAsync(paginationParams));

                var paginationMetadata = new PaginationMetadata {
                    CurrentPage = paginationParams.PageNumber,
                    PageSize = paginationParams.PageSize,
                    TotalCount = totalCount,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)paginationParams.PageSize)
                };

                return PagedResult<IEnumerable<TDto>>.Success(items, paginationMetadata);
            } catch (Exception ex) {
                return PagedResult<IEnumerable<TDto>>.Failure("InternalServerError", ex.Message);
            }
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
