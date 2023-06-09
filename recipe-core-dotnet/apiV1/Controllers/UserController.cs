using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using recipe_core_dotnet.ApiV1.Dto;
using recipe_core_dotnet.common.Models;
using recipe_core_dotnet.common.Services.users;

namespace recipe_core_dotnet.apiV1.Controllers;

[ApiController]
[Route("api/v1/users")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly UserService _userService;

    public UserController(ILogger<UserController> logger, UserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [HttpGet]
    [Authorize(Roles = "admin")]
    [EnableRateLimiting("TokenBucketPolicy")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _userService.GetAllUsersAsync());
    }

    [HttpPost, AllowAnonymous]
    [EnableRateLimiting("TokenBucketPolicy")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
    {
        User? user = new User()
        {
            Email = dto.Email,
            PasswordHash = dto.Password,
            Uuid = Guid.NewGuid().ToString()
        };

        try
        {
            user = await _userService.RegisterUserAsync(user, dto.Role);
        }
        catch (UserServiceException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error in user register: {e.Message}");

            return BadRequest(new { message = "Failed to create user." });
        }

        if (user == null)
        {
            return BadRequest(new { message = "Failed to create user." });
        }

        return Ok(user);
    }
}