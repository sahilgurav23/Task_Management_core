namespace DTOs.Responses
{
    /// <summary>
    /// Contains task details and the abstract security permissions for the requesting user.
    /// </summary>
    public class TaskEditContextResponseDto
    {
        public TaskDetailsResponseDto TaskDetails { get; set; } = new();

        /// <summary>
        /// If true, the user can edit Title, Description, Priority, Due Date, and Assignee.
        /// </summary>
        public bool CanEditDetails { get; set; }

        /// <summary>
        /// If true, the user can edit the Status field.
        /// </summary>
        public bool CanEditStatus { get; set; }
    }
}
