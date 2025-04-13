namespace miniCRM_back.DTOs {
    public class TaskItemsGroupByUser {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string FIO { get; set; }
        public string Position { get; set; }
        public int TaskItemCount { get; set; }
        public float CompletedPercent { get; set; }
    }
}
