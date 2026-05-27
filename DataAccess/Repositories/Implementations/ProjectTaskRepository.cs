using Data.Entities;
using DataAccess.Context;
using DataAccess.Repositories.Interfaces;

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
    }
}
