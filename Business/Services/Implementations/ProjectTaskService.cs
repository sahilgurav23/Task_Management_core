using Business.Services.Interfaces;
using Data.Entities;
using DataAccess.Repositories.Implementations;
using DataAccess.Repositories.Interfaces;
using DTOs.Common;
using DTOs.Requests;
using DTOs.Responses;

namespace Business.Services.Implementations
{
    public class ProjectTaskService : IProjectTaskService
    {
        private readonly IProjectTaskRepository taskRepository;
        private readonly IActivityLogRepository activityLogRepository;

        public ProjectTaskService(IProjectTaskRepository TaskRepository, IActivityLogRepository ActivityLogRepository)
        {
            taskRepository = TaskRepository;
            activityLogRepository = ActivityLogRepository;
        }

        public async Task<ApiResponseDto<Guid>> CreateTask(CreateTaskRequestDto request, Guid currentUserId)
        {
            var sanitizedDueDate = request.DueDate.Date;

            var taskId = Guid.NewGuid();
            var currentTime = DateTime.UtcNow;

            var newTask = new ProjectTask
            {
                Id = taskId,
                Title = request.Title,
                Description = request.Description,
                PriorityId = request.PriorityId,
                StatusId = request.StatusId,
                AssignedUserId = request.AssigneeId,
                DueDate = sanitizedDueDate,
                CreatedById = currentUserId,
                CreatedOn = currentTime
            };

            var initialLog = new ActivityLog
            {
                Id = Guid.NewGuid(),
                TaskId = taskId,
                Description = "Created Task",
                CreatedByUserId = currentUserId,
                CreatedOn = currentTime
            };

            await taskRepository.CreateWithLog(newTask, initialLog);

            return new ApiResponseDto<Guid>
            {
                Success = true,
                Message = "Task created successfully.",
                Data = taskId
            };
        }
        /// <summary>
        /// Processes task table data, applying base URLs to images and structuring the pagination response.
        /// </summary>
        public async Task<ApiResponseDto<PaginatedResponseDto<TaskListResponseDto>>> GetTaskList(Guid userId, TaskFilterRequestDto filter, string baseUrl)
        {
            int pageNumber = filter.PageNumber > 0 ? filter.PageNumber : 1;
            int pageSize = filter.PageSize > 0 ? filter.PageSize : 10;

            var (tasks, totalCount) = await taskRepository.GetTaskTableData(
                userId,
                filter.SearchTerm,
                filter.StatusId,
                pageNumber,
                pageSize);

            foreach (var task in tasks)
            {
                if (!string.IsNullOrEmpty(task.AssigneeImageUrl))
                    task.AssigneeImageUrl = $"{baseUrl}/{task.AssigneeImageUrl.TrimStart('/')}";
            }

            var paginatedData = new PaginatedResponseDto<TaskListResponseDto>
            {
                Items = tasks,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return new ApiResponseDto<PaginatedResponseDto<TaskListResponseDto>>
            {
                Success = true,
                Message = "Task list fetched successfully.",
                Data = paginatedData
            };
        }

        /// <summary>
        /// Fetches task details and applies environment-specific base URLs for images.
        /// </summary>
        public async Task<ApiResponseDto<TaskDetailsResponseDto>> GetTaskDetails(Guid taskId, string baseUrl)
        {
            var taskDetails = await taskRepository.GetTaskDetails(taskId);

            if (taskDetails == null)
            {
                return new ApiResponseDto<TaskDetailsResponseDto>
                {
                    Success = false,
                    Message = "Task not found.",
                    Errors = new List<string> { $"No task exists with ID: {taskId}" }
                };
            }

            if (!string.IsNullOrEmpty(taskDetails.AssigneeImageUrl))
                taskDetails.AssigneeImageUrl = $"{baseUrl}/{taskDetails.AssigneeImageUrl.TrimStart('/')}";

            return new ApiResponseDto<TaskDetailsResponseDto>
            {
                Success = true,
                Message = "Task details fetched successfully.",
                Data = taskDetails
            };
        }

        /// <summary>
        /// Fetches chronological task activities. No base URL required as this is text-only.
        /// </summary>
        public async Task<ApiResponseDto<IEnumerable<TaskActivityResponseDto>>> GetTaskActivities(Guid taskId)
        {
            var activities = await activityLogRepository.GetTaskActivities(taskId);

            return new ApiResponseDto<IEnumerable<TaskActivityResponseDto>>
            {
                Success = true,
                Message = "Task activities fetched successfully.",
                Data = activities
            };
        }
    }
}
