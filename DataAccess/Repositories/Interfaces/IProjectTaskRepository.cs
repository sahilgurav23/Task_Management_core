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
        Task<(IEnumerable<TaskListResponseDto> Tasks, int TotalCount)> GetTaskTableData( Guid userId, string? searchTerm, int? statusId, int pageNumber, int pageSize);
    }
}
