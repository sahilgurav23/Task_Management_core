namespace DTOs.Responses
{
    /// <summary>
    /// Represents a single row in the task data table.
    /// </summary>
    public class TaskListResponseDto
    {
        /// <summary>
        /// The unique task identifier.
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// The headline or title of the task.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// The string representation of the Priority (e.g., "High", "Medium").
        /// </summary>
        public string Priority { get; set; } = string.Empty;

        /// <summary>
        /// The numeric ID of the priority, useful if the frontend needs to color-code badges.
        /// </summary>
        public int PriorityId { get; set; }

        /// <summary>
        /// The full name of the assigned user.
        /// </summary>
        public string AssigneeName { get; set; } = string.Empty;

        /// <summary>
        /// The absolute URL path to the assignee's profile image.
        /// </summary>
        public string? AssigneeImageUrl { get; set; }

        /// <summary>
        /// The date the task must be completed by.
        /// </summary>
        public DateTime DueDate { get; set; }
    }
}
