using Data.Entities;

namespace DataAccess.Repositories.Interfaces
{
    /// <summary>
    /// Handle dashboard operations for the profile entity
    /// </summary>
    public interface IProfileRepository
    {
        /// <summary>
        /// Retrieves a profile by its unique identifier.
        /// </summary>
        Task<Profile> GetById(Guid id);

        /// <summary>
        /// Updates an existing profile in the database.
        /// </summary>
        Task UpdateProfile(Profile profile);

        /// <summary>
        /// Retrieves only the structural naming and image path attributes of a user 
        /// optimized for light payload extraction.
        /// </summary>
        Task<(string FullName, string? ProfileImagePath)?> GetNavigationDataById(Guid id);
    }
}
