namespace miniCRM_back.Models {
    public class Comment:BaseEntity {
        public int UserId { get; set; }
        public required string Text { get; set; }
        public int TaskItemId { get; set; }
        public TaskItem? TaskItem { get; set; }
    }
}
