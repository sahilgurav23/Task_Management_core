using DTOs.Common;
using DTOs.Requests;

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
    }
}
