using Business.Services.Interfaces;
using DTOs.Requests;
using Microsoft.AspNetCore.Mvc;

namespace TaskFlowPro.Controllers.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        [HttpPost("validate")]
        public async Task<IActionResult> ValidateLogin([FromBody] LoginRequestDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _loginService.ValidateLoginAsync(request);

            if (!result.IsValid)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }
    }
}
