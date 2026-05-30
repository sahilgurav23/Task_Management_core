namespace DTOs.Responses
{
    /// <summary>
    /// Wrapper for the reporting graphs data.
    /// </summary>
    public class AdminReportResponseDto
    {
        /// <summary>
        /// Data for the Monthly Line Graph (Last 12 Months).
        /// </summary>
        public IEnumerable<MonthlyCompletionDto> MonthlyTrend { get; set; } = new List<MonthlyCompletionDto>();

        /// <summary>
        /// Data for the Daily Vertical Bar Graph (Last 30 Days).
        /// </summary>
        public IEnumerable<DailyCompletionDto> DailyTrend { get; set; } = new List<DailyCompletionDto>();
    }

    public class MonthlyCompletionDto
    {
        /// <summary>
        /// Formatted string for the X-Axis (e.g., "Jan 2026").
        /// </summary>
        public string MonthLabel { get; set; } = string.Empty;

        /// <summary>
        /// Total tasks completed in this month.
        /// </summary>
        public int TaskCount { get; set; }
    }

    public class DailyCompletionDto
    {
        /// <summary>
        /// Formatted string for the X-Axis (e.g., "15 May").
        /// </summary>
        public string DateLabel { get; set; } = string.Empty;

        /// <summary>
        /// Total tasks completed on this specific day.
        /// </summary>
        public int TaskCount { get; set; }
    }
}
