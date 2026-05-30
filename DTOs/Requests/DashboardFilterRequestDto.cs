using Data.Enums;
namespace DTOs.Requests
{
    /// <summary>
    /// Request payload for filtering the Admin Dashboard.
    /// </summary>
    public class DashboardFilterRequestDto
    {
        /// <summary>
        /// 1 = This Week, 2 = This Month, 3 = This Year. 
        /// Defaults to 1 (This Week).
        /// </summary>
        public int TimeFilter { get; set; } = (int)DashboardTimeFilter.ThisWeek;
    }
}
