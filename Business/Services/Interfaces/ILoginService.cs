using DTOs.Requests;
using DTOs.Responses;

namespace Business.Services.Interfaces
{
    public interface ILoginService
    {
        Task<LoginResponseDto> ValidateLoginAsync(LoginRequestDto request);
    }
}
