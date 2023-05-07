using Microsoft.EntityFrameworkCore;
using recipe_core_dotnet.common.Models;
using recipe_core_dotnet.common.Services.auth;

namespace recipe_core_dotnet.common.Services.users;

public class UserService
{
    private readonly ILogger<UserService> _logger;
    private readonly AppDbContext _context;
    private readonly IAuthService _authService;

    public UserService(ILogger<UserService> logger, AppDbContext context, IAuthService authService)
    {
        _logger = logger;
        _context = context;
        _authService = authService;
    }

    public Task<List<User>> GetAllUsersAsync()
    {
        return _context.Users.ToListAsync();
    }

    public async Task<User?> RegisterUserAsync(User user)
    {
        var existsUser = await _context.Users.Where(u => u.Email == user.Email).AnyAsync();

        if (existsUser)
        {
            return null;
        }

        user.Password = _authService.HashPassword(user.Password);

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        return user;
    }
}