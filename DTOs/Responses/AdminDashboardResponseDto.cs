namespace DTOs.Responses
{
    /// <summary>
    /// The complete data wrapper for the Admin Dashboard.
    /// </summary>
    public class AdminDashboardResponseDto
    {
        public DashboardSummaryDto Summary { get; set; } = new();
        public IEnumerable<DashboardBarChartDto> BarChartData { get; set; } = new List<DashboardBarChartDto>();
        public DashboardPieChartContextDto PieChartData { get; set; } = new();
    }

    public class DashboardSummaryDto
    {
        public int TotalTasks { get; set; }
        public int CompletedTasks { get; set; }
        public int PendingTasks { get; set; }
    }

    public class DashboardBarChartDto
    {
        public int StatusId { get; set; }
        public string StatusName { get; set; } = string.Empty;
        public int TaskCount { get; set; }
    }

    public class DashboardPieChartContextDto
    {
        public int TotalTasks { get; set; }
        public IEnumerable<DashboardPieChartItemDto> Priorities { get; set; } = new List<DashboardPieChartItemDto>();
    }

    public class DashboardPieChartItemDto
    {
        public int PriorityId { get; set; }
        public string PriorityName { get; set; } = string.Empty;
        public int TaskCount { get; set; }
        public double Percentage { get; set; }
    }
}
