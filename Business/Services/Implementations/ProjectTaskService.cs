using Business.Services.Interfaces;
using Data.Entities;
using DataAccess.Repositories.Interfaces;
using DTOs.Common;
using DTOs.Requests;

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
    }
}
