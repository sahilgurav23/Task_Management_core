namespace DataAccess.Repositories.Interfaces
{
    public interface IDashboardRepository
    {
        Task<int> GetTotalTaskCount(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets the count of tasks grouped by their StatusId.
        /// </summary>
        Task<Dictionary<int, int>> GetTaskCountsByStatus(DateTime startDate, DateTime endDate);

        /// <summary>
        /// Gets the count of tasks grouped by their PriorityId.
        /// </summary>
        Task<Dictionary<int, int>> GetTaskCountsByPriority(DateTime startDate, DateTime endDate);
    }
}
