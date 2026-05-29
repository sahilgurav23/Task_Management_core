using Business.Services.Interfaces;
using DTOs.Common;
using DTOs.Requests;
using DTOs.Responses;
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
                    return guidFromHeader;
            }

            // Fallback to JWT claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out Guid userId) ? userId : Guid.Empty;
        }

        /// <summary>
        /// Fetches a paginated, searchable, and filtered list of tasks for the data table.
        /// </summary>
        /// <param name="filter">The query string parameters for filtering and pagination.</param>
        /// <returns>A 200 OK containing the paginated data table results.</returns>
        [HttpGet("list")]
        [ProducesResponseType(typeof(ApiResponseDto<PaginatedResponseDto<TaskListResponseDto>>), 200)]
        public async Task<IActionResult> GetTaskList([FromQuery] TaskFilterRequestDto filter)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return Unauthorized(new ApiResponseDto<object>
                {
                    Success = false,
                    Message = "Invalid user token."
                });

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var response = await taskService.GetTaskList(userId, filter, baseUrl);

            return Ok(response);
        }


        /// <summary>
        /// Retrieves full details for a specific task based on its ID.
        /// </summary>
        /// <param name="id">The unique identifier of the task.</param>
        /// <returns>A 200 OK with the task details or 404 Not Found.</returns>
        [HttpGet("{id:guid}/details")]
        [ProducesResponseType(typeof(ApiResponseDto<TaskDetailsResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponseDto<TaskDetailsResponseDto>), 404)]
        public async Task<IActionResult> GetTaskDetails(Guid id)
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var response = await taskService.GetTaskDetails(id, baseUrl);

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        /// <summary>
        /// Retrieves the historical activity logs for a specific task.
        /// </summary>
        /// <param name="id">The unique identifier of the task.</param>
        /// <returns>A 200 OK containing an array of activity entries.</returns>
        [HttpGet("{id:guid}/activities")]
        [ProducesResponseType(typeof(ApiResponseDto<IEnumerable<TaskActivityResponseDto>>), 200)]
        public async Task<IActionResult> GetTaskActivities(Guid id)
        {
            var response = await taskService.GetTaskActivities(id);

            return Ok(response);
        }


        /// <summary>
        /// Retrieves the task details along with abstract boolean permissions to configure the UI Edit form safely.
        /// </summary>
        [HttpGet("{id:guid}/edit-context")]
        [ProducesResponseType(typeof(ApiResponseDto<TaskEditContextResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponseDto<TaskEditContextResponseDto>), 404)]
        public async Task<IActionResult> GetTaskEditContext(Guid id)
        {
            var userId = GetCurrentUserId();
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var response = await taskService.GetTaskEditContext(id, userId, baseUrl);

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }

        /// <summary>
        /// Updates a task. Securely ignores fields the current user is not authorized to edit.
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), 200)]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), 403)]
        [ProducesResponseType(typeof(ApiResponseDto<bool>), 404)]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] UpdateTaskRequestDto request)
        {
            var userId = GetCurrentUserId();

            var response = await taskService.UpdateTask(id, request, userId);

            if (!response.Success)
            {
                // Return 403 Forbidden if the business layer rejected due to permissions
                if (response.Message.Contains("Forbidden"))
                    return StatusCode(403, response);

                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
