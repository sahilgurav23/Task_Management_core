using System.ComponentModel.DataAnnotations;

namespace DTOs.Requests
{
    /// <summary>
    /// Payload required to create a new task.
    /// </summary>
    public class CreateTaskRequestDto
    {
        /// <summary>
        /// The headline or title of the task. Required.
        /// </summary>
        [Required(ErrorMessage = "Task title is required.")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Detailed explanation of the task. Required.
        /// </summary>
        [Required(ErrorMessage = "Task description is required.")]
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Enum ID representing the task priority. Required.
        /// </summary>
        [Required]
        [Range(1, 4, ErrorMessage = "Invalid priority level.")]
        public int PriorityId { get; set; }

        /// <summary>
        /// Enum ID representing the initial status. Required.
        /// </summary>
        [Required]
        [Range(1, 4, ErrorMessage = "Invalid status level.")]
        public int StatusId { get; set; }

        /// <summary>
        /// The Guid of the user who will execute the task. Required.
        /// </summary>
        [Required]
        public Guid AssigneeId { get; set; }

        /// <summary>
        /// The date the task must be completed by. Required.
        /// </summary>
        [Required]
        public DateTime DueDate { get; set; }
    }
}
