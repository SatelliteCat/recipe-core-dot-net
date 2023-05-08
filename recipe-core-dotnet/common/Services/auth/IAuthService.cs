using recipe_core_dotnet.common.Models;

namespace recipe_core_dotnet.common.Services.auth;

public interface IAuthService
{
    Task<bool> IsValidUserAsync(string email, string password);
    string HashPassword(string password);
    public Task<string> CreateToken(User user);
    string GetPasswordHashSalt();
    string GetJwtIssuer();
    Task<User?> GetUserByEmailAsync(string email);
}