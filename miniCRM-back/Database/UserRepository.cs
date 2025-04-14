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

        public async Task<(IEnumerable<UserWithTaskItems> data,int totalCount)> GetUsersWithTasks(PaginationParams paginationParams) {
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
                 ).ToListAsync();
            //.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
            //.Take(paginationParams.PageSize).ToListAsync();
            var totalCount = tasks.Count();
            return (data: tasks, totalCount);
        }

        public async Task<(IEnumerable<TaskItemsGroupByUser> data, int totalCount)> GetTaskItemsGroupByUser(PaginationParams paginationParams) {
            var query = _dbSet.AsQueryable();
            query = GetOrderByQuery(paginationParams, query);
            var usersTasks = await query.Include(u => u.TaskItems) //TODO: replace this query with GetTaskItemsQuery method
                .SelectMany(u => u.TaskItems.DefaultIfEmpty(),
                            (u, ti) => new UserWithTaskItems {
                                UserId = u.Id,
                                UserName = u.Name,
                                FIO = u.FIO,
                                Position = u.Position,
                                TaskItemId = ti != null ? ti.Id : -1,
                                TaskItemPercent = ti != null ? ti.Percent : 0
                            }
                 ).ToListAsync();
            var grouped = usersTasks.GroupBy(u => new { u.UserId, u.UserName, u.FIO, u.Position })
                                            .Select(g => new TaskItemsGroupByUser {
                                                UserId = g.Key.UserId,
                                                UserName = g.Key.UserName,
                                                FIO = g.Key.FIO,
                                                Position = g.Key.Position,
                                                TaskItemCount = g.Where(t => t.TaskItemId > 0).Select(t => t.TaskItemId).Distinct().Count(),
                                                CompletedPercent = g.Average(t => t.TaskItemPercent)
                                            });
            var paged = grouped.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                                            .Take(paginationParams.PageSize).ToList();
            var totalCount = grouped.Count();
            return (data: paged, totalCount);
        }

        public Task<User?> GetUserWithTasks(int id) {
            throw new NotImplementedException();
        }

        public override async Task<User> CreateAsync(User entity) {
            await _dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }
    }
}
