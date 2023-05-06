using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> Login([FromBody] AuthLoginDto dto)
    {
        if (!await _authService.IsValidUserAsync(dto.Email, dto.Password))
        {
            // return Unauthorized();
            return BadRequest(new { message = "Invalid email or password" });
        }

        return Ok(new { token = _authService.CreateToken(dto.Email) });
    }
}