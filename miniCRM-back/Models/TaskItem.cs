﻿namespace miniCRM_back.Models {
    public class TaskItem:BaseEntity {
        public required string Name { get; set; }
        public string? Details { get; set; }
        public float Percent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public int Complexity { get; set; }
        public int UserId { get; set; }
        public User? User { get; set; }
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
