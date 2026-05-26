namespace DTOs.Responses
{
    /// <summary>
    /// Lightweight profile data optimized specifically for navigation bars and headers.
    /// </summary>
    public class NavigationProfileResponseDto
    {
        /// <summary>
        /// The full identity name of the authenticated user to display in the header.
        /// </summary>
        public string FullName { get; set; } = string.Empty;

        /// <summary>
        /// High-resolution vertical or square avatar absolute URL path. 
        /// Returns null if no image has been processed.
        /// </summary>
        public string? ProfileImageUrl { get; set; }
    }
}
