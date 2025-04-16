using Asp.Versioning;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using miniCRM_back.DTOs;
using miniCRM_back.Models;
using miniCRM_back.Configs;
using miniCRM_back.Services.Contracts;

namespace miniCRM_back.Controllers {
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class TaskItemController : ControllerBase {
        private readonly ITaskItemService _service;
        private readonly ILogger<TaskItemController> _logger;
        public TaskItemController(ITaskItemService service, ILogger<TaskItemController> logger) {
            _service = service; _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<TaskItemDto>>> GetById(int id) {
            var result = await _service.GetByIdAsync(id);
            if(result.IsSuccess) {
                return Ok(ApiResponse<TaskItemDto>.SuccessResponse(result.Value));
            }
            else {
                return BadRequest(ApiResponse<TaskItemDto>.ErrorResponse(result.ErrorCode, result.ErrorMessage));
            }
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

        [HttpGet("user/{userId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<TaskItemDto>>>> GetTasksByUserId(int userId,PaginationParams paginationParams) {
            var result = await _service.GetTasksByUserId(userId,paginationParams);
            if (result.IsSuccess) {
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

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id) {
            if (id <= 0) return BadRequest(ApiResponse<TaskItemDto>.ErrorResponse("IdMismatch", "Id mismatch"));
            await _service.DeleteAsync(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<TaskItemDto>>> Update(int id, TaskItemDto taskItem) {
            if (id != taskItem.Id) {
                return BadRequest(ApiResponse<TaskItemDto>.ErrorResponse("IdMismatch", "Id mismatch"));
            }
            var result = await _service.UpdateAsync(taskItem);
            if (result.IsSuccess) {
                return Ok(ApiResponse<TaskItemDto>.SuccessResponse(result.Value));
            }
            else {
                return BadRequest(ApiResponse<TaskItemDto>.ErrorResponse(result.ErrorCode, result.ErrorMessage));
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
