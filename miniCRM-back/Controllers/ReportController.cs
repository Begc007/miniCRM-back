using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using miniCRM_back.Configs;
using miniCRM_back.DTOs;
using miniCRM_back.Services;
using miniCRM_back.Services.Contracts;
using System.Collections.Generic;

namespace miniCRM_back.Controllers {
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class ReportController : ControllerBase {
        private readonly IReportService _service;
        private readonly ILogger<ReportController> _logger;
        public ReportController(IReportService service, ILogger<ReportController> logger) {
            _service = service; _logger = logger;
        }

        [HttpGet("expired-tasks")]
        public async Task<ActionResult<ApiResponse<IEnumerable<ExpiredTaskItem>>>> GetExpiredTasks(PaginationParams paginationParams, bool includeBlankExpiredAt) {
            var result = await _service.GetExpiredTasksAsync(paginationParams, includeBlankExpiredAt);
            if (result.IsSuccess) {
                return ApiResponse<IEnumerable<ExpiredTaskItem>>
                    .SuccessResponse(result.Value, pagination: result.GetPaginationMetadata());
            }
            return BadRequest(ApiResponse<ExpiredTaskItem>.ErrorResponse(result.ErrorCode, result.ErrorMessage));
        }
    }
}