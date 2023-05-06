namespace recipe_core_dotnet.common.Services.auth;

public interface IAuthService
{
    Task<bool> IsValidUserAsync(string email, string password);
    string HashPassword(string password);
    string CreateToken(string email);
    string GetPasswordHashSalt();
}