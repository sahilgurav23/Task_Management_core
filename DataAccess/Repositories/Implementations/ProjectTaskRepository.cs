using Data.Entities;
using DataAccess.Context;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Data.Enums;
namespace DataAccess.Repositories.Implementations
{
    public class ProjectTaskRepository : IProjectTaskRepository
    {
        private readonly ApplicationDbContext context;

        public ProjectTaskRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<Guid> Create(ProjectTask task)
        {
            await context.ProjectTasks.AddAsync(task);
            await context.SaveChangesAsync();
            return task.Id;
        }

        /// <summary>
        /// Implements an optimized inner join between ProjectTasks and Profiles.
        /// </summary>
        public async Task<(IEnumerable<DTOs.Responses.TaskListResponseDto> Tasks, int TotalCount)> GetTaskTableData(Guid userId, string? searchTerm, int? statusId, int pageNumber, int pageSize)
        {
            var query = from pt in context.ProjectTasks.AsNoTracking()
                        join p in context.Profiles.AsNoTracking()
                        on pt.AssignedUserId equals p.Id
                        where pt.AssignedUserId == userId
                        select new { pt, p };

            if (statusId.HasValue)
                query = query.Where(x => x.pt.StatusId == statusId.Value);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var lowerSearch = searchTerm.ToLower();
                query = query.Where(x => x.pt.Title.ToLower().Contains(lowerSearch) || x.p.FullName.ToLower().Contains(lowerSearch));
            }

            int totalCount = await query.CountAsync();

            var tasks = await query.OrderByDescending(x => x.pt.CreatedOn)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(x => new DTOs.Responses.TaskListResponseDto
                {
                    TaskId = x.pt.Id,
                    Title = x.pt.Title,
                    PriorityId = x.pt.PriorityId,
                    Priority = ((PriorityEnum)x.pt.PriorityId).ToString(),
                    AssigneeName = x.p.FullName,
                    AssigneeImageUrl = x.p.ProfileImagePath,
                    DueDate = x.pt.DueDate
                })
                .ToListAsync();

            return (tasks, totalCount);
        }

        /// <summary>
        /// Adds both the task and the log to the DbContext and commits them together.
        /// This is highly optimized as EF Core will batch the INSERT statements into a single database roundtrip.
        /// </summary>
        public async Task<Guid> CreateWithLog(ProjectTask task, ActivityLog log)
        {
            // Add both entities to the change tracker
            await context.ProjectTasks.AddAsync(task);
            await context.ActivityLogs.AddAsync(log);

            // Execute a single save operation for both inserts
            await context.SaveChangesAsync();

            return task.Id;
        }
    }
}
