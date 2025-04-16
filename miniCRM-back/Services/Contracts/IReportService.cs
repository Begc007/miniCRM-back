using miniCRM_back.Configs;
using miniCRM_back.DTOs;
using miniCRM_back.Models;

namespace miniCRM_back.Services.Contracts {
    public interface IReportService:IBaseService<TaskItem, TaskItemDto, TaskItemForCreationDto, TaskItemDto> {
        Task<PagedResult<IEnumerable<ExpiredTaskItem>>> GetExpiredTasksAsync(PaginationParams paginationParams, bool includeBlankExpiredAt);
    }
}
