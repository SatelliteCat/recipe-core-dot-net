using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using recipe_core_dotnet.ApiV1.Dto;
using recipe_core_dotnet.common.Services.auth;

namespace recipe_core_dotnet.apiV1.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly IAuthService _authService;

    public AuthController(ILogger<AuthController> logger, IAuthService authService)
    {
        _logger = logger;
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [EnableRateLimiting("TokenBucketPolicy")]
    public async Task<IActionResult> Login([FromBody] AuthLoginDto dto)
    {
        if (!await _authService.IsValidUserAsync(dto.Email, dto.Password))
        {
            return BadRequest(new { message = "Invalid email or password" });
        }

        var user = await _authService.GetUserByEmailAsync(dto.Email);

        return Ok(new { token = await _authService.CreateToken(user!) });
    }
}