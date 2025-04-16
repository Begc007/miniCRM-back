using miniCRM_back.Models;

namespace miniCRM_back.DTOs {
    public class CommentForCreateDto:CreateDtoBase {
        public int UserId { get; set; }
        public required string Text { get; set; }
        public int TaskItemId { get; set; }

        //Attachment
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public string? ContentType { get; set; }
        public long? Size { get; set; }
    }
}
