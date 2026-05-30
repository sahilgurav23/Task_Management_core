using Business.Services.Interfaces;
using DTOs.Common;
using DTOs.Requests;
using DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TaskFlowPro.Controllers.Admin
{
    /// <summary>
    /// Handles administrative analytics and aggregated data views.
    /// </summary>
    [Route("api/admin/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _dashboardService;

        public DashboardController(IAdminDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        private Guid GetCurrentUserId()
        {
            // First try to get from custom header sent by UI
            if (Request.Headers.TryGetValue("X-User-Guid", out var headerUserId))
            {
                if (Guid.TryParse(headerUserId, out Guid guidFromHeader))
                    return guidFromHeader;
            }

            // Fallback to JWT claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out Guid userId) ? userId : Guid.Empty;
        }

        /// <summary>
        /// Fetches highly optimized, aggregated data for the Admin Dashboard UI.
        /// </summary>
        /// <param name="filter">Time filter query (1 = ThisWeek, 2 = ThisMonth, 3 = ThisYear). Defaults to 1.</param>
        /// <returns>A 200 OK with the full dashboard payload.</returns>
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponseDto<AdminDashboardResponseDto>), 200)]
        public async Task<IActionResult> GetDashboardOverview([FromQuery] DashboardFilterRequestDto filter)
        {
            var response = await _dashboardService.GetDashboardData(filter);

            return Ok(response);
        }
    }
}
