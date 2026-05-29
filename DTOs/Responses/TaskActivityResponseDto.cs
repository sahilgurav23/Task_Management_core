namespace DTOs.Responses
{
    /// <summary>
    /// Represents a single activity log entry for a task.
    /// </summary>
    public class TaskActivityResponseDto
    {
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; }
        public string CreatedByName { get; set; } = string.Empty;
    }
}
