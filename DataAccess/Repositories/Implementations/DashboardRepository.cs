using DataAccess.Context;
using DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories.Implementations
{
    public class DashboardRepository :IDashboardRepository
    {
        private readonly ApplicationDbContext _context;

        public DashboardRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> GetTotalTaskCount(DateTime startDate, DateTime endDate)
        {
            return await _context.ProjectTasks.Where(t => t.CreatedOn >= startDate && t.CreatedOn <= endDate).CountAsync();
        }

        public async Task<Dictionary<int, int>> GetTaskCountsByStatus(DateTime startDate, DateTime endDate)
        {
            return await _context.ProjectTasks
                .Where(t => t.CreatedOn >= startDate && t.CreatedOn <= endDate)
                .GroupBy(t => t.StatusId)
                .Select(g => new { StatusId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.StatusId, v => v.Count);
        }

        public async Task<Dictionary<int, int>> GetTaskCountsByPriority(DateTime startDate, DateTime endDate)
        {
            return await _context.ProjectTasks
                .Where(t => t.CreatedOn >= startDate && t.CreatedOn <= endDate)
                .GroupBy(t => t.PriorityId)
                .Select(g => new { PriorityId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(k => k.PriorityId, v => v.Count);
        }



    }
}
