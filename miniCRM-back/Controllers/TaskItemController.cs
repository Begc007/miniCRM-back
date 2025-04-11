using Asp.Versioning;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using miniCRM_back.DTOs;
using miniCRM_back.Services;
using miniCRM_back.Models;
using miniCRM_back.Configs;

namespace miniCRM_back.Controllers {
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TaskItemController : ControllerBase {
        private readonly IBaseService<TaskItem,TaskItemDto, TaskItemForCreationDto> _service;
        private readonly ILogger<TaskItemController> _logger;
        public TaskItemController(IBaseService<TaskItem, TaskItemDto, TaskItemForCreationDto> service, ILogger<TaskItemController> logger) {
            _service = service; _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<TaskItemDto>>>> GetAll(PaginationParams paginationParams) {
            var result = await _service.GetAllAsync(paginationParams);
            if(result.IsSuccess) {
                return Ok(ApiResponse<IEnumerable<TaskItemDto>>.SuccessResponse(result.Value, pagination: GetPaginationMetadata(result)));
            }
            else {
                return BadRequest(ApiResponse<TaskItemDto>.ErrorResponse(result.ErrorCode, result.ErrorMessage));
            }
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<TaskItemDto>>> Create(TaskItemForCreationDto taskItem) {
            var result = await _service.CreateAsync(taskItem);
            if (result.IsSuccess) {
                return CreatedAtAction(nameof(Create), ApiResponse<TaskItemDto>.SuccessResponse(result.Value));
            }
            else {
                return BadRequest(ApiResponse<TaskItemDto>.ErrorResponse(result.ErrorCode, result.ErrorMessage ));
            }
        }



        private static PaginationMetadata GetPaginationMetadata(PagedResult<IEnumerable<TaskItemDto>> result) {
            return new PaginationMetadata {
                CurrentPage = result.Pagination.CurrentPage,
                TotalPages = result.Pagination.TotalPages,
                PageSize = result.Pagination.PageSize,
                TotalCount = result.Pagination.TotalCount
            };
        }
    }
}
