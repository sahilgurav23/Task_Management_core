using Business.Services.Interfaces;
using Data.Entities;
using DataAccess.Repositories.Interfaces;
using DTOs.Common;
using DTOs.Requests;
using DTOs.Responses;

namespace Business.Services.Implementations
{
    public class ProjectTaskService : IProjectTaskService
    {
        private readonly IProjectTaskRepository taskRepository;

        public ProjectTaskService(IProjectTaskRepository taskRepository)
        {
            this.taskRepository = taskRepository;
        }

        public async Task<ApiResponseDto<Guid>> CreateTask(CreateTaskRequestDto request, Guid currentUserId)
        {
            // Business Rule: Strip time from DueDate to strictly store date only
            var sanitizedDueDate = request.DueDate.Date;

            var newTask = new ProjectTask
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                PriorityId = request.PriorityId,
                StatusId = request.StatusId,
                AssignedUserId = request.AssigneeId,
                DueDate = sanitizedDueDate,
                CreatedById = currentUserId,
                CreatedOn = DateTime.UtcNow
            };

            var taskId = await taskRepository.Create(newTask);

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
    }
}
