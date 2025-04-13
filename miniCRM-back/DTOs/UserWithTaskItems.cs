namespace miniCRM_back.DTOs {
    public class UserWithTaskItems {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FIO { get; set; }
        public string Position { get; set; }
        public int TaskItemId { get; set; }
        public string TaskItemName { get; set; }
        public float TaskItemPercent { get; set; }
    }
}
