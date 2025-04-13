using AutoMapper;
using miniCRM_back.Configs;
using miniCRM_back.Database;
using miniCRM_back.DTOs;
using miniCRM_back.Models;

namespace miniCRM_back.Services {
    public class TaskItemService : BaseService<TaskItem, TaskItemDto, TaskItemForCreationDto>, ITaskItemService {
        private readonly ITaskRepository _taskItemRepository;
        public TaskItemService(ITaskRepository repository, ILogger<BaseService<TaskItem, TaskItemDto, TaskItemForCreationDto>> logger, IMapper mapper) : base(repository, logger, mapper) {
            _taskItemRepository = repository;
        }

        public async Task<PagedResult<IEnumerable<TaskItemDto>>> GetTasksByUserId(int userId, PaginationParams paginationParams) {
            try {
                var tasks = await _taskItemRepository.GetTasksByUserId(userId, paginationParams);
                var dtos = mapper.Map<IEnumerable<TaskItemDto>>(tasks);
                var totalCount = await _taskItemRepository.CountTasksCountByUserId(userId);
                var paginationMetadata = GetPaginationMetadata(paginationParams, totalCount);
                return PagedResult<IEnumerable<TaskItemDto>>.Success(dtos, paginationMetadata);
            } catch (Exception ex) {
                return PagedResult<IEnumerable<TaskItemDto>>.Failure("InternalServerError", ex.Message);
            }
        }

        
    }
}
