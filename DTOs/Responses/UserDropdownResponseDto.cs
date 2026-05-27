namespace DTOs.Responses
{
    /// <summary>
    /// Lightweight user representation specifically for UI dropdown selections.
    /// </summary>
    public class UserDropdownResponseDto
    {
        /// <summary>
        /// The unique user identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The display name of the user.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// The absolute URL to the user's profile image (if any).
        /// </summary>
        public string? ProfileImageUrl { get; set; }
    }
}
