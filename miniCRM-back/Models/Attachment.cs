namespace miniCRM_back.Models {
    public class Attachment:BaseEntity {
        public required string FileName { get; set; }
        public required string FilePath { get; set; }
        public required string ContentType { get; set; }
        public long Size { get; set; }
        public string? Description { get; set; }
        public int TaskItemId { get; set; }
        public TaskItem? TaskItem { get; set; }
    }
}
