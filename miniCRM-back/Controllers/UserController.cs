using Asp.Versioning;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using miniCRM_back.Models;
using miniCRM_back.Models.Auth;
using miniCRM_back.Services;

namespace miniCRM_back.Controllers {
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class UserController : ControllerBase {
        //private readonly IBaseService<User, UserDto, UserForCreationDto> _service;
        //private readonly ILogger<UserController> _logger;
        private readonly IAuthService _authService;

        public UserController(IAuthService authService) {
            _authService = authService;
        }
        //public UserController(IBaseService<User, UserDto, UserForCreationDto> service, ILogger<UserController> logger) {
        //    _service = service; _logger = logger;
        //}

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
    }
}
