using Asp.Versioning;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using miniCRM_back.Configs;
using miniCRM_back.DTOs;
using miniCRM_back.Models;
using miniCRM_back.Services.Contracts;
using System.Collections.Generic;

namespace miniCRM_back.Controllers {
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    public class CommentController : ControllerBase {
        private readonly ICommentService _service;
        private readonly ILogger<CommentController> _logger;
        public CommentController(ICommentService service, ILogger<CommentController> logger) {
            _service = service; _logger = logger;
        }

        [HttpGet("task/{taskId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<CommentDto>>>> GetByTaskId(int taskId) {
            var result = await _service.GetByTaskIdAsync(taskId);
            if (result.IsSuccess) {
                return Ok(ApiResponse<IEnumerable<CommentDto>>.SuccessResponse(result.Value));
            }
            return BadRequest(ApiResponse< IEnumerable<CommentDto>>.ErrorResponse(result.ErrorCode, result.ErrorMessage));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<CommentDto>>> Create(CommentForCreateDto commentDto) {
            var result = await _service.CreateAsync(commentDto);
            if (result.IsSuccess) {
                return Ok(ApiResponse<CommentDto>.SuccessResponse(result.Value));
            }
            return BadRequest(ApiResponse<CommentDto>.ErrorResponse(result.ErrorCode, result.ErrorMessage));
        }

        [HttpPost("upload")]
        public async Task<ActionResult<ApiResponse<CommentDto>>> Upload(int taskItemId, int userId, string? text, IFormFile file) {
            if (file == null || file.Length == 0) {
                return BadRequest(new { success = false, message = "No file uploaded" });
            }

            //TODO: later create special service for file management tasks. Controller action must be "thin"
            var uploadsFolder = "Files";
            if (!Directory.Exists(uploadsFolder)) {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = $"{Guid.NewGuid()}-{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create)) {
                await file.CopyToAsync(stream);
            }

            var dto = new CommentForCreateDto {
                UserId = userId,
                Text = text ?? $"Добавлен файл \"{file.FileName}\"",
                TaskItemId = taskItemId,
                CreateTimestamp = DateTime.UtcNow,
                CreatedBy = "system",
                //File = fileData,
                FileName = file.FileName,
                FilePath = $"/{uploadsFolder}/{uniqueFileName}",
                ContentType = file.ContentType,
                Size = file.Length
            };
            var result = await _service.CreateAsync(dto);

            if (result.IsSuccess) {
                return Ok(ApiResponse<CommentDto>.SuccessResponse(result.Value));
            }
            return BadRequest(ApiResponse<CommentDto>.ErrorResponse(result.ErrorCode, result.ErrorMessage));

        }

        [HttpGet("file/{id}")]
        public async Task<ActionResult> DownloadFile(int id) {
            //TODO: later create special service for file management tasks. Controller action must be "thin"

            var result = await _service.GetByIdAsync(id);
            if(!result.IsSuccess) {
                return BadRequest(ApiResponse<CommentDto>.ErrorResponse(result.ErrorCode, result.ErrorMessage));
            }
            var comment = result.Value;
            if (comment == null || string.IsNullOrEmpty(comment.FilePath)) {
                return NotFound(new { success = false, message = "File not found" });
            }

            string fileName = comment.FileName ?? "download";
            string filePath = comment.FilePath.TrimStart('/');

            if (!System.IO.File.Exists(filePath)) {
                return NotFound(new { success = false, message = "File not found on disk" });
            }

            var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);

            return File(fileBytes, comment.ContentType ?? "application/octet-stream", fileName);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id) {
            //TODO: later create special service for file management tasks. Controller action must be "thin"

            var result = await _service.GetByIdAsync(id);
            if (!result.IsSuccess) {
                return BadRequest(ApiResponse<CommentDto>.ErrorResponse(result.ErrorCode, result.ErrorMessage));
            }

            var filePath = result.Value.FilePath;
            await _service.DeleteAsync(id);

            if (!string.IsNullOrEmpty(filePath) && filePath.StartsWith("/uploads/")) {
                filePath = filePath.TrimStart('/');
                if (System.IO.File.Exists(filePath)) {
                    System.IO.File.Delete(filePath);
                }
            }

            return Ok();
        }
    }
}