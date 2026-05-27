namespace DTOs.Requests
{
    /// <summary>
    /// Standardized request for paginated and searchable data.
    /// </summary>
    public class PaginationRequestDto
    {
        /// <summary>
        /// The current page number (defaults to 1).
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// The number of records per page (defaults to 10).
        /// </summary>
        public int PageSize { get; set; } = 10;

        /// <summary>
        /// Optional search keyword to filter records.
        /// </summary>
        public string? SearchTerm { get; set; }
    }
}
