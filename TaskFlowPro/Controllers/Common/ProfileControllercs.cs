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
        /// Extracts the user ID from the JWT claims.
        /// </summary>
        private Guid GetCurrentUserId()
        {
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
