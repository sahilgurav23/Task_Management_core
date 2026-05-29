namespace Data.Entities
{
    /// <summary>
    /// Tracks historical actions and updates performed on tasks.
    /// </summary>
    public class ActivityLog
    {
        /// <summary>
        /// The unique identifier for the log entry.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The ID of the task this activity is associated with.
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// A human-readable description of the action performed.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The ID of the user who performed the action.
        /// </summary>
        public Guid CreatedByUserId { get; set; }

        /// <summary>
        /// The exact UTC date and time the activity occurred.
        /// </summary>
        public DateTime CreatedOn { get; set; }
    }
}
