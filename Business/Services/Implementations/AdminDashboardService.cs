using Business.Services.Interfaces;
using Data.Enums;
using DataAccess.Repositories.Interfaces;
using DTOs.Common;
using DTOs.Requests;
using DTOs.Responses;

namespace Business.Services.Implementations
{
    public class AdminDashboardService: IAdminDashboardService
    {
        private readonly IDashboardRepository dashboardRepository;

        public AdminDashboardService(IDashboardRepository DashboardRepository)
        {
            dashboardRepository = DashboardRepository;
        }

        public async Task<ApiResponseDto<AdminDashboardResponseDto>> GetDashboardData(DashboardFilterRequestDto filter)
        {
            var (startDate, endDate) = CalculateDateRange((DashboardTimeFilter)filter.TimeFilter);

            var totalTasks = await dashboardRepository.GetTotalTaskCount(startDate, endDate);
            var statusCounts = await dashboardRepository.GetTaskCountsByStatus(startDate, endDate);
            var priorityCounts = await dashboardRepository.GetTaskCountsByPriority(startDate, endDate);

            var barChartData = Enum.GetValues(typeof(StatusEnum)).Cast<StatusEnum>()
                .Select(status => new DashboardBarChartDto
                {
                    StatusId = (int)status,
                    StatusName = status.ToString(),
                    TaskCount = statusCounts.TryGetValue((int)status, out int count) ? count : 0
                }).ToList();

            int completedTasks = barChartData.FirstOrDefault(x => x.StatusId == (int)StatusEnum.Done)?.TaskCount ?? 0;
            var summary = new DashboardSummaryDto
            {
                TotalTasks = totalTasks,
                CompletedTasks = completedTasks,
                PendingTasks = totalTasks - completedTasks
            };

            var pieChartData = new DashboardPieChartContextDto
            {
                TotalTasks = totalTasks,
                Priorities = Enum.GetValues(typeof(PriorityEnum)).Cast<PriorityEnum>().Select(priority =>
                {
                    int pCount = priorityCounts.TryGetValue((int)priority, out int count) ? count : 0;
                    return new DashboardPieChartItemDto
                    {
                        PriorityId = (int)priority,
                        PriorityName = priority.ToString(),
                        TaskCount = pCount,
                        Percentage = totalTasks > 0 ? Math.Round(((double)pCount / totalTasks) * 100, 2) : 0
                    };
                }).ToList()
            };

            return new ApiResponseDto<AdminDashboardResponseDto>
            {
                Success = true,
                Message = $"Admin dashboard data fetched successfully for filter: {((DashboardTimeFilter)filter.TimeFilter).ToString()}.",
                Data = new AdminDashboardResponseDto { Summary = summary, BarChartData = barChartData, PieChartData = pieChartData }
            };
        }

        /// <summary>
        /// Private helper to resolve relative dates based on the current UTC time.
        /// </summary>
        private (DateTime StartDate, DateTime EndDate) CalculateDateRange(DashboardTimeFilter filter)
        {
            var now = DateTime.UtcNow;
            DateTime startDate;
            DateTime endDate;

            switch (filter)
            {
                case DashboardTimeFilter.ThisWeek:
                    int diff = (7 + (now.DayOfWeek - DayOfWeek.Monday)) % 7;
                    startDate = now.AddDays(-1 * diff).Date; 
                    endDate = startDate.AddDays(7).AddTicks(-1);
                    break;

                case DashboardTimeFilter.ThisMonth:
                    startDate = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                    endDate = startDate.AddMonths(1).AddTicks(-1);
                    break;

                case DashboardTimeFilter.ThisYear:
                    startDate = new DateTime(now.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                    endDate = startDate.AddYears(1).AddTicks(-1);
                    break;

                default:
                    goto case DashboardTimeFilter.ThisWeek;
            }

            return (startDate, endDate);
        }
    }
}
