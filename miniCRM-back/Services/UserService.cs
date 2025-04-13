using AutoMapper;
using miniCRM_back.Configs;
using miniCRM_back.Database;
using miniCRM_back.DTOs;
using miniCRM_back.Models;

namespace miniCRM_back.Services {
    public class UserService : BaseService<User, UserDto, UserForCreationDto>, IUserService {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository repository, ILogger<BaseService<User, UserDto, UserForCreationDto>> logger, IMapper mapper) : base(repository, logger, mapper) {
            _userRepository = repository;
        }

        public async Task<PagedResult<IEnumerable<UserWithTaskItems>>> GetUsersWithTaskItems(PaginationParams paginationParams) {
            // get all users with percent completion for his/her tasks and task count
            try {
                var tasks = await _userRepository.GetUsersWithTasks(paginationParams);
                var totalCount = await _userRepository.CountUsersWithTasks();
                var paginationMetadata = GetPaginationMetadata(paginationParams, totalCount);
                return PagedResult<IEnumerable<UserWithTaskItems>>.Success(tasks, paginationMetadata);
            } catch (Exception ex) {
                return PagedResult<IEnumerable<UserWithTaskItems>>.Failure("InternalServerError", ex.Message);
            }
        }

        public async Task<PagedResult<IEnumerable<TaskItemsGroupByUser>>> GetTaskItemsGroupByUser(PaginationParams paginationParams) {
            try {
                var usersTasks = await _userRepository.GetUsersWithTasks(paginationParams);
                var totalCount = await _userRepository.CountUsersWithTasks();
                var paginationMetadata = GetPaginationMetadata(paginationParams, totalCount);

                var grouped = usersTasks.GroupBy(u => new { u.UserId, u.UserName, u.FIO, u.Position })
                                                .Select(g => new TaskItemsGroupByUser {
                                                    UserId = g.Key.UserId,
                                                    UserName = g.Key.UserName,
                                                    FIO = g.Key.FIO,
                                                    Position = g.Key.Position,
                                                    TaskItemCount = g.Where(t => t.TaskItemId > 0).Select(t => t.TaskItemId).Distinct().Count(),
                                                    CompletedPercent = g.Average(t => t.TaskItemPercent)
                                                });

                return PagedResult<IEnumerable<TaskItemsGroupByUser>>.Success(grouped, paginationMetadata);
            } catch (Exception ex) {
                return PagedResult<IEnumerable<TaskItemsGroupByUser>>.Failure("InternalServerError", ex.Message);
            }
        }
    }
}
