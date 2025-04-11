using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;

namespace miniCRM_back.Configs {
    public class PaginationParams {
        private const int MaxPageSize = 50;
        private int _pageSize = 10;

        [FromQuery]
        public int PageNumber { get; set; } = 1;

        [FromQuery]
        public int PageSize {
            get => _pageSize;
            set => _pageSize = Math.Min(value, MaxPageSize);
        }

        [FromQuery]
        public string SortBy { get; set; } = string.Empty;

        [FromQuery]
        public string SortDirection { get; set; } = "asc";

        [BindNever]
        public bool IsSortDirectionDescending =>
            !string.IsNullOrEmpty(SortDirection) &&
            SortDirection.ToLower() == "desc";
    }
}
