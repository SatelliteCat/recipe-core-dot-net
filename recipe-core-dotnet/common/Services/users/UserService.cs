using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using recipe_core_dotnet.common.Models;
using recipe_core_dotnet.common.Services.auth;

namespace recipe_core_dotnet.common.Services.users;

public class UserService
{
    private readonly ILogger<UserService> _logger;
    private readonly AppDbContext _context;
    private readonly IAuthService _authService;
    private readonly UserManager<User> _userManager;

    public UserService(ILogger<UserService> logger, AppDbContext context, IAuthService authService,
        UserManager<User> userManager)
    {
        _logger = logger;
        _context = context;
        _authService = authService;
        _userManager = userManager;
    }

    public Task<List<User>> GetAllUsersAsync()
    {
        return _context.Users.ToListAsync();
    }

    public async Task<User?> RegisterUserAsync(User user, string? role = "user")
    {
        if (await _context.Users.Where(u => u.Email == user.Email).AnyAsync())
        {
            throw new UserServiceException("Email already exists.");
        }

        if (role != null && !await _context.Roles.Where(r => r.Name == role).AnyAsync())
        {
            throw new UserServiceException("Role not found.");
        }

        user.PasswordHash = _authService.HashPassword(user.PasswordHash!);
        user.SecurityStamp = Guid.NewGuid().ToString();
        user.ConcurrencyStamp = Guid.NewGuid().ToString();

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        await _userManager.AddToRoleAsync(user, role ?? "user");
        await _context.SaveChangesAsync();

        return user;
    }
}