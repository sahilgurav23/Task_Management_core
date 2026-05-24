namespace DTOs.Responses
{
    public class LoginResponseDto
    {
        public bool IsValid { get; set; }
        public string Message { get; set; } = string.Empty;
        public Guid? ProfileId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public int? Role { get; set; }
    }
}
