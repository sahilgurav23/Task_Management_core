using DTOs.Requests;
using DTOs.Responses;
using DTOs.Common;


namespace Business.Services.Interfaces
{
    /// <summary>
    /// Defines the business logic operations for user profiles.
    /// </summary>
    public interface IProfileService
    {
        /// <summary>
        /// Fetches the profile details for a given user ID.
        /// </summary>
        Task<ApiResponseDto<ProfileDetailsResponseDto>> GetProfileAsync(Guid userId, string baseUrl);

        /// <summary>
        /// Updates the user's profile, including optional password changes and image uploads.
        /// </summary>
        Task<ApiResponseDto<ProfileDetailsResponseDto>> UpdateProfileAsync(Guid userId, UpdateProfileRequestDto request, string baseUrl);
    }
}
