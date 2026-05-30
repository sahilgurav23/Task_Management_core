using DataAccess.Context;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class ReportRepository : IReportRepository
    {
        private readonly ApplicationDbContext context;

        public ReportRepository(ApplicationDbContext Context)
        {
            context = Context;
        }

        public async Task<Dictionary<string, int>> GetCompletedTasksByMonth(DateTime startDate, int doneStatusId)
        {
            var query = await context.ProjectTasks
                .AsNoTracking()
                .Where(t => t.StatusId == doneStatusId && t.UpdatedOn >= startDate)
                .GroupBy(t => new { t.UpdatedOn!.Value.Year, t.UpdatedOn.Value.Month })
                .Select(g => new
                {
                    YearMonth = g.Key.Year + "-" + g.Key.Month.ToString("D2"),
                    Count = g.Count()
                })
                .ToListAsync();

            return query.ToDictionary(x => x.YearMonth, x => x.Count);
        }

        public async Task<Dictionary<DateTime, int>> GetCompletedTasksByDay(DateTime startDate, int doneStatusId)
        {
            var query = await context.ProjectTasks
                .AsNoTracking()
                .Where(t => t.StatusId == doneStatusId && t.UpdatedOn >= startDate)
                .GroupBy(t => t.UpdatedOn!.Value.Date) 
                .Select(g => new
                {
                    Date = g.Key,
                    Count = g.Count()
                })
                .ToDictionaryAsync(x => x.Date, x => x.Count);

            return query;
        }
    }
}
