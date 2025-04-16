using AutoMapper;
using Microsoft.EntityFrameworkCore;
using miniCRM_back.Configs;
using miniCRM_back.Database;
using miniCRM_back.DTOs;
using miniCRM_back.Models;
using miniCRM_back.Services.Contracts;

namespace miniCRM_back.Services {
    public class CommentService : BaseService<Comment, CommentDto, CommentForCreateDto, CommentDto>, ICommentService {
        public CommentService(IGenericRepository<Comment> repository, ILogger<BaseService<Comment, CommentDto, CommentForCreateDto, CommentDto>> logger, IMapper mapper) : base(repository, logger, mapper) {
        }

        public async Task<Result<IEnumerable<CommentDto>>> GetByTaskIdAsync(int taskId) {
            try {
                var noPaginationParams = new PaginationParams {
                    PageNumber = 1,
                    PageSize = 500,
                    SortBy = "Id",
                    SortDirection = "asc"
                };

                var comments = await repository.GetWithCustomQueryAsync(noPaginationParams,
                    query => query.Where(c => c.TaskItemId == taskId).OrderBy(c => c.Id));

                var dtos = mapper.Map<IEnumerable<CommentDto>>(comments);
                return Result<IEnumerable<CommentDto>>.Success(dtos);
            } catch (Exception ex) {
                return Result<IEnumerable<CommentDto>>.Failure("InternalServerError", ex.Message);
            }
        }
    }
}
