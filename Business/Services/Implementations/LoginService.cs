using Business.Services.Interfaces;
using DataAccess.Repositories.Interfaces;
using DTOs.Requests;
using DTOs.Responses;

namespace Business.Services.Implementations
{
    public class LoginService : ILoginService
    {
        private readonly ILoginRepository loginRepository;

        public LoginService(ILoginRepository loginRepository)
        {
            this.loginRepository = loginRepository;
        }

        public async Task<LoginResponseDto> ValidateLoginAsync(LoginRequestDto request)
        {
            var profile = await loginRepository.GetProfileByCredentialsAsync(request.Email, request.Password);

            if (profile == null)
            {
                return new LoginResponseDto
                {
                    IsValid = false,
                    Message = "Invalid email or password."
                };
            }

            return new LoginResponseDto
            {
                IsValid = true,
                Message = "Login successful.",
                ProfileId = profile.Id,
                FullName = profile.FullName,
                EmailAddress = profile.EmailAddress,
                Role = profile.Role
            };
        }
    }
}
