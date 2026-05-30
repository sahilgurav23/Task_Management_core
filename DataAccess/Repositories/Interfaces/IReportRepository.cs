namespace DataAccess.Repositories.Interfaces
{
    public interface IReportRepository
    {
        /// <summary>
        /// Groups completed tasks by Month and Year.
        /// </summary>
        Task<Dictionary<string, int>> GetCompletedTasksByMonth(DateTime startDate, int doneStatusId);

        /// <summary>
        /// Groups completed tasks strictly by their Date portion.
        /// </summary>
        Task<Dictionary<DateTime, int>> GetCompletedTasksByDay(DateTime startDate, int doneStatusId);
    }
}
