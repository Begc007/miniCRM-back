namespace miniCRM_back.Models {
    public class Comment:BaseEntity {
        public int UserId { get; set; }
        public User? User { get; set; }
        public required string Text { get; set; }
        public int TaskItemId { get; set; }
        public TaskItem? TaskItem { get; set; }

        //Attachment
        public byte[]? File { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public string? ContentType { get; set; }
        public long? Size { get; set; }
    }
}
