using DTOs.Common;
using DTOs.Requests;
using DTOs.Responses;

namespace Business.Services.Interfaces
{
    /// <summary>
    /// Contains core business rules for task management.
    /// </summary>
    public interface IProjectTaskService
    {
        /// <summary>
        /// Validates and processes the creation of a new task.
        /// </summary>
        Task<ApiResponseDto<Guid>> CreateTask(CreateTaskRequestDto request, Guid currentUserId);

        /// <summary>
        /// Retrieves the paginated and filtered list of tasks for the data table.
        /// </summary>
        Task<ApiResponseDto<PaginatedResponseDto<TaskListResponseDto>>> GetTaskList(Guid userId, TaskFilterRequestDto filter, string baseUrl);

        /// <summary>
        /// Retrieves the details of a specific task, formatting image URLs appropriately.
        /// </summary>
        Task<ApiResponseDto<TaskDetailsResponseDto>> GetTaskDetails(Guid taskId, string baseUrl);

        /// <summary>
        /// Retrieves the activity log history for a specific task.
        /// </summary>
        Task<ApiResponseDto<IEnumerable<TaskActivityResponseDto>>> GetTaskActivities(Guid taskId);

        /// <summary>
        /// Retrieves the task details along with UI permissions for the requesting user.
        /// </summary>
        Task<ApiResponseDto<TaskEditContextResponseDto>> GetTaskEditContext(Guid taskId, Guid currentUserId, string baseUrl);

        /// <summary>
        /// Securely processes a task update, silently discarding fields the user is not authorized to change.
        /// </summary>
        Task<ApiResponseDto<bool>> UpdateTask(Guid taskId, UpdateTaskRequestDto request, Guid currentUserId);

        /// <summary>
        /// Securely validates permissions and updates a task's status to Done.
        /// </summary>
        Task<ApiResponseDto<bool>> MarkTaskAsDone(Guid taskId, Guid currentUserId);
    }
}
