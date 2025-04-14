namespace miniCRM_back.DTOs {
    public class UpdateDtoBase {
        public int Id { get; set; }
        public DateTime? UpdateTimestamp { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
