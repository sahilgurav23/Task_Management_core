using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    /// <summary>
    /// In this We store the Profile Details
    /// </summary>
    public class Profile
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        public string EmailAddress { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public int Role {  get; set; } 
    }
}
