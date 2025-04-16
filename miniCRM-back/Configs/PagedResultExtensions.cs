namespace miniCRM_back.Configs {
    public static class PagedResultExtensions {
        public static PaginationMetadata GetPaginationMetadata<T>(this PagedResult<IEnumerable<T>> result) {
            return new PaginationMetadata {
                CurrentPage = result.Pagination.CurrentPage,
                TotalPages = result.Pagination.TotalPages,
                PageSize = result.Pagination.PageSize,
                TotalCount = result.Pagination.TotalCount
            };
        }
    }
}
