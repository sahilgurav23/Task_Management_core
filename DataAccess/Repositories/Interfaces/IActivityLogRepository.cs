namespace DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Handles database operations for the ActivityLog entity.
    /// </summary>
    public interface IActivityLogRepository
    {
        /// <summary>
        /// Fetches the chronological activity history for a specific task.
        /// </summary>
        Task<IEnumerable<DTOs.Responses.TaskActivityResponseDto>> GetTaskActivities(Guid taskId);
    }
}
