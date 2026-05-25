using System.ComponentModel.DataAnnotations;

namespace DTOs.Common
{
    /// <summary>
    /// Standard API response wrapper.
    /// </summary>
    public class ApiResponseDto<T>
    {
        /// <summary>
        /// Indicates whether request was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Response message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Response data.
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Validation or exception errors.
        /// </summary>
        public List<string> Errors { get; set; } = new();
    }

}