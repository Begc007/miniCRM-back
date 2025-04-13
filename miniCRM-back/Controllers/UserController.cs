using Asp.Versioning;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using miniCRM_back.Configs;
using miniCRM_back.Database;
using miniCRM_back.DTOs;
using miniCRM_back.Models;
using miniCRM_back.Models.Auth;
using miniCRM_back.Services;

namespace miniCRM_back.Controllers {
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public UserController(IAuthService authService, IUserService userService) {
            _authService = authService; _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(Models.Auth.LoginRequest request) {
            var response = await _authService.LoginAsync(request);

            if (response == null) {
                return Unauthorized(new { message = "Неверное имя пользователя или пароль" });
            }

            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(Models.Auth.RegisterRequest request) {//TODO: later make this method JWT protected
            var response = await _authService.RegisterAsync(request);

            if (response == null) {
                return BadRequest(new { message = "Пользователь с таким именем уже существует" });
            }

            return Ok(response);
        }

        [HttpGet()]
        public async Task<ActionResult<PagedResult<IEnumerable<UserWithTaskItems>>>> GetAll(PaginationParams paginationParams) {
            var result = await _userService.GetUsersWithTaskItems(paginationParams);
            if (result.IsSuccess) {
                return Ok(ApiResponse<IEnumerable<UserWithTaskItems>>.SuccessResponse(result.Value, pagination: GetPaginationMetadata(result)));
            }
            else {
                return BadRequest(ApiResponse<UserWithTaskItems>.ErrorResponse(result.ErrorCode, result.ErrorMessage));
            }
        }

        private static PaginationMetadata GetPaginationMetadata(PagedResult<IEnumerable<UserWithTaskItems>> result) { //TODO: make this method common into utils class since it is repeated in controllers
            return new PaginationMetadata {
                CurrentPage = result.Pagination.CurrentPage,
                TotalPages = result.Pagination.TotalPages,
                PageSize = result.Pagination.PageSize,
                TotalCount = result.Pagination.TotalCount
            };
        }
    }
}
