﻿using miniCRM_back.Configs;
using miniCRM_back.Database;
using miniCRM_back.DTOs;
using miniCRM_back.Models;

namespace miniCRM_back.Services {
    public interface IUserService:IBaseService<User, UserDto, UserForCreationDto, UserForUpdateDto> {
        Task<PagedResult<IEnumerable<UserWithTaskItems>>> GetUsersWithTaskItems(PaginationParams paginationParams);
        Task<PagedResult<IEnumerable<TaskItemsGroupByUser>>> GetTaskItemsGroupByUser(PaginationParams paginationParams);
    }
}
