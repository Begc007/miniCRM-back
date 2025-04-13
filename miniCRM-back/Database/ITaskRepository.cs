using miniCRM_back.Configs;
using miniCRM_back.Models;

namespace miniCRM_back.Database {
    public interface ITaskRepository:IGenericRepository<TaskItem> {
        Task<IEnumerable<TaskItem>> GetTasksByUserId(int userId, PaginationParams paginationParams);
        Task<int> CountTasksCountByUserId(int userId);
    }
}
