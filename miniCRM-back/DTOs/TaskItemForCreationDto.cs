namespace miniCRM_back.DTOs {
    public class TaskItemForCreationDto:CreateDtoBase {
        public required string Name { get; set; }
        public string? Details { get; set; }
        public float Percent { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public DateTime? TargetDate { get; set; }
        public int UserId { get; set; }
        //public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
        //public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
