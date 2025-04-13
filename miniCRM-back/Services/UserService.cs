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
                //var dtos = mapper.Map<IEnumerable<UserWithTaskItems>>(tasks);
                var totalCount = await _userRepository.CountUsersWithTasks();
                var paginationMetadata = GetPaginationMetadata(paginationParams, totalCount);
                return PagedResult<IEnumerable<UserWithTaskItems>>.Success(tasks, paginationMetadata);
            } catch (Exception ex) {
                return PagedResult<IEnumerable<UserWithTaskItems>>.Failure("InternalServerError", ex.Message);
            }
        }
    }
}
