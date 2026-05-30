using Business.Services.Interfaces;
using DTOs.Common;
using DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TaskFlowPro.Controllers.Admin
{
    /// <summary>
    /// Handles graph and chart reporting for Admin analytics.
    /// </summary>
    [Route("api/admin/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IAdminReportService reportService;

        public ReportController(IAdminReportService ReportService)
        {
            reportService = ReportService;
        }

        private Guid GetCurrentUserId()
        {
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
        /// Retrieves the task completion trend data formatted specifically for line and bar graphs.
        /// Includes zero-filled continuous timelines for frontend rendering stability.
        /// </summary>
        /// <returns>A 200 OK containing monthly and daily graph datasets.</returns>
        [HttpGet("completion-trends")]
        [ProducesResponseType(typeof(ApiResponseDto<AdminReportResponseDto>), 200)]
        public async Task<IActionResult> GetCompletionTrends()
        {
            var response = await reportService.GetCompletionTrends();
            return Ok(response);
        }
    }
}
