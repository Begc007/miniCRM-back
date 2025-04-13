using miniCRM_back.Configs;
using miniCRM_back.DTOs;
using miniCRM_back.Models;

namespace miniCRM_back.Services {
    public interface ITaskItemService:IBaseService<TaskItem, TaskItemDto, TaskItemForCreationDto> {
        Task<PagedResult<IEnumerable<TaskItemDto>>> GetTasksByUserId(int userId, PaginationParams paginationParams);
    }
}