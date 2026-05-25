namespace DTOs.Responses
{
    /// <summary>
    /// Profile details response.
    /// </summary>
    public class ProfileDetailsResponseDto
    {
        /// <summary>
        /// Profile identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// User full name.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// User email address.
        /// </summary>
        public string EmailAddress { get; set; } = string.Empty;

        /// <summary>
        /// User role identifier.
        /// </summary>
        public int Role { get; set; }

        /// <summary>
        /// Profile image URL.
        /// </summary>
        public string? ProfileImageUrl { get; set; }
    }
}
