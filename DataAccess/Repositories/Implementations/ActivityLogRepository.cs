using DataAccess.Context;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class ActivityLogRepository: IActivityLogRepository
    {
        private readonly ApplicationDbContext context;

        public ActivityLogRepository(ApplicationDbContext Context)
        {
            context = Context;
        }

        /// <summary>
        /// Translates to an INNER JOIN between ActivityLogs and Profiles.
        /// </summary>
        public async Task<IEnumerable<DTOs.Responses.TaskActivityResponseDto>> GetTaskActivities(Guid taskId)
        {
            return await (from al in context.ActivityLogs.AsNoTracking()
                          join p in context.Profiles.AsNoTracking()
                          on al.CreatedByUserId equals p.Id
                          where al.TaskId == taskId
                          orderby al.CreatedOn descending
                          select new DTOs.Responses.TaskActivityResponseDto
                          {
                              Description = al.Description,
                              CreatedOn = al.CreatedOn,
                              CreatedByName = p.FullName
                          }).ToListAsync();
        }

    }
}
