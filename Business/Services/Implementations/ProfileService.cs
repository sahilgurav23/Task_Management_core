using Business.Services.Helpers;
using Business.Services.Interfaces;
using Data.Entities;
using DataAccess.Repositories.Interfaces;
using DTOs.Common;
using DTOs.Requests;
using DTOs.Responses;

namespace Business.Services.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository profileRepository;
        private readonly ImageHelper imageHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileService"/>.
        /// </summary>
        public ProfileService(IProfileRepository profileRepository, ImageHelper image)
        {
            this.profileRepository = profileRepository;
            imageHelper = image;
        }


        /// <summary>
        /// Retrieves profile details and maps them to the response DTO.
        /// </summary>
        public async Task<ApiResponseDto<ProfileDetailsResponseDto>> GetProfileAsync(Guid userId, string baseUrl)
        {
            var profile = await profileRepository.GetById(userId);

            if (profile == null)
            {
                return new ApiResponseDto<ProfileDetailsResponseDto>
                {
                    Success = false,
                    Message = "Profile not found.",
                    Errors = new List<string> { $"No user found with ID: {userId}" }
                };
            }

            return new ApiResponseDto<ProfileDetailsResponseDto>
            {
                Success = true,
                Message = "Profile fetched successfully.",
                Data = MapToResponseDto(profile, baseUrl)
            };
        }

        /// <summary>
        /// Updates the profile data, handles secure password validation, and processes mobile-optimized image uploads.
        /// </summary>
        public async Task<ApiResponseDto<ProfileDetailsResponseDto>> UpdateProfileAsync(Guid userId, UpdateProfileRequestDto request, string baseUrl)
        {
            var profile = await profileRepository.GetById(userId);

            if (profile == null)
            {
                return new ApiResponseDto<ProfileDetailsResponseDto>
                {
                    Success = false,
                    Message = "Profile not found.",
                    Errors = new List<string> { $"No user found with ID: {userId}" }
                };
            }

            profile.FullName = request.FullName;
            profile.EmailAddress = request.EmailAddress;
            profile.UpdatedOn = DateTime.UtcNow;

            if (!string.IsNullOrEmpty(request.NewPassword))
            {
                if (string.IsNullOrEmpty(request.OldPassword) || profile.Password != request.OldPassword)
                {
                    return new ApiResponseDto<ProfileDetailsResponseDto>
                    {
                        Success = false,
                        Message = "Password update failed.",
                        Errors = new List<string> { "Invalid current password provided." }
                    };
                }
                profile.Password = request.NewPassword; 
            }

            if (request.ProfileImage != null && request.ProfileImage.Length > 0)
            {
                var imagePath = await imageHelper.ProcessAndSaveVerticalMediaAsync(request.ProfileImage);
                profile.ProfileImagePath = imagePath;
            }

            await profileRepository.UpdateProfile(profile);

            return new ApiResponseDto<ProfileDetailsResponseDto>
            {
                Success = true,
                Message = "Profile updated successfully.",
                Data = MapToResponseDto(profile, baseUrl)
            };
        }


        /// <summary>
        /// Private helper to map the Profile entity to the ProfileDetailsResponseDto.
        /// </summary>
        private ProfileDetailsResponseDto MapToResponseDto(Profile profile, string baseUrl)
        {
            return new ProfileDetailsResponseDto
            {
                Id = profile.Id,
                FullName = profile.FullName,
                EmailAddress = profile.EmailAddress,
                Role = profile.Role,
                ProfileImageUrl = string.IsNullOrEmpty(profile.ProfileImagePath) ? null : $"{baseUrl}/{profile.ProfileImagePath.TrimStart('/')}"
            };
        }
    }
}
