using DTOs.Common;
using DTOs.Responses;

namespace Business.Services.Interfaces
{
    public interface IAdminReportService
    {
        /// <summary>
        /// Generates timeline-filled graph data for task completions.
        /// </summary>
        Task<ApiResponseDto<AdminReportResponseDto>> GetCompletionTrends();
    }
}
