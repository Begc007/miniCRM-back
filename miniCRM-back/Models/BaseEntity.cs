namespace miniCRM_back.Models {
    public abstract class BaseEntity {
        public int Id { get; set; }
        public DateTime CreateTimestamp { get; set; } = DateTime.UtcNow;
        public DateTime? UpdateTimestamp { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
