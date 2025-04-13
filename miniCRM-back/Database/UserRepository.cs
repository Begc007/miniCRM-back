using Microsoft.EntityFrameworkCore;
using miniCRM_back.Configs;
using miniCRM_back.DTOs;
using miniCRM_back.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace miniCRM_back.Database {

    public class UserRepository : GenericRepository<User>, IUserRepository {
        private readonly DbSet<User> _dbSet;
        public UserRepository(crmDbContext context) : base(context) {
            _dbSet = context.Set<User>();
        }

        public async Task<int> CountUsersWithTasks() {
            var query = GetTaskItemsQuery();
            var count = await query.CountAsync();

            return count;
        }

        public async Task<IEnumerable<UserWithTaskItems>> GetUsersWithTasks(PaginationParams paginationParams) {
            var query = _dbSet.AsQueryable();
            query = GetOrderByQuery(paginationParams, query);
            var tasks = await query.Include(u => u.TaskItems) //TODO: replace this query with GetTaskItemsQuery method
                .SelectMany(u => u.TaskItems.DefaultIfEmpty(),
                            (u, ti) => new UserWithTaskItems {
                                                UserId = u.Id,
                                                UserName = u.Name,
                                                FIO = u.FIO,
                                                Position = u.Position,
                                                TaskItemId = ti != null ? ti.Id : -1,
                                                TaskItemPercent = ti != null ? ti.Percent : 0
                                            }
                 )
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize).ToListAsync();
            return tasks;
        }

        public Task<User?> GetUserWithTasks(int id) {
            throw new NotImplementedException();
        }

        private IQueryable<UserWithTaskItems> GetTaskItemsQuery() {
            return _dbSet
                .Include(u => u.TaskItems)
                .SelectMany(u => u.TaskItems, (u, ti) =>
                new UserWithTaskItems { UserId = u.Id, UserName = u.Name, FIO = u.FIO, Position = u.Position, TaskItemId = ti.Id, TaskItemPercent = ti.Percent });
        }
    }
}
