using Business.Services.Implementations;
using Business.Services.Interfaces;
using DTOs.Common;
using DTOs.Requests;
using DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace TaskFlowPro.Controllers.Common
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileControllercs : Controller
    {
        private readonly IProfileService profileService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileController"/>.
        /// </summary>
        public ProfileControllercs(IProfileService ProfileService)
        {
            profileService = ProfileService;
        }


        /// <summary>
        /// Retrieves the profile details of the currently authenticated user.
        /// </summary>
        [HttpGet("me")]
        [ProducesResponseType(typeof(ApiResponseDto<ProfileDetailsResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponseDto<ProfileDetailsResponseDto>), 404)]
        public async Task<IActionResult> GetMyProfile()
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return Unauthorized(CreateErrorResponse("Invalid user token."));

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var response = await profileService.GetProfileAsync(userId, baseUrl);

            if (!response.Success)
                return NotFound(response); // Returns 404

            return Ok(response); // Returns 200
        }

        /// <summary>
        /// Updates the profile details of the currently authenticated user.
        /// Consumes multipart/form-data to support file uploads.
        /// </summary>
        /// <param name="request">The profile update request payload.</param>
        /// <returns>A 200 OK on success, or a 400 Bad Request if validation fails.</returns>
        [HttpPut("me")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(ApiResponseDto<ProfileDetailsResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponseDto<ProfileDetailsResponseDto>), 400)]
        public async Task<IActionResult> UpdateMyProfile([FromForm] UpdateProfileRequestDto request)
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return Unauthorized(CreateErrorResponse("Invalid user token."));

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var response = await profileService.UpdateProfileAsync(userId, request, baseUrl);

            if (!response.Success)
                return BadRequest(response); // Returns 400 for bad password or logic errors

            return Ok(response); // Returns 200
        }


        /// <summary>
        /// Fetches high-performance minified profile data strictly for header/top navigation layouts.
        /// </summary>
        /// <returns>
        /// Returns 200 OK containing name and optimized image URL link context.
        /// Returns 401 Unauthorized if access token signature verification is missing or corrupted.
        /// Returns 404 Not Found if authenticated claim matches no database profile record.
        /// </returns>
        [HttpGet("nav-details")]
        [ProducesResponseType(typeof(ApiResponseDto<NavigationProfileResponseDto>), 200)]
        [ProducesResponseType(typeof(ApiResponseDto<NavigationProfileResponseDto>), 401)]
        [ProducesResponseType(typeof(ApiResponseDto<NavigationProfileResponseDto>), 404)]
        public async Task<IActionResult> GetNavigationDetails()
        {
            var userId = GetCurrentUserId();
            if (userId == Guid.Empty)
                return Unauthorized(CreateNavigationErrorResponse("Token identity missing validation context."));

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var response = await profileService.GetNavigationProfileAsync(userId, baseUrl);

            if (!response.Success)
                return NotFound(response);

            return Ok(response);
        }


        /// <summary>
        /// Fetches a paginated, searchable list of users for task assignment dropdowns.
        /// </summary>
        /// <returns>200 OK with paginated list.</returns>
        [HttpGet("assignee-dropdown")]
        [ProducesResponseType(typeof(ApiResponseDto<PaginatedResponseDto<UserDropdownResponseDto>>), 200)]
        public async Task<IActionResult> GetAssigneeDropdown([FromQuery] PaginationRequestDto request)
        {
            // Ensure minimum defaults are respected if client sends 0 or negatives
            if (request.PageNumber < 1) request.PageNumber = 1;
            if (request.PageSize < 1) request.PageSize = 10;

            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var response = await profileService.GetAssigneeDropdown(request, baseUrl);

            return Ok(response);
        }



        private ApiResponseDto<NavigationProfileResponseDto> CreateNavigationErrorResponse(string message)
        {
            return new ApiResponseDto<NavigationProfileResponseDto>
            {
                Success = false,
                Message = message,
                Errors = new List<string> { "Authentication failed context validation." }
            };
        }


        /// <summary>
        /// Extracts the user ID from the custom header or JWT claims.
        /// </summary>
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
            if (Guid.TryParse(userIdClaim, out Guid userId))
            {
                return userId;
            }
            return Guid.Empty;
        }

        /// <summary>
        /// Creates a standard unauthorized error response.
        /// </summary>
        private ApiResponseDto<ProfileDetailsResponseDto> CreateErrorResponse(string message)
        {
            return new ApiResponseDto<ProfileDetailsResponseDto>
            {
                Success = false,
                Message = message,
                Errors = new List<string> { "Authentication failed." }
            };
        }

    }
}
