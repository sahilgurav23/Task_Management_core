using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    /// <summary>
    /// In this We store the Profile Details
    /// </summary>
    public class Profile
    {
        public Guid Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        public string EmailAddress { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public int Role { get; set; }

        public string? ProfileImagePath { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
