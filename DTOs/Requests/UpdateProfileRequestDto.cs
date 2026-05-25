using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Requests
{
    /// <summary>
    /// Update profile request details.
    /// </summary>
    public class UpdateProfileRequestDto
    {
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
        /// Profile image file.
        /// Optional.
        /// </summary>
        public IFormFile? ProfileImage { get; set; }

        /// <summary>
        /// Current password for verification.
        /// Optional.
        /// Required when changing password.
        /// </summary>
        public string? OldPassword { get; set; }

        /// <summary>
        /// New password.
        /// Optional.
        /// </summary>
        public string? NewPassword { get; set; }
    }
}
