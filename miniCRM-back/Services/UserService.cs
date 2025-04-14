using AutoMapper;
using miniCRM_back.Configs;
using miniCRM_back.Database;
using miniCRM_back.DTOs;
using miniCRM_back.Models;
using NSwag.Generation.Processors;
using System.Net.WebSockets;

namespace miniCRM_back.Services {
    public class UserService : BaseService<User, UserDto, UserForCreationDto, UserForUpdateDto>, IUserService {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository repository, ILogger<BaseService<User, UserDto, UserForCreationDto, UserForUpdateDto>> logger, IMapper mapper) : base(repository, logger, mapper) {
            _userRepository = repository;
        }

        public override async Task<Result<UserDto>> CreateAsync(UserForCreationDto createDto) {
            var user = mapper.Map<User>(createDto);
            user.PasswordHash = AuthService.HashPassword(createDto.Password);
            var created = await _userRepository.CreateAsync(user);
            return Result<UserDto>.Success(mapper.Map<UserDto>(created));
        }

        public override async Task<Result<UserDto>> UpdateAsync(UserForUpdateDto dto) {
            var existingUser = await _userRepository.GetByIdAsync(dto.Id);
            if (existingUser == null) {
                return Result<UserDto>.Failure("NotFound","User not found");
            }
           
            mapper.Map(dto, existingUser);

            if (dto.Password is not null) {
                existingUser.PasswordHash = AuthService.HashPassword(dto.Password);
            }

            var updated = await _userRepository.UpdateAsync(existingUser);
            return Result<UserDto>.Success(mapper.Map<UserDto>(updated));
        }
        public async Task<PagedResult<IEnumerable<UserWithTaskItems>>> GetUsersWithTaskItems(PaginationParams paginationParams) {
            // get all users with percent completion for his/her tasks and task count
            try {
                var result = await _userRepository.GetUsersWithTasks(paginationParams);
                var paginationMetadata = GetPaginationMetadata(paginationParams, result.totalCount);
                return PagedResult<IEnumerable<UserWithTaskItems>>.Success(result.data, paginationMetadata);
            } catch (Exception ex) {
                return PagedResult<IEnumerable<UserWithTaskItems>>.Failure("InternalServerError", ex.Message);
            }
        }

        public async Task<PagedResult<IEnumerable<TaskItemsGroupByUser>>> GetTaskItemsGroupByUser(PaginationParams paginationParams) {
            try {
                var result = await _userRepository.GetTaskItemsGroupByUser(paginationParams);

                var paginationMetadata = GetPaginationMetadata(paginationParams, result.totalCount);
                

                return PagedResult<IEnumerable<TaskItemsGroupByUser>>.Success(result.data, paginationMetadata);
            } catch (Exception ex) {
                return PagedResult<IEnumerable<TaskItemsGroupByUser>>.Failure("InternalServerError", ex.Message);
            }
        }
    }
}
