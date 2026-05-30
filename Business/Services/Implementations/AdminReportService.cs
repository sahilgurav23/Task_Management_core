using Business.Services.Interfaces;
using Data.Enums;
using DataAccess.Repositories.Interfaces;
using DTOs.Common;
using DTOs.Responses;

namespace Business.Services.Implementations
{
    public class AdminReportService : IAdminReportService
    {
        private readonly IReportRepository reportRepository;

        public AdminReportService(IReportRepository ReportRepository)
        {
            reportRepository = ReportRepository;
        }

        public async Task<ApiResponseDto<AdminReportResponseDto>> GetCompletionTrends()
        {
            var now = DateTime.UtcNow;
            int doneStatusId = (int)StatusEnum.Done;

            var monthStartDate = now.AddMonths(-11).Date; 
            var dayStartDate = now.AddDays(-29).Date;

            var rawMonthlyData = await reportRepository.GetCompletedTasksByMonth(monthStartDate, doneStatusId);
            var rawDailyData = await reportRepository.GetCompletedTasksByDay(dayStartDate, doneStatusId);

            var monthlyTrend = new List<MonthlyCompletionDto>();
            for (int i = 11; i >= 0; i--)
            {
                var targetMonth = now.AddMonths(-i);
                var key = $"{targetMonth.Year}-{targetMonth.Month:D2}"; 

                monthlyTrend.Add(new MonthlyCompletionDto
                {
                    MonthLabel = targetMonth.ToString("MMM yyyy"), 
                    TaskCount = rawMonthlyData.TryGetValue(key, out int count) ? count : 0
                });
            }

            var dailyTrend = new List<DailyCompletionDto>();
            for (int i = 29; i >= 0; i--)
            {
                var targetDay = now.AddDays(-i).Date;

                dailyTrend.Add(new DailyCompletionDto
                {
                    DateLabel = targetDay.ToString("dd MMM"),
                    TaskCount = rawDailyData.TryGetValue(targetDay, out int count) ? count : 0
                });
            }

            return new ApiResponseDto<AdminReportResponseDto>
            {
                Success = true,
                Message = "Completion trends generated successfully.",
                Data = new AdminReportResponseDto
                {
                    MonthlyTrend = monthlyTrend,
                    DailyTrend = dailyTrend
                }
            };
        }
    }
}
