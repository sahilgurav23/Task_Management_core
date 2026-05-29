namespace DTOs.Responses
{
    /// <summary>
    /// Represents the full details of a specific task.
    /// </summary>
    public class TaskDetailsResponseDto
    {
        public Guid TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int PriorityId { get; set; }
        public string Priority { get; set; } = string.Empty;
        public int StatusId { get; set; }
        public string Status { get; set; } = string.Empty;
        public Guid AssigneeId { get; set; }
        public string AssigneeName { get; set; } = string.Empty;
        public string? AssigneeImageUrl { get; set; }
        public DateTime DueDate { get; set; }
    }
}
