using miniCRM_back.Configs;
using miniCRM_back.DTOs;
using miniCRM_back.Models;

namespace miniCRM_back.Services {
    public interface ICommentService:IBaseService<Comment, CommentDto, CommentForCreateDto, CommentDto> {
        Task<Result<IEnumerable<CommentDto>>> GetByTaskIdAsync(int taskId);
        
    }
}
