namespace DTOs.Requests
{
    /// <summary>
    /// Request payload for fetching paginated and filtered tasks.
    /// Inherits PageNumber, PageSize, and SearchTerm from PaginationRequestDto.
    /// </summary>
    public class TaskFilterRequestDto : PaginationRequestDto
    {
        /// <summary>
        /// Optional filter for the task status ID.
        /// </summary>
        public int? StatusId { get; set; }
    }
}
