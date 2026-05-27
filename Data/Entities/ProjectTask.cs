namespace Data.Entities
{
    /// <summary>
    /// Represents a task assigned to a user within the system.
    /// </summary>
    public class ProjectTask
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int PriorityId { get; set; }

        public int StatusId { get; set; }

        public Guid AssignedUserId { get; set; }

        /// <summary>
        /// Stores only the Date portion (Time is set to midnight UTC).
        /// </summary>
        public DateTime DueDate { get; set; }

        public Guid CreatedById { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
