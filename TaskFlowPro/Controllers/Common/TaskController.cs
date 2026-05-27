using Business.Services.Interfaces;
using DTOs.Common;
using DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TaskFlowPro.Controllers.Common
{

    /// <summary>
    /// Handles task creation and management endpoints for all roles.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : Controller
    {
        private readonly IProjectTaskService taskService;

        public TaskController(IProjectTaskService taskService)
        {
            this.taskService = taskService;
        }

        /// <summary>
        /// Creates a new task.
        /// </summary>
        /// <returns>201 Created on success, 400 Bad Request on validation failure.</returns>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseDto<Guid>), 201)]
        [ProducesResponseType(typeof(ApiResponseDto<Guid>), 400)]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponseDto<Guid>
                {
                    Success = false,
                    Message = "Validation failed.",
                    Errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()
                });
            }

            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return Unauthorized();

            var response = await taskService.CreateTask(request, userId);

            // Using 201 Created as best practice for POST/Creation
            return Created(string.Empty, response);
        }

        private Guid GetCurrentUserId()
        {
            // First try to get from custom header sent by UI
            if (Request.Headers.TryGetValue("X-User-Guid", out var headerUserId))
            {
                if (Guid.TryParse(headerUserId, out Guid guidFromHeader))
                {
                    return guidFromHeader;
                }
            }

            // Fallback to JWT claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out Guid userId) ? userId : Guid.Empty;
        }
    }
}
