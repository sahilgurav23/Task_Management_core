using DTOs.Common;
using DTOs.Requests;
using DTOs.Responses;

namespace Business.Services.Interfaces
{
    public interface IAdminDashboardService
    {
        Task<ApiResponseDto<AdminDashboardResponseDto>> GetDashboardData(DashboardFilterRequestDto filter);
    }
}
