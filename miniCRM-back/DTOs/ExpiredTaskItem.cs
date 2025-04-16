using miniCRM_back.Models;

namespace miniCRM_back.DTOs {
    public class ExpiredTaskItem:TaskItem {
        // TODO: here is some calculation issues. need to review again later
        public int ExpiredDays { get { return ExpiredAt.HasValue ? (int)Math.Floor((DateTime.Now - ExpiredAt.Value).TotalDays) : 0; } }
    }
}
