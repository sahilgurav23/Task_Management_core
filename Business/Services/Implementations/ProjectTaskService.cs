using Business.Services.Interfaces;
using Data.Entities;
using Data.Enums;
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

        public async Task<ApiResponseDto<TaskEditContextResponseDto>> GetTaskEditContext(Guid taskId, Guid currentUserId, string baseUrl)
        {
            var rawTask = await taskRepository.GetById(taskId);

            if (rawTask == null)
                return new ApiResponseDto<TaskEditContextResponseDto> { Success = false, Message = "Task not found." };

            var detailsResponse = await GetTaskDetails(taskId, baseUrl);

            bool isCreator = rawTask.CreatedById == currentUserId;
            bool isAssignee = rawTask.AssignedUserId == currentUserId;

            return new ApiResponseDto<TaskEditContextResponseDto>
            {
                Success = true,
                Message = "Edit context retrieved.",
                Data = new TaskEditContextResponseDto
                {
                    TaskDetails = detailsResponse.Data!,
                    CanEditDetails = isCreator,
                    CanEditStatus = isCreator || isAssignee
                }
            };
        }

        public async Task<ApiResponseDto<bool>> UpdateTask(Guid taskId, UpdateTaskRequestDto request, Guid currentUserId)
        {
            var task = await taskRepository.GetById(taskId);

            if (task == null)
                return new ApiResponseDto<bool> { Success = false, Message = "Task not found." };

            bool isCreator = task.CreatedById == currentUserId;
            bool isAssignee = task.AssignedUserId == currentUserId;

            if (!isCreator && !isAssignee)
            {
                return new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = "Forbidden: You do not have permission to edit this task."
                };
            }

            string logDescription = "";

            if (isCreator)
            {
                if (!string.IsNullOrWhiteSpace(request.Title)) task.Title = request.Title;
                if (!string.IsNullOrWhiteSpace(request.Description)) task.Description = request.Description;
                if (request.PriorityId.HasValue) task.PriorityId = request.PriorityId.Value;
                if (request.DueDate.HasValue) task.DueDate = request.DueDate.Value.Date;
                if (request.AssignedUserId.HasValue) task.AssignedUserId = request.AssignedUserId.Value;
                if (request.StatusId.HasValue) task.StatusId = request.StatusId.Value;

                logDescription = "Updated task details.";
            }
            else if (isAssignee)
            {
                if (request.StatusId.HasValue)
                {
                    if (task.StatusId != request.StatusId.Value)
                    {
                        task.StatusId = request.StatusId.Value;
                        logDescription = $"Updated task status to {((StatusEnum)task.StatusId).ToString()}.";
                    }
                    else
                    {
                        // Status is the same, but still return success since the user has permission
                        return new ApiResponseDto<bool> { Success = true, Message = "Task status unchanged.", Data = true };
                    }
                }
                else
                {
                    // Assignee trying to update other fields without statusId
                    return new ApiResponseDto<bool> { Success = false, Message = "Forbidden: Assignees can only update task status." };
                }
            }

            task.UpdatedOn = DateTime.UtcNow;

            var log = new ActivityLog
            {
                Id = Guid.NewGuid(),
                TaskId = task.Id,
                Description = logDescription,
                CreatedByUserId = currentUserId,
                CreatedOn = DateTime.UtcNow
            };

            await taskRepository.UpdateWithLog(task, log);

            return new ApiResponseDto<bool>
            {
                Success = true,
                Message = "Task updated successfully.",
                Data = true
            };
        }

        public async Task<ApiResponseDto<bool>> MarkTaskAsDone(Guid taskId, Guid currentUserId)
        {
            var securityContext = await taskRepository.GetTaskSecurityContext(taskId);
        
            if (securityContext == null)
                return new ApiResponseDto<bool> { Success = false, Message = "Task not found." };
        
            bool isCreator = securityContext.Value.CreatedById == currentUserId;
            bool isAssignee = securityContext.Value.AssignedUserId == currentUserId;
        
            if (!isCreator && !isAssignee)
            {
                return new ApiResponseDto<bool>
                {
                    Success = false,
                    Message = "Forbidden: You are not authorized to complete this task."
                };
            }
        
            if (securityContext.Value.StatusId == (int)StatusEnum.Done)
            {
                return new ApiResponseDto<bool>
                {
                    Success = true,
                    Message = "Task is already marked as Done.",
                    Data = true
                };
            }
        
            var log = new ActivityLog
            {
                Id = Guid.NewGuid(),
                TaskId = taskId,
                Description = "Marked task as Done.",
                CreatedByUserId = currentUserId,
                CreatedOn = DateTime.UtcNow
            };
        
            await taskRepository.UpdateTaskStatusWithLog(taskId, (int)StatusEnum.Done, log);
        
            return new ApiResponseDto<bool>
            {
                Success = true,
                Message = "Task successfully marked as Done.",
                Data = true
            };
        }
    }
    }
