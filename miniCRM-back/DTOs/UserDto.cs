using miniCRM_back.Models;

namespace miniCRM_back.DTOs {
    public class UserDto:ReadDtoBase {
        public required string Name { get; set; }
        //public required string Password { get; set; }
        public string? FIO { get; set; } //TODO: check if the field can be nullable in ТЗ
        public string? Position { get; set; } //TODO: check if the field can be nullable in ТЗ
        //public ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
    }
}
