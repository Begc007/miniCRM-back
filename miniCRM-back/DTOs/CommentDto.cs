using miniCRM_back.Models;

namespace miniCRM_back.DTOs {
    public class CommentDto:ReadDtoBase {
        public int UserId { get; set; }
        public required string Text { get; set; }
        public int TaskItemId { get; set; }
        public TaskItem? TaskItem { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public string? ContentType { get; set; }
        public long? Size { get; set; }
    }
}
