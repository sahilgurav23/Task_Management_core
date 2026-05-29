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
    }
}
