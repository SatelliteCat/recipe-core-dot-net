using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    [Authorize]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _userService.GetAllUsersAsync());
    }

    [HttpPost, AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto dto)
    {
        User? user = null;

        try
        {
            user = await _userService.RegisterUserAsync(new User()
            {
                Email = dto.Email,
                Password = dto.Password,
                Uuid = Guid.NewGuid().ToString()
            });
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");

            return UnprocessableEntity(new
            {
                message = "Failed to create user"
            });
        }

        if (user == null)
        {
            return BadRequest(new
            {
                message = "Email exists"
            });
        }

        return Ok(user);
    }
}