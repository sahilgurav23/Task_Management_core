using Data.Entities;
using DTOs.Responses;

namespace DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Handles database operations for the ProjectTask entity.
    /// </summary>
    public interface IProjectTaskRepository
    {
        /// <summary>
        /// Saves a new task into the database.
        /// </summary>
        Task<Guid> Create(ProjectTask task);

        /// <summary>
        /// Fetches a paginated, filtered, and searched list of tasks joined with assignee profiles.
        /// </summary>
        Task<(IEnumerable<TaskListResponseDto> Tasks, int TotalCount)> GetTaskTableData(Guid userId, string? searchTerm, int? statusId, int pageNumber, int pageSize);

        /// <summary>
        /// Saves a new task and its initial activity log to the database in a single transaction.
        /// </summary>
        /// <param name="task">The prepared task entity.</param>
        /// <param name="log">The prepared activity log entity.</param>
        /// <returns>The ID of the newly created task.</returns>
        Task<Guid> CreateWithLog(ProjectTask task, ActivityLog log);

        /// <summary>
        /// Retrieves highly optimized task details joined with the assignee's profile.
        /// </summary>
        Task<TaskDetailsResponseDto?> GetTaskDetails(Guid taskId);

        /// <summary>
        /// Fetches a raw task entity by its ID for tracking and updating.
        /// </summary>
        Task<Data.Entities.ProjectTask?> GetById(Guid taskId);

        /// <summary>
        /// Updates an existing task and inserts an activity log in a single transaction.
        /// </summary>
        Task UpdateWithLog(ProjectTask task, ActivityLog log);

        /// <summary>
        /// Fetches only the IDs required to verify access permissions for a task, minimizing memory allocation.
        /// </summary>
        Task<(Guid CreatedById, Guid AssignedUserId, int StatusId)?> GetTaskSecurityContext(Guid taskId);

        /// <summary>
        /// Executes a lightning-fast partial SQL update for the status, and inserts an activity log.
        /// </summary>
        Task UpdateTaskStatusWithLog(Guid taskId, int newStatusId, ActivityLog log);
    }
}
