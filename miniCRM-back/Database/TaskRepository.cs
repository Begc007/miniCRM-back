using Microsoft.EntityFrameworkCore;
using miniCRM_back.Configs;
using miniCRM_back.Models;

namespace miniCRM_back.Database {
    public class TaskRepository : GenericRepository<TaskItem>, ITaskRepository {
        private readonly crmDbContext _context;
        public TaskRepository(crmDbContext context) : base(context) {
            _context = context;
        }

        public Task<int> CountTasksCountByUserId(int userId) {
            var query = GetQuery(userId);
            return query.CountAsync();
        }

        public async Task<IEnumerable<TaskItem>> GetTasksByUserId(int userId, PaginationParams paginationParams) {
            IQueryable<TaskItem> query = GetQuery(userId);

            query = GetOrderByQuery(paginationParams, query);

            var tasks = await query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            return tasks;
        }

        private IQueryable<TaskItem> GetQuery(int userId) {
            return _context.TaskItems
                            .Where(t => t.UserId == userId)
                            .AsQueryable();
        }
    }
}
