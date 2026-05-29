namespace DTOs.Requests
{
    /// <summary>
    /// Payload for updating a task. All fields are optional to support partial updates based on permissions.
    /// </summary>
    public class UpdateTaskRequestDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? PriorityId { get; set; }
        public Guid? AssignedUserId { get; set; }
        public DateTime? DueDate { get; set; }
        public int? StatusId { get; set; }
    }
}
