using Microsoft.EntityFrameworkCore;
using miniCRM_back.Configs;
using miniCRM_back.Models;
using System.Linq.Expressions;

namespace miniCRM_back.Database {
    public class GenericRepository<TEntity> : IGenericRepository<TEntity>
        where TEntity : BaseEntity {
        protected readonly crmDbContext context;
        protected readonly DbSet<TEntity> dbSet;

        public GenericRepository(crmDbContext context) {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        protected static IQueryable<TEntity> GetOrderByQuery(PaginationParams paginationParams, IQueryable<TEntity> query) {
            if (string.IsNullOrEmpty(paginationParams.SortBy)) {
                query = query.OrderBy(e => e.Id);
            }
            else {
                query = paginationParams.SortDirection == "desc"
                    ? query.OrderByDescending(e => EF.Property<object>(e, paginationParams.SortBy.FirstCharToUpper()))
                    : query.OrderBy(e => EF.Property<object>(e, paginationParams.SortBy.FirstCharToUpper()));
            }

            return query;
        }
        public async Task<int> CountAsync() {
            return await dbSet.CountAsync();
        }

        public async Task<int> CountWithCustomQueryAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryBuilder) {
            var baseQuery = queryBuilder(dbSet);
            return await baseQuery.CountAsync();
        }

        public virtual async Task<TEntity> CreateAsync(TEntity entity) {
            await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public virtual async Task DeleteAsync(int id) {
            var entity = await GetByIdAsync(id);
            if(entity is not null) {
                dbSet.Remove(entity);
                await context.SaveChangesAsync();
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(PaginationParams paginationParams) {
            var query = dbSet.AsQueryable();
            query = GetOrderByQuery(paginationParams, query);
            var entities = await query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();
            return entities;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllWithIncludesAsync(PaginationParams paginationParams, params Expression<Func<TEntity, object>>[] includeProperties) {
                IQueryable<TEntity> query = dbSet;
                query = GetOrderByQuery(paginationParams, query);

                foreach (var includeProperty in includeProperties) {
                    query = query.Include(includeProperty);
                }

                query = query
                    .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                    .Take(paginationParams.PageSize);

                var entities = await query.ToListAsync();

                return entities;
            }

        public virtual async Task<TEntity?> GetByIdAsync(int id) {
            var entity = await dbSet.FindAsync(id);
            return entity;
        }

        public async Task<TEntity?> GetByIdWithIncludesAsync(int id, params Expression<Func<TEntity, object>>[] includeProperties) {
            IQueryable<TEntity> query = dbSet.Where(e => e.Id == id);

            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }

            var entity = await query.SingleOrDefaultAsync();
            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetWithCustomQueryAsync(PaginationParams paginationParams, Func<IQueryable<TEntity>, IQueryable<TEntity>> queryBuilder) {
            var query = queryBuilder(dbSet);
            var sortedQuery = GetOrderByQuery(paginationParams, query);
            var entities = await query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                                        .Take(paginationParams.PageSize).ToListAsync();
            return entities;
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity TEntity) {
            dbSet.Update(TEntity);
            await context.SaveChangesAsync();
            return TEntity;
        }
    }
}