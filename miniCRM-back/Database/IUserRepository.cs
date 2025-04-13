using miniCRM_back.Configs;
using miniCRM_back.DTOs;
using miniCRM_back.Models;

namespace miniCRM_back.Database {
    public interface IUserRepository:IGenericRepository<User> {
        Task<IEnumerable<UserWithTaskItems>> GetUsersWithTasks(PaginationParams paginationParams);
        Task<User?> GetUserWithTasks(int id);
        Task<int> CountUsersWithTasks();
    }
}
