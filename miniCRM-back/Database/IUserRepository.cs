using miniCRM_back.Configs;
using miniCRM_back.DTOs;
using miniCRM_back.Models;

namespace miniCRM_back.Database {
    public interface IUserRepository:IGenericRepository<User> {
        Task<(IEnumerable<UserWithTaskItems> data, int totalCount)> GetUsersWithTasks(PaginationParams paginationParams);
        Task<User?> GetUserWithTasks(int id);
        Task<(IEnumerable<TaskItemsGroupByUser> data, int totalCount)> GetTaskItemsGroupByUser(PaginationParams paginationParams, string? fio);
    }
}
